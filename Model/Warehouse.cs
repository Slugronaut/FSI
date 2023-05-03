using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAOT.Model
{
    /// <summary>
    /// A collection of storage spaces and materials.
    /// </summary>
    public class Warehouse
    {
        public readonly string WarehouseId;
        public List<StorageSpace> Storage;

        public const string DbFile = "warehouse.db";
        public static bool DatabaseExists => File.Exists(Config.DirectoryOfDb(DbFile)) ? true : false;
        public static SQLiteConnection NewDbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;New=True;Compress=True;PRAGMA foreign_keys = 1");
        public static SQLiteConnection DbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Compress=True;PRAGMA foreign_keys = 1");
        public static SQLiteConnection DbReadonlyConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Read Only=True;Compress=True;PRAGMA foreign_keys = 1");
        



        public Warehouse(string id, List<StorageSpace> storage = null)
        {
            WarehouseId = id;
            Storage = storage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        public void RefreshModel(SQLiteConnection conn)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshView(FilterableDataGridView view)
        {
            //TODO: update our grid view here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matId"></param>
        /// <returns></returns>
        public StorageSpace FindFirstMaterialLocation(string matId)
        {
            if (string.IsNullOrEmpty(matId)) return null;

            foreach (var loc in Storage)
            {
                if (loc.HasMaterial(matId))
                    return loc;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matId"></param>
        /// <returns></returns>
        public List<StorageSpace> FindAllMaterialLocations(string matId)
        {
            List<StorageSpace> locs = new List<StorageSpace>(10);
            if (string.IsNullOrEmpty(matId)) return locs;

            foreach (var loc in Storage)
            {
                if (loc.HasMaterial(matId))
                    locs.Add(loc);
            }

            return locs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locId"></param>
        public StorageSpace FindStorageLocation(string locId)
        {
            if (string.IsNullOrEmpty(locId)) return null;

            foreach(var loc in Storage)
            {
                if (loc.LocationId == locId)
                    return loc;
            }

            return null;
        }

        /// <summary>
        /// Converts and returns this warehouse's StorageSpaces in the Aisle-major format.
        /// </summary>
        /// <returns></returns>
        public List<Aisle> AisleFormatStorage(int aisleCol, int rackCol, int levelCol, int binCol)
        {
            if (aisleCol < 1 || rackCol < 1 || levelCol < 1 || binCol < 1)
                throw new Exception("Column count must be greater than zero.");

            List<Aisle> aisles = new List<Aisle>();

            foreach(var storage in Storage)
            {
                #region Parse Location Info
                string aisleId = storage.LocationId.Substring(0, aisleCol);
                string rackId = storage.LocationId.Substring(aisleCol, rackCol);
                string levelId = storage.LocationId.Substring(aisleCol + rackCol, levelCol);
                string binId = storage.LocationId.Substring(aisleCol + rackCol + levelCol, binCol);
                #endregion


                #region Find or Create containers as needed
                var a = aisles.Where(x => x.Id == aisleId).FirstOrDefault(); //aisles.FirstOrDefault(x => x != null && x.Id == aisleId);
                if(a == null)
                {
                    a = new Aisle(aisleId);
                    aisles.Add(a);
                }

                var r = a.Racks.FirstOrDefault(x => x != null && x.Id == rackId);
                if(r == null)
                {
                    r = new Rack(rackId);
                    a.Racks.Add(r);
                }

                var l = r.Levels.FirstOrDefault(x => x != null && x.Id == levelId);
                if(l == null)
                {
                    l = new Level(levelId);
                    r.Levels.Add(l);
                }

                var b = l.Bins.FirstOrDefault(x => x != null && x.Id == binId);
                if(b == null)
                {
                    b = new Bin(binId);
                    l.Bins.Add(b);
                }
                #endregion


            }
            return aisles;
        }


        #region Static Methods

        public static void CreateUpdateTracker(User user)
        {
            DbAction(user, conn =>
            {
                var sql = $"CREATE TABLE dbStatus (";
                //var sql = $"UPDATE locations SET contents = '{locData.SerializeContents()}' WHERE locationId = '{locationId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

            });
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ConfirmDatabaseExists()
        {
            if (!DatabaseExists)
            {
                using (var conn = NewDbConnection)
                {
                    conn.Open();
                    string sql = @"CREATE TABLE warehouses (warehouseId TEXT PRIMARY KEY);";
                    var cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();


                    sql = @"CREATE TABLE locations (locationId TEXT PRIMARY KEY,
                                                        warehouseId TEXT not null,
                                                        multistore INTEGER,
                                                        storagetype TEXT,
                                                        contents TEXT,
                                                        CONSTRAINT fk_warehouseId
                                                            FOREIGN KEY (warehouseId)
                                                            REFERENCES warehouses (warehouseId) 
                                                            ON UPDATE CASCADE
                                                            ON DELETE RESTRICT
                                                        );";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();


                    cmd.Dispose();
                    conn.Close();
                }

                Warehouse.CreateDefaultWarehouse("WH");

                //confirm that we have a warehouse called 'WH'
                var names = Warehouse.RequestWarehouseNames();
                if (names.Count != 1) throw new Exception("Something went wrong while creating a default warehouse database.\n\nIncorrect number of warehouses in the warehouse database.");
                else if (names[0] != "WH") throw new Exception("Something went wrong while creating a default warehouse database.\n\nThe default warehouse is named \"" + names[0] + "\" instead of \"WH\".");
            }
        }

        /// <summary>
        /// Verfies access to the model's database, opens a connection and performs the given action
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
                if(DbAccess.DatabaseLocking)
                {
                    //TODO: Validate access to DB file here using the file-lock subsystem
                    //display error message if write access not allow.
                    //Error.ShowMessage("We have not yet implemented DB locking. This messagebox is being displayed at the location where this should happen in the code.");

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        private static void UnlockedDbAction(Action<SQLiteConnection> action)
        {
            using (var conn = DbConnection)
            {
                conn.Open();
                action(conn);
                conn.Close();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Utility for importing old FSI storage data.
        /// </summary>
        /// <param name="filename"></param>
        public static void ImportStorageDataIntoDb(User user, string filename, string warehouseId)
        {
            //TODO: read in storage info and insert it into warehouse
            if (!WarehouseExists(warehouseId))
                CreateWarehouse(user, warehouseId);

            
        }

        /// <summary>
        /// Helper method for parsing a spreadsheet containing info about our inventory.
        /// </summary>
        /// <param name="filename"></param>
        public static List<StorageSpace> ParseStorageSpreadsheet(string fileName, bool applyMaterialQty = false, bool validateMaterialExists = false)
        {
            Dictionary<string, StorageSpace> locMap = new Dictionary<string, StorageSpace>(100);


            string filePath = Path.Combine(Config.AppPath, fileName);
            //first, import materials sheet
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    //we are only going to read the first sheet
                    while (reader.Name != "Sheet1")
                        reader.NextResult();

                    if (reader.Name == "Sheet1")
                    {
                        Console.WriteLine("Valid materials sheet found. Importing location data now...");
                        //var result = reader.AsDataSet();
                        Dictionary<string, int> columnMap = new Dictionary<string, int>();
                        int rowCount = 0;
                        while (reader.Read())
                        {
                            if (rowCount == 0)
                            {
                                //this is just the header for column titles
                                for (int i = 0; i < reader.FieldCount; i++)
                                    columnMap[reader.GetString(i)] = i;
                            }
                            else
                            {
                                int locIdCol = columnMap["Location"];
                                int matCol = columnMap["Material"];
                                int qtyCol = columnMap["Qty"];

                                string locId;
                                string mat;
                                double qty;

                                #region Get Location ID
                                try
                                {
                                    locId = reader.GetString(locIdCol);
                                }
                                catch(Exception e)
                                {
                                    throw new Exception("Failed to parse the Location column.", e);
                                }
                                #endregion


                                #region Get Material ID
                                try
                                {
                                    mat = reader.GetString(matCol);
                                }
                                catch(Exception e1)
                                {
                                    try
                                    {
                                        double matD = reader.GetDouble(matCol);
                                        mat = string.Empty + matD;
                                    }
                                    catch (Exception e2)
                                    {
                                        throw new Exception("Failed to parse the Material column.", e2);
                                    }
                                }
                                #endregion


                                #region Get Quantity
                                try
                                {
                                    var q = reader.GetInt32(qtyCol);
                                    qty = q;
                                }
                                catch(Exception e1)
                                {
                                    try
                                    {
                                        var q = reader.GetDouble(qtyCol);
                                        qty = q;
                                    }
                                    catch(Exception e2)
                                    {
                                        try
                                        {
                                            var q = reader.GetString(qtyCol);
                                            qty = double.Parse(q);
                                        }
                                        catch(Exception e3)
                                        {
                                            throw new Exception("Failed to parse the Qty column.", e3);
                                        }
                                    }
                                }
                                #endregion

                                if (!locMap.TryGetValue(locId, out StorageSpace store))
                                {
                                    store = new StorageSpace(locId);
                                    locMap[locId] = store;
                                }

                                store.AdjustQty(mat, qty);
                            }
                            rowCount++;
                        }
                        Console.WriteLine("... location sheet import complete.");
                    }
                    else
                    {
                        throw new Exception("No valid storage locations spreadsheet could be found. Please be sure that all information is contained on a sheet entitled 'Sheet1' and that the following columns exist:\n\tComponent number\n\tDocument / Drawing\n\tObject description\n\tUn\n\tMatl Group");
                    }
                }
            }

            return locMap.Values.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        public static void CreateWarehouse(User user, string warehouseId)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);
            DbAction(user, conn =>
            {
                var sql = $"INSERT INTO warehouses (warehouseId) VALUES ('{warehouseId}');";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// Creates a default warehouse. This is meant to only be used during database creation and thus does not require a user.
        /// </summary>
        /// <param name="warehouseId"></param>
        private static void CreateDefaultWarehouse(string warehouseId)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);
            UnlockedDbAction(conn =>
            {
                var sql = $"INSERT INTO warehouses (warehouseId) VALUES ('{warehouseId}');";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        public static void CreateStorageLocation(User user, string warehouseId, string locationId)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);
            locationId = DbAccess.Sanitize(locationId);

            DbAction(user, conn =>
            {
                var sql = $"INSERT INTO locations (locationId, warehouseId, multistore, storagetype, contents) VALUES ('{locationId}', '{warehouseId}', '1', \"storage\", 'NULL');";
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
        /// <param name="warehouseId"></param>
        /// <param name="aisles"></param>
        public static void CreateStorageLocations(User user, string warehouseId, List<Aisle> aisles)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);

            DbAction(user, conn =>
            {
                foreach(var aisle in aisles)
                {
                    foreach(var rack in aisle.Racks)
                    {
                        foreach(var level in rack.Levels)
                        {
                            foreach(var bin in level.Bins)
                            {
                                string locationId = $"{aisle.Id}{rack.Id}{level.Id}{bin.Id}";
                                var sql = $"INSERT INTO locations (locationId, warehouseId, multistore, storagetype, contents) VALUES ('{DbAccess.Sanitize(locationId)}', '{warehouseId}', '1', \"storage\", 'NULL');";
                                using (var cmd = new SQLiteCommand(sql, conn))
                                {
                                    try
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch(Exception e)
                                    {
                                        //if this is a unique-key exception, just continue. it simply means
                                        //we already have this location in the db. we can safely ignore it.
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        public static void CreateStorageLocations(User user, string warehouseId, List<string> locationIds)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);

            DbAction(user, conn =>
            {
                foreach (string locationId in locationIds)
                {
                    var sql = $"INSERT INTO locations (locationId, warehouseId, multistore, storagetype, contents) VALUES ('{DbAccess.Sanitize(locationId)}', '{warehouseId}', '1', \"storage\", 'NULL');";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            //if this is a unique-key exception, just continue. it simply means
                            //we already have this location in the db. we can safely ignore it.
                            continue;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="warehouseId"></param>
        /// <param name="store"></param>
        public static void SetLocationContents(User user, string warehouseId, StorageSpace store)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);
            DbAction(user, conn =>
            {
                string items;
                string sql;

                if (store.Items.Count < 1)
                {
                    sql = $"UPDATE locations SET contents = 'NULL' WHERE locationId = '{store.LocationId}';";
                }
                else
                {
                    //var sql = string.Format("INSERT INTO locations (locationId, warehouseId, multistore, storagetype, contents) VALUES ('{1}', '{0}', '1', \"storage\", 'NULL')", warehouseId, DbAccess.Sanitize(store.LocationId));
                    items = store.SerializeContents();
                    sql = $"UPDATE locations SET contents = '{items}' WHERE locationId = '{store.LocationId}';";
                }
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
        /// <param name="warehouseId"></param>
        /// <param name="store"></param>
        /// <param name="type"></param>
        public static void SetLocationType(User user, string warehouseId, StorageSpace store, StorageSpace.LocationTypes type)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);
            DbAction(user, conn =>
            {
                var sql = $"UPDATE locations SET storagetype = '{Enum.GetName(typeof(StorageSpace.LocationTypes), type)}' WHERE locationId = '{store.LocationId}';";
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
        /// <param name="warehouseId"></param>
        /// <param name="materialId"></param>
        /// <param name="qty"></param>
        public static void SetMaterialInLocation(User user, string warehouseId, string locationId, string materialId, double qty)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);
            locationId = DbAccess.Sanitize(locationId);
            var locData = RequestStorageSpaceData(warehouseId, locationId);
            locData.SetQty(materialId, qty);

            DbAction(user, conn =>
            {
                var sql = $"UPDATE locations SET contents = '{locData.SerializeContents()}' WHERE locationId = '{locationId}';";
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
        /// <param name="warehouseId"></param>
        /// <param name="materialId"></param>
        /// <param name="adjustQty"></param>
        public static void AdjustMaterialInLocation(User user, string warehouseId, string locationId, string materialId, int adjustQty)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);
            locationId = DbAccess.Sanitize(locationId);
            var locData = RequestStorageSpaceData(warehouseId, locationId);
            locData.AdjustQty(materialId, adjustQty);

            DbAction(user, conn =>
            {
                var sql = $"UPDATE locations SET contents = '{locData.SerializeContents()}' WHERE locationId = '{locationId}';";
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
        /// <param name="warehouseId"></param>
        /// <param name="materialId"></param>
        public static void RemoveMaterialFromLocation(User user, string warehouseId, string locationId, string materialId)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);
            locationId = DbAccess.Sanitize(locationId);
            var locData = RequestStorageSpaceData(warehouseId, locationId);
            locData.RemoveItem(materialId);

            DbAction(user, conn =>
            {
                //IMPORTANT NOTE: We aren't deleting the item, we're removing
                //the material and qty text from it's content's string.
                //throw new Exception("NOT YET IMPLEMENTED");
                //var sql = $"DELETE FROM locations WHERE warehouseId = '{warehouseId}' AND contents LIKE %%;";
                var sql = $"UPDATE locations SET contents = '{locData.SerializeContents()}' WHERE locationId = '{locationId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

            });

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        public static void CreateStorageLocations(User user, string warehouseId, List<StorageSpace> stores)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);

            DbAction(user, conn =>
            {
                foreach (var store in stores)
                {
                    var sql = $"INSERT INTO locations (locationId, warehouseId, multistore, storagetype, contents) VALUES ('{DbAccess.Sanitize(store.LocationId)}', '{warehouseId}', '1', \"storage\", 'NULL');";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locatonId"></param>
        public static void DeleteStorageLocation(User user, string warehouseId, string locatonId)
        {
            throw new Exception("Storage location removal not yet implemented.");
        }

        /// <summary>
        /// Deletes all storage locations and their contents from a given warehouse.
        /// </summary>
        /// <param name="warehouseId"></param>
        public static void DeleteAllStorageLocations(User user, string warehouseId)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);

            DbAction(user, conn =>
            {
                var sql = $"DELETE FROM locations WHERE warehouseId = '{warehouseId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> RequestWarehouseNames()
        {
            var names = new List<string>();
            if (!DatabaseExists) return names;

            DbAction(null, conn =>
            {
                var sql = "SELECT * FROM warehouses ORDER BY warehouseId DESC;";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = reader["warehouseId"] as string;
                            if(!string.IsNullOrEmpty(name))
                                names.Add(name);
                        }
                        reader.Close();
                    }
                }
            });
            return names;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public static Warehouse RequestWarehouseData(string warehouseId)
        {
            warehouseId = DbAccess.Sanitize(warehouseId);
            Warehouse warehouse = new Warehouse(warehouseId);

            DbAction(null, conn =>
            {
                //get all locations with the same warehouse id
                var sql = $"SELECT * FROM locations WHERE warehouseId = '{warehouseId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    //parse through the list of locations returned
                    List<StorageSpace> storageLocs = new List<StorageSpace>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        string locId = null;
                        string contents = null;
                        StorageSpace.LocationTypes locType = StorageSpace.LocationTypes.storage;
                        try
                        {
                            while (reader.Read())
                            {
                                locId = reader["locationId"] as string;
                                contents = reader["contents"] as string;
                                locType = (StorageSpace.LocationTypes)Enum.Parse(typeof(StorageSpace.LocationTypes), reader["storagetype"] as string);
                                var locContents = ParseLocationContentsString(locId, contents, locType);
                                if (locContents != null)
                                    storageLocs.Add(locContents);
                            }
                        }
                        catch(Exception e)
                        {
                            throw new Exception($"There was an error while parsing the location data for location {locId} with the contents of {contents}", e);
                        }
                        reader.Close();
                    }

                    warehouse.Storage = storageLocs;
                }
            });
            
            return warehouse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static StorageSpace RequestStorageSpaceData(string warehouseId, string locationId)
        {
            StorageSpace store = null;
            warehouseId = DbAccess.Sanitize(warehouseId);
            locationId = DbAccess.Sanitize(locationId);

            DbAction(null, conn =>
            {
                //get all locations with the same warehouse id
                var sql = $"SELECT * FROM locations WHERE warehouseId = '{warehouseId}' AND locationId = '{locationId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    //parse through the list of locations returned
                    List<StorageSpace> storageLocs = new List<StorageSpace>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) store = null;
                        else store = ParseLocationContentsString(reader["locationId"] as string, reader["contents"] as string, (StorageSpace.LocationTypes)Enum.Parse(typeof(StorageSpace.LocationTypes), reader["storagetype"] as string));
                        reader.Close();
                    }
                }
            });

            return store;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public static bool WarehouseExists(string warehouseId)
        {
            bool found = false;
            warehouseId = DbAccess.Sanitize(warehouseId);
            DbAction(null, conn =>
            {
                //get all locations with the same warehouse id
                var sql = $"SELECT * FROM locations WHERE warehouseId = '{warehouseId}';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    //parse through the list of lcations returned
                    List<StorageSpace> storageLocs = new List<StorageSpace>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        found = reader.Read();
                        reader.Close();
                    }
                }
            });

            return found;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static StorageSpace ParseLocationContentsString(string locId, string contents, StorageSpace.LocationTypes type)
        {
            if (string.IsNullOrEmpty(locId))
                return null;
            if (string.IsNullOrEmpty(contents))
                return new StorageSpace(locId);

            //TODO: Could use a lot of format validation here!!
            Dictionary<string, double> items = null;
            if (!string.IsNullOrEmpty(contents) && contents != "NULL")
            {
                items = new Dictionary<string, double>();
                var itemSplit = contents.Split(';');
                foreach (var item in itemSplit)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    var parts = item.Split(',');
                    var id = parts[0];
                    var qty = int.Parse(parts[1]);
                    items[id] = qty;
                }
            }

            return new StorageSpace(locId, items, type);
        }

        #endregion
    }
}
