using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAOT
{
    public static class Assert
    {
        public static void IsTrue(bool condition)
        {
            if (!condition) throw new Exception("An assertion failed. This action must terminate.");
        }

        public static void IsFalse(bool condition)
        {
            if (condition) throw new Exception("An assertion failed. This action must terminate.");
        }

        public static bool IsNotNull(object val)
        {
            return (val == null || val.Equals(null)) ? false : true;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class AssertionException : Exception
    {
        public AssertionException() : base() { }
        public AssertionException(string message, string expected) : base(message+ "\n"+expected) { }
        public AssertionException(string message, string expected, Exception innerException) : base(message + "\n" + expected, innerException) { }
    }


    /// <summary>
    /// 
    /// </summary>
    public static class Error
    {
        public static void ShowMessage(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
        {
            MessageBox.Show(message + " at line " + lineNumber + " (" + caller + ")");
        }
    }
}
