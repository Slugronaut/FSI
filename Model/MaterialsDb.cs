using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace SAOT
{
    public class MaterialsDb
    {
        public const string DbFile = "materials.db";
        public static bool DatabaseExists => File.Exists(Config.DirectoryOfDb(DbFile));
        public static SQLiteConnection NewDbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;New=True;Compress=True;");
        public static SQLiteConnection DbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Compress=True;");
        public static SQLiteConnection DbReadonlyConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Read Only=True;Compress=True;");


        public static void ConfirmDatabaseExists()
        {
            if (!DatabaseExists)
            {
                using (var conn = NewDbConnection)
                {
                    conn.Open();

                    string sql = @"create table materials (sapid text primary key,
                                                        docid text,
                                                        customerid text,
                                                        desc text,
                                                        uom text,
                                                        groupid text
                                                        );";
                    var cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    conn.Close();
                }
            }
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
        /// <param name="id"></param>
        /// <returns></returns>
        public static Material RequestMaterial(string id)
        {
            ConfirmDatabaseExists();
            Material mat = null;
            id = DbAccess.Sanitize(id);

            DbAction(null, conn =>
            {
                var sql = $"SELECT * FROM materials WHERE sapid = '{id}'";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    string sapid = DbAccess.Unsanitize(reader["sapid"] as string);
                    string docid = DbAccess.Unsanitize(reader["docid"] as string);
                    string customerid = DbAccess.Unsanitize(reader["customerid"] as string);
                    string desc = DbAccess.Unsanitize(reader["desc"] as string);
                    string uom = DbAccess.Unsanitize(reader["uom"] as string);
                    string groupid = DbAccess.Unsanitize(reader["groupid"] as string);

                    mat = new Material(sapid, docid, desc, uom, groupid, customerid, null);
                    reader.Close();
                }

                cmd.Dispose();
            });

            return mat;
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<Material> RequestAllMaterials()
        {
            ConfirmDatabaseExists();
            List<Material> materials = new List<Material>(4);

            DbAction(null, conn =>
            {
                var sql = "SELECT * FROM \"materials\"";
                var cmd = new SQLiteCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string sapid = DbAccess.Unsanitize(reader["sapid"] as string);
                        string docid = DbAccess.Unsanitize(reader["docid"] as string);
                        string customerid = DbAccess.Unsanitize(reader["customerid"] as string);
                        string desc = DbAccess.Unsanitize(reader["desc"] as string);
                        string uom = DbAccess.Unsanitize(reader["uom"] as string);
                        string groupid = DbAccess.Unsanitize(reader["groupid"] as string);

                        materials.Add(new Material(sapid, docid, desc, uom, groupid, customerid, null));
                    }
                    reader.Close();
                }
                cmd.Dispose();
            });

            return materials;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AddMaterial(User user, Material mat)
        {
            ConfirmDatabaseExists();
            var santiziedMat = new Material(
                DbAccess.Sanitize(mat.MatId),
                DbAccess.Sanitize(mat.DocumentId),
                DbAccess.Sanitize(mat.Description),
                DbAccess.Sanitize(mat.UoM),
                DbAccess.Sanitize(mat.MatGroup),
                DbAccess.Sanitize(mat.CustomerId),
                null
                );

            var sql = $"INSERT INTO materials (sapid, docid, customerid, desc, uom, groupid) VALUES ('{mat.MatId}', '{mat.DocumentId}', '{mat.CustomerId}', '{mat.Description}', '{mat.UoM}', '{mat.MatGroup}');";
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
