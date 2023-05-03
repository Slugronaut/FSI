using System;
using System.Linq;

namespace SAOT
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbAccess
    {

        public static bool DatabaseLocking = true;


        /// <summary>
        /// Some low-budget SQL string cleansing goodness. Nice and slow and full of bad ideas.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Sanitize(string input)
        {
            if (input == null) return string.Empty;
            input = input.Replace("@", "__AT__");
            input = input.Replace(".", "__DOT__");
            return String.Concat(input.Select(x => (x == '_' || Char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)) ? x : throw new Exception("The character \""+x+"\" is not allowed as input.")));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Unsanitize(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            input = input.Replace("__AT__", "@");
            return input.Replace("__DOT__", ".");
        }
    }

}
