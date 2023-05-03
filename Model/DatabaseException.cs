using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOT.Model
{

    /// <summary>
    /// 
    /// </summary>
    public class DatabaseLockedException : Exception
    {
        public DatabaseLockedException(string dbName, string userId) : base($"The {dbName} database is currently locked by the user {userId}.") { }
        public DatabaseLockedException(string dbName, string userId, Exception innerException) : base($"The {dbName} database is currently locked by the user {userId}.", innerException) { }
    }
}
