using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace SAOT.Model
{

    /// <summary>
    /// 
    /// </summary>
    public class Carrier
    {
        public const string DbFile = "carriers.db";
        public static bool DatabaseExists => File.Exists(Config.DirectoryOfDb(DbFile)) ? true : false;
        public static SQLiteConnection NewDbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;New=True;Compress=True;");
        public static SQLiteConnection DbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Compress=True;");
        public static SQLiteConnection DbReadonlyConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Read Only=True;Compress=True;");


        public int Id { get; private set; }
        public string Name { get; private set; }


        private Carrier(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public static void ConfirmDatabaseExists()
        {
            if (!DatabaseExists)
            {
                using (var conn = NewDbConnection)
                {
                    conn.Open();

                    string sql = @"CREATE TABLE carriers (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT);";
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
        public static string GetCarrierNameFromId(int id)
        {
            Carrier carrier = null;
            try { carrier = Carrier.RequestCarrier(id); }
            catch (Exception e)
            {
                carrier = null;
            }
            return carrier?.Name;
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
        public static List<Carrier> RequestAllCarriers()
        {
            ConfirmDatabaseExists();
            List<Carrier> carriers = new List<Carrier>(4);

            DbAction(null, conn =>
            {
                var sql = "SELECT * FROM carriers";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string name = DbAccess.Unsanitize(reader["name"] as string);

                        carriers.Add(new Carrier(name, id));
                    }
                    reader.Close();
                }
                cmd.Dispose();
            });

            return carriers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> RequestAllCarrierNames()
        {
            ConfirmDatabaseExists();
            List<string> carriers = new List<string>(4);

            DbAction(null, conn =>
            {
                var sql = "SELECT name FROM carriers ORDER BY DESC";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = DbAccess.Unsanitize(reader["name"] as string);
                        carriers.Add(name);
                    }
                    reader.Close();
                }
                cmd.Dispose();
            });

            return carriers;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Carrier RequestCarrier(string carrierName)
        {
            ConfirmDatabaseExists();
            Carrier carrier = null;
            carrierName = DbAccess.Sanitize(carrierName);

            DbAction(null, conn =>
            {
                var sql = $"SELECT * FROM carriers WHERE name = '{carrierName}';";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    string name = DbAccess.Unsanitize(reader["name"] as string);
                    int id = Convert.ToInt32(reader["id"]);
                    carrier = new Carrier(name, id);
                    reader.Close();
                }

                cmd.Dispose();
            });

            return carrier;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Carrier RequestCarrier(int carrierId)
        {
            ConfirmDatabaseExists();
            Carrier carrier = null;

            DbAction(null, conn =>
            {
                var sql = $"SELECT * FROM carriers WHERE id = '{carrierId}';";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    string name = DbAccess.Unsanitize(reader["name"] as string);
                    int id = Convert.ToInt32(reader["id"]);

                    carrier = new Carrier(name, id);
                    reader.Close();
                }

                cmd.Dispose();
            });

            return carrier;
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
        public static void AddCarrier(User user, string name)
        {
            name = DbAccess.Sanitize(name);

            var sql = $"INSERT INTO carriers (name) VALUES ('{name}');";
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
