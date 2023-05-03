using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAOT
{
    public static class ExtMethods
    {

        public static bool IsNumeric(this string str)
        {
            return decimal.TryParse(str, out decimal result);
        }
    }
}
