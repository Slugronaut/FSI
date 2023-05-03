using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SAOT.Model
{
    public abstract class PickOrdersDatabase
    {

        public const string DbFile = "pickorders.db";
        public static SQLiteConnection NewDbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;New=True;Compress=True;");
        public static SQLiteConnection DbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Compress=True;");
        public static SQLiteConnection DbReadonlyConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Read Only=True;Compress=True;");
        public static bool DatabaseExists => File.Exists(Config.DirectoryOfDb(DbFile)) ? true : false;

        static bool DbIsLocked { get => false; }
        static string DbLockId { get => "DebugUser"; }

        static void ValidateDBAction(User user)
        {
            if (user == null || !user.CanCreateOrder)
                throw new UserException("The current user cannot perform this action.");
            if (DbIsLocked)
                throw new DatabaseLockedException("Pick Orders", DbLockId);
        }

        /// <summary>
        /// Verfiies access to the model's database, opens a connection and performs the given action
        /// on it before closing the connection.
        /// </summary>
        /// <param name="action"></param>
        public static void DbAction(User user, Action<SQLiteConnection> action)
        {
            if (user == null)
            {
                using (var conn = DbReadonlyConnection)
                {
                    conn.Open();
                    action(conn);
                    conn.Close();
                }
            }
            else
            {
                if (DbAccess.DatabaseLocking)
                {
                    //TODO: Validate access to DB file here using the file-lock subsystem
                    //display error message if write access not allow.
                }
                using (var conn = DbConnection)
                {
                    conn.Open();
                    action(conn);
                    conn.Close();
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        #region Write
        /// <summary>
        /// 
        /// </summary>
        public static void CreateNewDatabase()
        {
            using (var conn = NewDbConnection)
            {
                conn.Open();

                //create order table
                string sql = @"CREATE TABLE pickOrders (
                orderId INTEGER PRIMARY KEY AUTOINCREMENT,
                createdBy INTEGER,
                orderType INTEGER,
                dateCreated TEXT,
                dateModified TEXT,
                destinationId INTEGER,
                inventoryId INTEGER,
                status INTEGER,
                adjustedInSAP INTEGER,
                carrierId INTEGER,
                carrierTracking TEXT
                );";
                var cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //create picklist table
                sql = @"CREATE TABLE pickItems (
                pickItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                orderId INTEGER,
                dateModified TEXT,
                status INTEGER,
                matId TEXT,
                requestedQty REAL,
                pickedQty REAL,
                pickedBy INTEGER,
                notes TEXT
                );";
                cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public static void ConfirmDatabaseExists()
        {
            if (!DatabaseExists)
                CreateNewDatabase();
        }

        /// <summary>
        /// 
        /// </summary>
        public static int CreatePickOrder(User user, Vendor shipDestId, Vendor invTransferId, OrderTypes orderType, List<Material> mats, List<decimal> requestedQtys)
        {
            if(mats == null || requestedQtys == null || mats.Count != requestedQtys.Count)
                throw new Exception("Number of materials does not equal number of pick quantities.");
            
            ValidateDBAction(user);
            if (mats.Count != requestedQtys.Count)
                throw new Exception("The number of materials does not match the number of pick quanities. If you see this message, tell James he's an idiot.");
            
            int orderId = -1;

            DbAction(user, (conn) =>
            {
                int destId;
                int invId;

                string dateCreated = JsonConvert.SerializeObject(DateTime.Now);
                if(orderType == OrderTypes.PickForProduction || orderType == OrderTypes.InventoryCount)
                {
                    destId = 0;
                    invId = 0;
                }
                else
                {
                    destId = shipDestId.Id;
                    invId = invTransferId.Id;
                }

                //insert the order in the pickOrders table
                var sql = $"INSERT INTO pickOrders (createdBy, orderType, dateCreated, dateModified, destinationId, inventoryId, status, adjustedInSAP, carrierId, carrierTracking) values ('{user.Id}', '{(int)orderType}', '{dateCreated}', '{dateCreated}', '{destId}', '{invId}', '{PickOrderStatuses.PickPending}', '0', '0', NULL);";
                var cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                orderId = (int)conn.LastInsertRowId;


                //now insert all pick items into the pickItems table
                for (int i = 0; i < mats.Count; i++)
                {
                    var mat = mats[i];
                    var qty = requestedQtys[i];

                    sql = $"INSERT INTO pickItems values (NULL, '{orderId}', '{dateCreated}', '{(int)PickItemStatuses.PickPending}', '{mat.MatId}', '{qty}', '0', '-1', NULL);";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            });


            return orderId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        public static void CancelPickOrder(User user, int orderId)
        {
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickOrders SET status = '{(int)PickItemStatuses.Canceled}' WHERE pickItemId = '{orderId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        /// <param name="mat"></param>
        /// <param name="requestQty"></param>
        public static void AddPickItems(User user, int orderId, List<Material> mats, List<decimal> requestedQtys)
        {
            if (mats == null || requestedQtys == null || mats.Count != requestedQtys.Count)
                throw new Exception("Number of materials does not equal number of pick quantities.");

            ValidateDBAction(user);
            if (mats.Count != requestedQtys.Count)
                throw new Exception("The number of materials does not match the number of pick quanities. If you see this message, tell James he's an idiot.");


            DbAction(user, (conn) =>
            {
                string dateCreated = JsonConvert.SerializeObject(DateTime.Now);
                
                //now insert all pick items into the pickItems table
                for (int i = 0; i < mats.Count; i++)
                {
                    var mat = mats[i];
                    var qty = requestedQtys[i];

                    var sql = $"INSERT INTO pickItems values (NULL, '{orderId}', '{dateCreated}', '{(int)PickItemStatuses.PickPending}', '{mat.MatId}', '{qty}', '0', '-1', NULL);";
                    var cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        /// <param name="mat"></param>
        public static bool CancelPickItem(User user, int orderId, Material mat)
        {
            var pickItem = RequestPickItem(orderId, mat);
            return CancelPickItem(user, pickItem.PickItemId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pickId"></param>
        public static bool CancelPickItem(User user, int pickId)
        {
            var item = RequestPickItem(pickId);
            if (item.Status == PickItemStatuses.Canceled) return false;

            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickItems SET status = '{(int)PickItemStatuses.Canceled}' WHERE pickItemId = '{pickId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });

            //now... we need to check the status of the order and update it based on if cancling this item
            //changes the entire order to 'complete' or not.
            UpdatePickOrderStatus(user, pickId);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        /// <param name="mat"></param>
        public static bool RestoreCanceledPickItem(User user, int orderId, Material mat)
        {
            var pickItem = RequestPickItem(orderId, mat);
            return RestoreCanceledPickItem(user, pickItem.PickItemId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pickId"></param>
        public static bool RestoreCanceledPickItem(User user, int pickId)
        {
            var item = RequestPickItem(pickId);
            if (item.Status != PickItemStatuses.Canceled) return false;

            int status = (int)(item.PickedQty >= item.RequestedQty ? PickItemStatuses.Picked : PickItemStatuses.PickPending);
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickItems SET status = '{status}' WHERE pickItemId = '{pickId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });

            //now... we need to check the status of the order and update it based on if cancling this item
            //changes the entire order to 'complete' or not.
            UpdatePickOrderStatus(user, pickId);
            return true;
        }

        /// <summary>
        /// Updates the pickorder status based on the status of all items in that order.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pickId"></param>
        static void UpdatePickOrderStatus(User user, int pickId)
        {
            bool allPicked = true;

            var pickList = PickOrdersDatabase.RequestPickList(pickId);
            var order = PickOrdersDatabase.RequestOrder(pickId);

            if(pickList != null && order != null)
            {
                foreach (var item in pickList)
                {
                    if (item.Status == PickItemStatuses.PickPending)
                        allPicked = false;
                }


                if (!allPicked)
                {
                    PickOrdersDatabase.SetOrderStateAsPending(user, pickId);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        /// <param name="mat"></param>
        /// <param name="pickedQty"></param>
        public static void PerformPick(User user, int orderId, Material mat, decimal pickedQty)
        {
            var pickItem = RequestPickItem(orderId, mat);
            PerformPick(user, pickItem.PickItemId, pickedQty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pickItem"></param>
        /// <param name="pickedQty"></param>
        public static void PerformPick(User user, PickListItem pickItem, decimal pickedQty)
        {
            PerformPick(user, pickItem.PickItemId, pickedQty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pickId"></param>
        /// <param name="pickedQty"></param>
        public static void PerformPick(User user, int pickId, decimal pickedQty)
        {
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickItems SET pickedQty = '{pickedQty}', pickedBy = '{user.Id}', status = '{(int)PickItemStatuses.Picked}' WHERE pickItemId = '{pickId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// Returns a previously picked item to its unpicked state.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pickId"></param>
        /// <param name="pickedQty"></param>
        public static void ReversePick(User user, int pickId)
        {
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickItems SET pickedQty = '{0}', pickedBy = '{-1}', status = '{(int)PickItemStatuses.PickPending}' WHERE pickItemId = '{pickId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        /// <param name="tracking"></param>
        public static void SetTracking(User user, int orderId, int carrierId, string tracking)
        {
            tracking = DbAccess.Sanitize(tracking);
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickOrders SET carrierId = '{carrierId}', carrierTracking = '{tracking}' WHERE orderId = '{orderId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        /// <param name="tracking"></param>
        public static void SetAdjusted(User user, int orderId, bool adjusted)
        {
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickOrders SET adjustedInSAP = '{Convert.ToInt32(adjusted)}' WHERE orderId = '{orderId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        /// <param name="tracking"></param>
        public static void CompletePick(User user, int orderId)
        {
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickOrders SET status = '{(int)PickOrderStatuses.Picked}' WHERE orderId = '{orderId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// Performs a systemic shipment that provides shipping labels and packing slips.
        /// This does not mean that the order has yet physically left the build but that
        /// is instead ready for physical pickup.
        /// </summary>
        /// <param name="orderId"></param>
        public static void ShipOrder(User user, int orderId)
        {
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickOrders SET status = '{(int)PickOrderStatuses.Shipped}' WHERE orderId = '{orderId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        public static void SetOrderStateAsPending(User user, int orderId)
        {
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickOrders SET status = '{(int)PickOrderStatuses.PickPending}' WHERE orderId = '{orderId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// This process finalizes the order and disallows it from further modification.
        /// It should be called when the order is physically being picked-up
        /// by a shipping carrier or being consumed in a production process.
        /// </summary>
        /// <param name="orderId"></param>
        public static void CloseOrder(User user, int orderId)
        {
            DbAction(user, (conn) =>
            {
                //TODO: write update sql
                string sql = $"UPDATE pickOrders SET status = '{(int)PickOrderStatuses.Closed}' WHERE orderId = '{orderId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }
        #endregion


        #region Read
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int NextOrderId()
        {
            int nextOrderId = 0;

            DbAction(null, (conn) =>
            {
                var sql = "SELECT MAX(orderId) FROM pickOrders";
                var cmd = new SQLiteCommand(sql, conn);
                var result = cmd.ExecuteScalar();
                if (result.GetType() == typeof(DBNull))
                    nextOrderId = 0;
                else nextOrderId = Convert.ToInt32(result);
            });

            return nextOrderId + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickId"></param>
        /// <returns></returns>
        public static bool IsTotalPicked(int pickId)
        {
            var item = RequestPickItem(pickId);
            return item.PickedQty >= item.RequestedQty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static PickOrder RequestOrder(int orderId)
        {
            PickOrder order = null;
            DbAction(null, (conn) =>
            {
                var sql = $"SELECT * FROM pickOrders WHERE orderId = '{orderId}';";
                using(var cmd = new SQLiteCommand(sql, conn))
                {
                    using(var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            try
                            {
                                order = ReadOrder(reader);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("There was an error while parsing one of the orders in the database.\n" + e.Message);
                            }
                        }
                        reader.Close();
                    }
                }
            });

            return order;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public static List<PickOrder> RequestOrdersWithMaterial(OrderTypes orderType, string materialId)
        {
            var materials = RequestPickListsWithMaterial(materialId);
            if (materials == null) return null;

            var orderIds = materials.GroupBy(x => x.OrderId).Select( y => y.First().OrderId);

            List<PickOrder> pickOrders = new List<PickOrder>(100);
            foreach(var orderId in orderIds)
            {
                var order = RequestOrder(orderId);
                if (order != null && ((int)orderType & order.OrderType) != 0)
                    pickOrders.Add(order);
            }

            return pickOrders.Count < 1 ? null : pickOrders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public static List<PickListItem> RequestPickListsWithMaterial(string materialId)
        {
            materialId = DbAccess.Sanitize(materialId);
            List<PickListItem> items = new List<PickListItem>(100);

            DbAction(null, (conn) =>
            {
                var sql = $"SELECT * FROM pickItems WHERE materialId = '{materialId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            try
                            {
                                var item = ReadPickItem(reader);
                                if (item != null)
                                    items.Add(item);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("There was an error while parsing one of the pick items in the database.\n" + e.Message);
                            }
                        }
                        reader.Close();
                    }
                }
            });

            return items.Count < 1 ? null : items;
        }

        /// <summary>
        /// Helper for reading sql data for a pick order and creating a data object from it.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        static PickOrder ReadOrder(SQLiteDataReader reader)
        {
            var trackingObj = reader["carrierTracking"];
            string trackingNum = null;
            if (trackingObj == null || trackingObj.GetType() == typeof(DBNull))
                trackingNum = string.Empty;
            else trackingNum = (string)trackingObj;

            return new PickOrder()
            {
                OrderId = Convert.ToInt32(reader["orderId"]),
                OrderType = Convert.ToInt32(reader["orderType"]),
                CreatedById = Convert.ToInt32(reader["createdBy"]),
                DateCreated = (DateTime)JsonConvert.DeserializeObject((string)reader["dateCreated"]),
                DateModified = (DateTime)JsonConvert.DeserializeObject((string)reader["dateModified"]),
                ShipToId = Convert.ToInt32(reader["destinationId"]),
                InventoryAdjustId = Convert.ToInt32(reader["inventoryId"]),
                Status = (PickOrderStatuses)(Convert.ToInt32(reader["status"])),
                AdjustedInSAP = Convert.ToBoolean(reader["adjustedInSAP"]),
                CarrierId = Convert.ToInt32(reader["carrierId"]),
                CarrierTracking = trackingNum,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="includePending"></param>
        /// <param name="includePicking"></param>
        /// <param name="includePicked"></param>
        /// <param name="includeShipped"></param>
        /// <param name="includeAdjusted"></param>
        /// <param name="includeClosed"></param>
        /// <returns></returns>
        public static List<PickOrder> RequestAllPickOrders(OrderTypes orderTypesFlag, bool includePending, bool includePicking, bool includePicked, bool includeShipped, bool includeAdjusted, bool includeClosed)
        {
            List<PickOrder> orders = new List<PickOrder>(50);
            DbAction(null, (conn) =>
            {
                var sql = $"SELECT * FROM pickOrders ORDER BY orderId ASC";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var order = ReadOrder(reader);
                                if (order.Status == PickOrderStatuses.PickPending && !includePending)
                                    continue;
                                if (order.Status == PickOrderStatuses.Picking && !includePicking)
                                    continue;
                                if (order.Status == PickOrderStatuses.Picked && !includePicked)
                                    continue;
                                if (order.Status == PickOrderStatuses.Shipped && !includeShipped)
                                    continue;
                                if (order.Status == PickOrderStatuses.Adjusted && !includeAdjusted)
                                    continue;
                                if (order.Status == PickOrderStatuses.Closed && !includeClosed)
                                    continue;

                                if (((int)order.OrderType & (int)orderTypesFlag) == 0)
                                    continue;

                                orders.Add(order);
                            }
                            catch(Exception e)
                            {
                                MessageBox.Show("Error #01: There was an error while parsing one of the orders in the database.\n" + e.Message);
                            }
                        }
                        reader.Close();
                    }
                }
               
            });
            return orders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        static PickListItem ReadPickItem(SQLiteDataReader reader)
        {
            var notesObj = reader["notes"];
            string notes = string.Empty;
            if (notesObj == null || notesObj.GetType() == typeof(DBNull))
                notes = string.Empty;

            return new PickListItem(Convert.ToInt32(reader["orderId"]))
            {
                PickItemId = Convert.ToInt32(reader["pickItemId"]),
                DateModified = (DateTime)JsonConvert.DeserializeObject((string)reader["dateModified"]),
                Status = (PickItemStatuses)(Convert.ToInt32(reader["status"])),
                MaterialId = (string)reader["matId"],
                RequestedQty = Convert.ToDecimal(reader["requestedQty"]),
                PickedQty = Convert.ToDecimal(reader["pickedQty"]),
                PickedById = Convert.ToInt32(reader["pickedBy"]),
                Notes = notes,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickItemId"></param>
        /// <returns></returns>
        public static PickListItem RequestPickItem(int pickItemId)
        {
            PickListItem item = null;
            DbAction(null, (conn) =>
            {
                var sql = $"SELECT * FROM pickItems WHERE pickItemId = '{pickItemId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            try
                            {
                                item = ReadPickItem(reader);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("There was an error while parsing one of the pick items in the database.\n" + e.Message);
                            }
                        }
                        reader.Close();
                    }
                }
            });

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public static PickListItem RequestPickItem(int orderId, Material materialId)
        {
            PickListItem item = null;
            DbAction(null, (conn) =>
            {
                var sql = $"SELECT * FROM pickItems WHERE orderId = '{orderId}' AND matId = '{materialId.MatId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            try
                            {
                                item = ReadPickItem(reader);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("There was an error while parsing one of the pick items in the database.\n" + e.Message);
                            }
                        }
                        reader.Close();
                    }
                }
            });

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static List<PickListItem> RequestPickList(int orderId)
        {
            var items = new List<PickListItem>(10);

            DbAction(null, (conn) =>
            {
                var sql = $"SELECT * FROM pickItems WHERE orderId = '{orderId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            try
                            {
                                var item = ReadPickItem(reader);
                                if (item != null)
                                    items.Add(item);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("There was an error while parsing one of the pick items in the database.\n" + e.Message);
                            }
                        }
                        reader.Close();
                    }
                }
            });

            return (items.Count < 1) ? null : items;
        }

        #endregion
    }
}
