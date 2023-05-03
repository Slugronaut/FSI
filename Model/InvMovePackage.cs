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
    public static class InvMovePackage
    {
        public static List<string> DragList = new List<string>(10);
        public static string DragSource;

        public static void BeginMove(string src, List<string> items)
        {
            EndMove();
            DragSource = src;
            DragList.AddRange(items);
        }

        public static void BeginMove(string src, params string[] items)
        {
            EndMove();
            DragSource = src;
            DragList.AddRange(items);
        }

        public static void EndMove()
        {
            DragSource = null;
            DragList.Clear();
        }
    }
}
