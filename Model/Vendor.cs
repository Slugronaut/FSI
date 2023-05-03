using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOT
{
    /// <summary>
    /// 
    /// </summary>
    public class Vendor
    {
        public const string DbFile = "vendors.db";
        public static bool DatabaseExists => File.Exists(Config.DirectoryOfDb(DbFile)) ? true : false;
        public static SQLiteConnection NewDbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;New=True;Compress=True;");
        public static SQLiteConnection DbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Compress=True;");
        public static SQLiteConnection DbReadonlyConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Read Only=True;Compress=True;");


        public int Id { get; private set; }
        public int TransferId { get; private set; }
        public string Name { get; private set; }
        public bool InUSA { get; set; }
        public string StreetAddress { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Zip { get; private set; }

        public string FullAddress
        {
            get => $"{Name}\r\n{StreetAddress}\r\n{City} {State}, {Zip}";
        }


        private Vendor(string name, int id, int transferId, string street, string city, string state, string zip)
        {
            Name = name;
            Id = id;
            TransferId = transferId;
            StreetAddress = street;
            City = city;
            State = state;
            Zip = zip;

            InUSA = true;
        }

        public static void ConfirmDatabaseExists()
        {
            if(!DatabaseExists)
            {
                using (var conn = NewDbConnection)
                {
                    conn.Open();

                    string sql = @"create table vendors (name text primary key,
                                                        id integer,
                                                        transferId integer,
                                                        street text,
                                                        city text,
                                                        state text,
                                                        zip text,
                                                        inUSA integer
                                                        );";
                    var cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetVendorNameFromId(int id)
        {
            Vendor creator = null;
            try { creator = Vendor.RequestVendor(id); }
            catch (Exception e)
            {
                creator = null;
            }
            return creator?.Name;
        }

        /// <summary>
        /// Verfiies access to the model's database, opens a connection and performs the givien action
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Vendor> RequestAllVendors()
        {
            ConfirmDatabaseExists();
            List<Vendor> vendors = new List<Vendor>(4);

            DbAction(null, conn =>
            {
                var sql = "SELECT * FROM \"vendors\"";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        int transferId = Convert.ToInt32(reader["transferId"]);
                        string name = DbAccess.Unsanitize(reader["name"] as string);
                        string street = DbAccess.Unsanitize(reader["street"] as string);
                        string city = DbAccess.Unsanitize(reader["city"] as string);
                        string state = DbAccess.Unsanitize(reader["state"] as string);
                        string zip = DbAccess.Unsanitize(reader["zip"] as string);
                        bool inUSA = Convert.ToBoolean(reader["inUSA"]);

                        vendors.Add(new Vendor(name, id, transferId, street, city, state, zip));
                    }
                    reader.Close();
                }
                cmd.Dispose();
            });

            return vendors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> RequestAllVendorNames()
        {
            ConfirmDatabaseExists();
            List<string> vendors = new List<string>(4);

            DbAction(null, conn =>
            {
                var sql = "SELECT name FROM vendors ORDER BY DESC";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = DbAccess.Unsanitize(reader["name"] as string);
                        vendors.Add(name);
                    }
                    reader.Close();
                }
                cmd.Dispose();
            });

            return vendors;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Vendor RequestVendor(string vendorName)
        {
            ConfirmDatabaseExists();
            Vendor vendor = null;
            vendorName = DbAccess.Sanitize(vendorName);

            DbAction(null, conn =>
            {
                var sql = $"SELECT * FROM vendors WHERE name = '{vendorName}';";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    string name = DbAccess.Unsanitize(reader["name"] as string);
                    int id = Convert.ToInt32(reader["id"]);
                    int transferId = Convert.ToInt32(reader["transferId"]);
                    string street = DbAccess.Unsanitize(reader["street"] as string);
                    string city = DbAccess.Unsanitize(reader["city"] as string);
                    string state = DbAccess.Unsanitize(reader["state"] as string);
                    string zip = DbAccess.Unsanitize(reader["zip"] as string);
                    bool inUSA = Convert.ToBoolean(reader["inUSA"]);

                    vendor = new Vendor(name, id, transferId, street, city, state, zip);
                    reader.Close();
                }
                    
                cmd.Dispose();
            });

            return vendor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Vendor RequestVendor(int vendorId)
        {
            ConfirmDatabaseExists();
            Vendor vendor = null;

            DbAction(null, conn =>
            {
                var sql = $"SELECT * FROM vendors WHERE id = '{vendorId}';";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    string name = DbAccess.Unsanitize(reader["name"] as string);
                    int id = Convert.ToInt32(reader["id"]);
                    int transferId = Convert.ToInt32(reader["transferId"]);
                    string street = DbAccess.Unsanitize(reader["street"] as string);
                    string city = DbAccess.Unsanitize(reader["city"] as string);
                    string state = DbAccess.Unsanitize(reader["state"] as string);
                    string zip = DbAccess.Unsanitize(reader["zip"] as string);
                    bool inUSA = Convert.ToBoolean(reader["inUSA"]);

                    vendor = new Vendor(name, id, transferId, street, city, state, zip);
                    reader.Close();
                }

                cmd.Dispose();
            });

            return vendor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="street"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="inUSA"></param>
        public static void AddVendor(User user, string name, int id, int transferId, string street, string city, string state, string zip, bool inUSA)
        {
            name = DbAccess.Sanitize(name);
            street = DbAccess.Sanitize(street);
            city = DbAccess.Sanitize(city);
            state = DbAccess.Sanitize(state);
            zip = DbAccess.Sanitize(zip);

            var sql = string.Format("INSERT INTO vendors (name, id, transferId, street, city, state, zip, inUSA) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');", name, id, transferId, street, city, state, zip, Convert.ToInt32(inUSA));
            DbAction(user, conn =>
            {
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
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="street"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="inUSA"></param>
        public static void UpdateVendor(User user, string name, int id, int transferId, string street, string city, string state, string zip, bool inUSA)
        {
            name = DbAccess.Sanitize(name);
            street = DbAccess.Sanitize(street);
            city = DbAccess.Sanitize(city);
            state = DbAccess.Sanitize(state);
            zip = DbAccess.Sanitize(zip);

            var sql = string.Format("UPDATE vendors SET (id, transferId, street, city, state, zip, inUSA) = ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}') WHERE name = '{0}';", name, id, transferId, street, city, state, zip, Convert.ToInt32(inUSA));
            DbAction(user, conn =>
            {
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }
    }
}
