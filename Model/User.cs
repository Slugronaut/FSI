using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Toolbox;

namespace SAOT
{
    /// <summary>
    /// 
    /// </summary>
    public class UserException : Exception
    {
        public UserException() : base() { }
        public UserException(string message) : base(message) { }
        public UserException(string message, Exception innerException) : base(message, innerException) { }
    }


    /// <summary>
    /// Global message posted when a valid user logs in.
    /// </summary>
    public class UserLoggedInEvent : IMessage
    {
        public User User{ get; private set; }

        public UserLoggedInEvent(User user)
        {
            User = user;
        }
    }


    /// <summary>
    /// Global message posted when a valid user logs out.
    /// </summary>
    public class UserLoggedOutEvent : IMessage
    {
        public User User { get; private set; }

        public UserLoggedOutEvent(User user)
        {
            User = user;
        }
    }


    /// <summary>
    /// Represents the user currently logged in.
    /// 
    /// WARNING: This class is an inherently flawed concept. Storing the user as an object that represents all of
    /// credentials and access privileges on the client is insane. But without a server to authenticate actions it's
    /// the only way to do it. Luckily, this is a small-scale applciations with extremely limted use and little-to-no
    /// reprocussions should anything be compromised.
    /// 
    /// </summary>
    public class User
    {
        [Flags]
        public enum AccessRights
        {
            Admin           = 1 << 0,
            CreateOrders    = 1 << 1,
            PickOrders      = 1 << 2,
            ManageMaterials = 1 << 3,
            ManageLocations = 1 << 4,
            ManageUsers     = 1 << 5,
            ManageInventory = 1 << 6,

            None = 0,
            All = Admin | CreateOrders | PickOrders | ManageMaterials | ManageLocations | ManageUsers | ManageInventory,
        }

        public static User CurrentUser { get; private set; }
        public const string DbFile = "users.db";
        public static readonly double Timeout = 3600; // time in seconds till logout when no activity
        public static SQLiteConnection NewDbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;New=True;Compress=True;");
        public static SQLiteConnection DbConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Compress=True;");
        public static SQLiteConnection DbReadonlyConnection => new SQLiteConnection("Data Source=" + Config.DirectoryOfDb(DbFile) + ";Version=3;FailIfMissing=True;Read Only=True;Compress=True;");

        DateTime LastActivityTime;
        DateTime LoginTime;

        public static bool IsUserLoggedIn(WeakReference user)
        {
            if (user == null || !user.IsAlive || user.Target as User == null)
                return false;
            return true;
        }
        public string Name { get; private set; }
        public int Id { get; private set; }
        public string Email { get; private set; }
        public uint RightsFlags { get; private set; } //yes, I'm aware of how insane it is to store and read this data on the client lol
        public bool HasTimedOut => (DateTime.Now - LastActivityTime).TotalSeconds > Timeout;

        /// <summary>
        /// Returns <c>true</c> if a user database file exists. <c>false</c> otherwise.
        /// </summary>
        public static bool DatabaseExists => File.Exists(Config.DirectoryOfDb(DbFile)) ? true : false;


        #region Access Flags

        public bool IsAdmin
        {
            get => (RightsFlags & (uint)AccessRights.Admin) != 0;
            set
            {
                if (value) RightsFlags |= (uint)AccessRights.Admin;
                else RightsFlags &= ((uint)AccessRights.Admin ^ 0xffffffff);
            }
        }

        public bool CanCreateOrder
        {
            get => (RightsFlags & (uint)AccessRights.CreateOrders) != 0;
            set
            {
                if (value) RightsFlags |= (uint)AccessRights.CreateOrders;
                else RightsFlags &= ((uint)AccessRights.CreateOrders ^ 0xffffffff);
            }
        }

        public bool CanPickOrders
        {
            get => (RightsFlags & (uint)AccessRights.PickOrders) != 0;
            set
            {
                if (value) RightsFlags |= (uint)AccessRights.PickOrders;
                else RightsFlags &= ((uint)AccessRights.PickOrders ^ 0xffffffff);
            }
        }

        public bool CanManageMaterials
        {
            get => (RightsFlags & (uint)AccessRights.ManageMaterials) != 0;
            set
            {
                if (value) RightsFlags |= (uint)AccessRights.ManageMaterials;
                else RightsFlags &= ((uint)AccessRights.ManageMaterials ^ 0xffffffff);
            }
        }

        public bool CanManageLocations
        {
            get => (RightsFlags & (uint)AccessRights.ManageLocations) != 0;
            set
            {
                if (value) RightsFlags |= (uint)AccessRights.ManageLocations;
                else RightsFlags &= ((uint)AccessRights.ManageLocations ^ 0xffffffff);
            }
        }

        public bool CanManageUsers
        {
            get => (RightsFlags & (uint)AccessRights.ManageUsers) != 0;
            set
            {
                if (value) RightsFlags |= (uint)AccessRights.ManageUsers;
                else RightsFlags &= ((uint)AccessRights.ManageUsers ^ 0xffffffff);
            }
        }

        public bool CanManageInventory
        {
            get => (RightsFlags & (uint)AccessRights.ManageInventory) != 0;
            set
            {
                if (value) RightsFlags |= (uint)AccessRights.ManageInventory;
                else RightsFlags &= ((uint)AccessRights.ManageInventory ^ 0xffffffff);
            }
        }

        #endregion


        /// <summary>
        /// Mmmm this is probably a security risk in a real app but really,
        /// who cares here. Like, three people will ever use it.
        /// </summary>
        /// <param name="name"></param>
        public User(int id, string name, string email, uint accessFlags)
        {
            Id = id;
            Name = name;
            Email = email;
            RightsFlags = accessFlags;
            LoginTime = DateTime.Now;
            RefreshTimeout();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshTimeout()
        {
            LastActivityTime = DateTime.Now;
        }


        #region Static Methods
        

        /// <summary>
        /// Posts <see cref="UserLoggedInEvent" /> if the user is succesfully logged in using the given name/password combo.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="passHash"></param>
        /// <throws>UserException if there was an issue with loging in with the given user name/password.</throws>
        public static bool Login(string id, string password)
        {
            if (string.IsNullOrEmpty(id)) throw new UserException("Did you really just try to log in with no username?");
            id = DbAccess.Sanitize(id);
            password = DbAccess.Sanitize(password);

            DbAction(null, (conn) =>
            {
                var sqlText = string.Format("select * from \"users\" where \"username\" = '{0}'", id);
                Console.WriteLine("OUTPUT: " + sqlText);

                var cmd = new SQLiteCommand(sqlText, conn);
                SQLiteDataReader reader;
                try
                {
                    reader = cmd.ExecuteReader();
                    if (!reader.Read())
                        throw new UserException("Invalid username or password.\n\n(Login error 01)");
                    
                }
                catch(UserException e)
                {
                    throw e; //this is just here to catch and re-throw the above error
                }
                catch (Exception e)
                { 
                    throw new UserException("Invalid username or password.\n\n(Login error 00)\n");
                }
                finally
                {
                    cmd.Dispose();
                }

                int accId = Convert.ToInt32(reader["id"]);
                string accName = reader["username"] as string;
                string accPass = reader["passhash"] as string;
                string email = reader["email"] as string;
                uint rightsFlags = Convert.ToUInt32((Int64)reader["rights"]);

                if (!Crypto.MatchesHash(password, accPass))
                    throw new Exception("Invalid username or password. (Login error 02)");

                reader.Close();
                reader = null;
                //let everyone know we've logged in
                MsgDispatch.PostMessage(new UserLoggedInEvent(new User(accId, accName, email, rightsFlags)));
            });

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetUserNameFromId(int id)
        {
            User creator = null;
            try { creator = RequestUserData(id); }
            catch (Exception e)
            {
                creator = null;
            }
            return creator?.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static User RequestUserData(int id)
        {
            User result = null;
            User.DbAction(null, (conn) =>
            {
                //todo: query user settings and display them in controls
                var sql = string.Format($"SELECT * FROM users WHERE id = '{id}';");
                var cmd = new SQLiteCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                cmd.Dispose();

                if (!reader.Read())
                    throw new UserException($"There are no records for user id '{id}'.");

                int accId = Convert.ToInt32(reader["id"]);
                string accName = reader["username"] as string;
                string email = reader["email"] as string;
                uint rightsFlags = Convert.ToUInt32((Int64)reader["rights"]);
                reader.Close();
                result = new User(accId, accName, email, rightsFlags);
            });

            if (result == null)
                throw new UserException($"An error occured while attempting to access records for user id '{id}'.");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static User RequestUserData(string username)
        {
            username = DbAccess.Sanitize(username);
            User result = null;
            User.DbAction(null, (conn) =>
            {
                //todo: query user settings and display them in controls
                var sql = string.Format("SELECT * FROM \"users\" WHERE \"username\" = '{0}';", username);
                var cmd = new SQLiteCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                cmd.Dispose();

                if (!reader.Read())
                    throw new UserException("There are no records for user '" + username + "'.");

                int accId = Convert.ToInt32(reader["id"]);
                string accName = reader["username"] as string;
                string email = reader["email"] as string;
                uint rightsFlags = Convert.ToUInt32((Int64)reader["rights"]);
                reader.Close();
                result = new User(accId, accName, email, rightsFlags);
            });

            if(result == null)
                throw new UserException("An error occured while attempting to access records for '" + username + "'.");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<User> RequestAllUsers()
        {
            List<User> users = new List<User>();

            DbAction(null, conn =>
            {
                var sql = "SELECT * FROM \"users\" ORDER BY \"username\" DESC";
                var cmd = new SQLiteCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                cmd.Dispose();

                while(reader.Read())
                {
                    int accId = Convert.ToInt32(reader["id"]);
                    string accName = reader["username"] as string;
                    string email = reader["email"] as string;
                    uint rightsFlags = Convert.ToUInt32((Int64)reader["rights"]);
                    users.Add(new User(accId, accName, email, rightsFlags));
                }
                reader.Close();
            });

            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="rights"></param>
        public static void ApplyUserSettings(User user, string username, string email, uint rights)
        {
            username = DbAccess.Sanitize(username);
            email = DbAccess.Sanitize(email);
            
            DbAction(user, (conn) =>
            {
                var sql = string.Format("UPDATE \"users\" SET (email, rights) = ('{1}', '{2}') WHERE \"username\" = '{0}';", username, email, rights);
                var cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="username"></param>
        public static void UpdatePassword(User user, string username, string password)
        {
            username = DbAccess.Sanitize(username);

            DbAction(user, conn =>
            {
                var sql = string.Format("UPDATE \"users\" SET (passhash) = '{1}' WHERE \"username\" = '{0}';", username, Crypto.SaltedHashOf(password));
                var cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            });
        }

        /// <summary>
        /// Retruns <c>true</c> if a specific username in the user database exists.
        /// </summary>
        public static bool IsUserAccAvailable(string username)
        {
            bool result = false;
            username = DbAccess.Sanitize(username);

            DbAction(null, (conn) =>
            {
                var sql = string.Format("SELECT count(*) FROM \"users\" WHERE \"username\" = '{0}';", username);
                var cmd = new SQLiteCommand(sql, conn);
                result = Convert.ToInt32(cmd.ExecuteScalar()) > 0 ? true : false;
                cmd.Dispose();
            });

            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static void RemoveDatabase()
        {
            File.Delete(Config.DirectoryOfDb(DbFile));
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
        /// Registers a user with the user database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="passHash"></param>
        /// <exception cref="UserException">Thrown if registration fails for some reason.</exception>
        public static void Register(User user, string id, string password, string email, uint accessRights)
        {
            id = DbAccess.Sanitize(id);
            password = DbAccess.Sanitize(password);
            email = DbAccess.Sanitize(email);

            DbAction(user, (conn) =>
            {
                //first, attempt to get the username from the db to see if it doesn't already exist
                var sql = string.Format("INSERT INTO \"users\" (username, passhash, email, rights) values ('{0}', '{1}', '{2}', {3});", id, Crypto.SaltedHashOf(password), email, accessRights);
                var cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateNewDatabase()
        {
            using (var conn = NewDbConnection)
            {
                conn.Open();

                string sql = @"create table users (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                username text,
                passhash text,
                email text,
                rights integer
                );";
                var cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sql = string.Format("insert into users (username, passhash, email, rights) values ('admin', '{0}', 'null', {1});", Crypto.SaltedHashOf("password"), (uint)AccessRights.All);
                cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
            }

        }
        #endregion
    }
}
