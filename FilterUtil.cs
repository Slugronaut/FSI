using System.Text.RegularExpressions;

namespace SAOT
{
    /// <summary>
    /// Creates a text filter based on a supplied string that may contain wildcards.
    /// </summary>
    /// <param name="impledAtEnd">If <c>true</c> a * wildcard is attached to the input string, otherwise the string is used verbatim.</param>
    public static class FilterUtil
    {
        public static string WildCardToRegular(string value, bool impliedAtEnd = true)
        {
            //strict match
            if(value.StartsWith("<") && value.EndsWith(">"))
                return "^" + Regex.Escape(value.TrimStart('<').TrimEnd('>')).Replace("\\?", ".").Replace("\\*", ".*") + "$";
            //implied wildcard on ends
            else return "^" + Regex.Escape(impliedAtEnd ? "*" + value + "*" : value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }
    }
}
