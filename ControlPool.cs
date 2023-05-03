using System.Collections.Generic;

namespace SAOT
{
    /// <summary>
    /// Helper class for creating WinForm controls that are pooled in memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ControlPool<T> where T : new()
    {
        LinkedList<T> Whitelist;
        LinkedList<T> Blacklist = new LinkedList<T>();

        public ControlPool(int preallocSize = 1500)
        {
            Whitelist = new LinkedList<T>();// new T[preallocSize]);
            //for(int i = 0; i < preallocSize; i++)
            //    Whitelist.AddLast(new T());

        }

        public T Retreive()
        {
            if (Whitelist.Count < 1)
                Whitelist.AddLast(new T());

            var node = Whitelist.Last;
            Whitelist.RemoveLast();
            Blacklist.AddLast(node);
            return node.Value;
        }

        public void RelenquishAll()
        {
            foreach (var node in Blacklist)
                Whitelist.AddLast(node);
            Blacklist.Clear();
        }
        
    }
}
