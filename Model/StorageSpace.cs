using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SAOT.Model
{
    /// <summary>
    /// An instance of a storage space that references both a storage 
    /// location and the materials within it.
    /// </summary>
    public class StorageSpace
    {
        public enum LocationTypes
        {
            blocked,
            storage,
            ncr_customer,
            ncr_vendor,
            quarantine,
            legacy,
            finished_goods,
        }
        public string LocationId { get; private set; }
        public LocationTypes Type = LocationTypes.storage;
        public Dictionary<string, double> Items { get; set; }

        public static Color TypeToColor(LocationTypes type)
        {
            Color color = Color.White;

            switch (type)
            {
                case StorageSpace.LocationTypes.blocked:
                    {
                        color = Color.FromArgb(75,75,75);// Color.DarkGray;
                        break;
                    }
                case StorageSpace.LocationTypes.legacy:
                    {
                        color = Color.LightBlue;
                        break;
                    }
                case StorageSpace.LocationTypes.ncr_customer:
                    {
                        color = Color.Pink;
                        break;
                    }
                case StorageSpace.LocationTypes.ncr_vendor:
                    {
                        color = Color.PaleVioletRed;
                        break;
                    }
                case StorageSpace.LocationTypes.quarantine:
                    {
                        color = Color.MediumVioletRed;
                        break;
                    }
                case StorageSpace.LocationTypes.storage:
                    {
                        color = Color.White;
                        break;
                    }
                case StorageSpace.LocationTypes.finished_goods:
                    {
                        color = Color.Aquamarine;
                        break;
                    }
            }
            return color;
        }

        public StorageSpace(string locationId, Dictionary<string, double> items = null, LocationTypes type = LocationTypes.storage)
        {
            Assert.IsFalse(string.IsNullOrEmpty(locationId));

            LocationId = locationId;
            Items = items ?? new Dictionary<string, double>();
            Type = type;
        }

        public bool HasMaterial(string matId)
        {
            return Items.ContainsKey(matId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        public void CopyContents(StorageSpace src, bool clearPrevious)
        {
            if(clearPrevious) Items.Clear();
            foreach(var kv in src.Items)
                this.SetQty(kv.Key, kv.Value);
        }

        /// <summary>
        /// Adds or subtracts a quantity of an item from this location.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="qty"></param>
        public void AdjustQty(string itemId, double qty)
        {
            if(Items.TryGetValue(itemId, out double q))
            {
                q += qty;
                if (q < 0) q = 0;
                Items[itemId] = q;
            }
            else
            {
                Items[itemId] = q > 0 ? q : 0;
            }
            
        }

        /// <summary>
        /// Completely removes all references to a material id that is in this location.
        /// </summary>
        /// <param name="itemId"></param>
        public void RemoveItem(string itemId)
        {
            Items.Remove(itemId);
        }

        /// <summary>
        /// Directly sets the quantity of an item in this location.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name=""></param>
        public void SetQty(string itemId, double qty)
        {
            Items[itemId] = qty;
        }

        /// <summary>
        /// Returns a string that represents the contents of this location as a single string.
        /// </summary>
        /// <returns></returns>
        public string SerializeContents()
        {
            if (Items.Count < 1) return null;
            StringBuilder sb = new StringBuilder(1000);
            foreach(var itemKv in Items)
                sb.Append(string.Format("{0},{1};", itemKv.Key, itemKv.Value));

            return sb.ToString();
        }


        #region Helpers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static List<string> SequenceOfLetters(string start, string end, int columns)
        {
            if (string.IsNullOrEmpty(start))
                throw new Exception("Start of the letter sequence is empty.");
            if (string.IsNullOrEmpty(start))
                throw new Exception("End of the letter sequence is empty.");
            if (!start.All(x => char.IsLetter(x)))
                throw new Exception("Start of the letter sequence does not contain all letters.");
            if (!end.All(x => char.IsLetter(x)))
                throw new Exception("End of the letter sequence does not contain all letters.");
            if (end.CompareTo(start) < 0)
                throw new Exception("End of the letter sequence occurs before the start.");
            if (start.Length > columns)
                throw new Exception("Start of the letter sequence has more characters than allowed. Maximum allowed characters is " + columns + ".");
            if (end.Length > columns)
                throw new Exception("End of the letter sequence has more characters than allowed. Maximum allowed characters is " + columns + ".");

            start = start.ToUpper();
            end = end.ToUpper();
            List<string> seq = new List<string>();
            int count = NumberFromExcelColumn(end) + 1;
            for (int x = NumberFromExcelColumn(start); x < count; x++)
            {
                var loc = IntToAlpha(x);
                seq.Add(loc.PadLeft(columns));
            }

            return seq;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static int NumberFromExcelColumn(string column)
        {
            int retVal = 0;
            string col = column.ToUpper();
            for (int iChar = col.Length - 1; iChar >= 0; iChar--)
            {
                char colPiece = col[iChar];
                int colNum = colPiece - 64;
                retVal = retVal + colNum * (int)Math.Pow(26, col.Length - (iChar + 1));
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string ExcelColumnFromNumber(int column)
        {
            string columnString = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString;
        }

        /// <summary>
        /// Essentially performs an base convertion from base 10 to base 26.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string IntToAlpha(int x)
        {
            int lowChar;
            StringBuilder result = new StringBuilder();
            do
            {
                lowChar = (x - 1) % 26;
                x = (x - 1) / 26;
                result.Insert(0, (char)(lowChar + 65));
            } while (x > 0);
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static List<string> SequenceOfNumbers(uint start, uint end, int columns)
        {
            if (end < start) throw new System.Exception("Ending sequence index must be higher or equal to the starting sequence index.");
            if (end.ToString().Length > columns || start.ToString().Length > columns)
                throw new Exception($"Start and end values for the sequence must both have a smaller number of values than {columns}.");
            
            List<string> seq = new List<string>();
            for (uint i = start; i <= end; i++)
                seq.Add(string.Format("{0:D" + columns + "}", i));

            return seq;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Aisle> DemoData()
        {
            List<Aisle> aisles = new List<Aisle>();
            foreach (var aisleId in SequenceOfLetters("HW", "HW", 2))
            {
                var aisle = new Aisle(aisleId);
                aisles.Add(aisle);
                foreach (var rackId in SequenceOfNumbers(1, 10, 3))
                {
                    var rack = new Rack(rackId);
                    aisle.Racks.Add(rack);
                    foreach (var levelId in SequenceOfLetters("A", "F", 3))
                    {
                        var level = new Level(levelId);
                        rack.Levels.Add(level);
                        foreach (var binId in SequenceOfNumbers(1, 8, 3))
                        {
                            var bin = new Bin(binId);
                            level.Bins.Add(bin);
                        }
                    }
                }
            }

            return aisles;
        }

        /// <summary>
        /// Segregates locations based on Aisle, Rack, Level, and Bin Designations.
        /// </summary>
        /// <param name="locs"></param>
        /// <param name=""></param>
        public static List<Aisle> SegregateLocations(List<StorageSpace> locs, int columns)
        {
            Dictionary<string, Aisle> aisles = new Dictionary<string, Aisle>();
            throw new Exception("Not yet implemented");
            //Hard-coded for AA:RRR:LLL:BBB splits!!
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aisleStart"></param>
        /// <param name="aisleEnd"></param>
        /// <param name="rackStart"></param>
        /// <param name="rackEnd"></param>
        /// <param name="levelStart"></param>
        /// <param name="levelEnd"></param>
        /// <param name="binStart"></param>
        /// <param name="binEnd"></param>
        /// <returns></returns>
        public static List<Aisle> GenerateLocationSequence(
            int aisleCol,
            int rackCol,
            int levelCol,
            int binCol,
            string aisleStart, string aisleEnd, 
            bool aisleLetters, string rackStart, 
            string rackEnd, bool rackLetters, 
            string levelStart, string levelEnd, 
            bool levelLetters, string binStart,
            string binEnd, 
            bool binLetters)
        {
            List<Aisle> aisles = new List<Aisle>();
            var aisleIds = aisleLetters ? SequenceOfLetters(aisleStart, aisleEnd, aisleCol) : SequenceOfNumbers(uint.Parse(aisleStart), uint.Parse(aisleEnd), aisleCol);
            foreach (var aisleId in aisleIds)
            {
                var aisle = new Aisle(aisleId);
                aisles.Add(aisle);
                var rackIds = rackLetters ? SequenceOfLetters(rackStart, rackEnd, rackCol) : SequenceOfNumbers(uint.Parse(rackStart), uint.Parse(rackEnd), rackCol);
                foreach (var rackId in rackIds)
                {
                    var rack = new Rack(rackId);
                    aisle.Racks.Add(rack);
                    var levelIds = levelLetters ? SequenceOfLetters(levelStart, levelEnd, levelCol) : SequenceOfNumbers(uint.Parse(levelStart), uint.Parse(levelEnd), levelCol);
                    foreach (var levelId in levelIds)
                    {
                        var level = new Level(levelId);
                        rack.Levels.Add(level);
                        var binIds = binLetters ? SequenceOfLetters(binStart, binEnd, binCol) : SequenceOfNumbers(uint.Parse(binStart), uint.Parse(binEnd), binCol);
                        foreach (var binId in binIds)
                        {
                            var bin = new Bin(binId);
                            level.Bins.Add(bin);
                        }
                    }
                }
            }

            return aisles;
        }


        public static ControlPool<ListViewItem> ListItemPool = new ControlPool<ListViewItem>(1500);
        

        /// <summary>
        /// TODO: Need to normalize heights so that visualization always starts at the bottom and staggers tops as needed when various columns have differing numbers of levels
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="view"></param>
        public static void PropogateListView(List<Aisle> locations, ListView view, Regex filterReg)
        {
            view.ShowItemToolTips = true;
            view.Clear();
            int maxBins = (locations == null || locations.Count == 0) ? 0 : locations.Select(x => x.MaxBins).Max();
            int headerCount = 0;
            int rowCount = 0;

            if (locations.Count == 0) return;

            //view.Parent.SuspendLayout();
            //view.SuspendLayout();
            view.BeginUpdate();
            var srter = view.ListViewItemSorter;
            view.ListViewItemSorter = null;
            var tempList = new List<ListViewItem>(500);

            foreach (var aisle in locations)
            {
                foreach (var rack in aisle.Racks)
                {
                    string groupId = aisle.Id + rack.Id;
                    headerCount++;
                    var grp = new ListViewGroup(groupId, groupId);
                    view.Groups.Add(grp);
                    

                    foreach(var level in rack.Levels.Reverse<Level>())
                    {
                        rowCount++;

                        //We need to create enough columns to allow for the maximum bin size scrolling horizontally.
                        //However, any levels that have fewer than the max bin size need to have their remaining spaces
                        //filled with blank data so as to keep the formatting normalized for all shelves in the grid.
                        for(int i = 0; i < maxBins; i++)
                        {
                            var binId = (i >= level.Bins.Count) ? string.Empty : level.Id + level.Bins[i].Id;
                            //if (filterReg == null || filterReg.IsMatch(binId))
                            //{
                            var rowItem = ListItemPool.Retreive();// new ListViewItem(binId, grp);
                            rowItem.Group = grp;
                            rowItem.Text = binId;
                            rowItem.BackColor = Color.DarkSeaGreen;
                            rowItem.ToolTipText = aisle.Id + rack.Id + binId;
                            tempList.Add(rowItem);
                            //view.Items.Add(rowItem);
                            //}
                        }
                    }
                }
            }

            //view.Items.AddRange(tempList.ToArray());

            int verticalFudge = 25;
            int horizontalFudge = 4;
            //view.Height = ((view.Items[0].Bounds.Height * headerCount) + (view.TileSize.Height * rowCount)) + verticalFudge;
            //view.Width = (view.TileSize.Width * (maxBins + 1));// + horizontalFudge;

            view.ListViewItemSorter = srter;
            view.EndUpdate();
            //view.ResumeLayout();
            //view.Parent.ResumeLayout();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public interface ILocationDenomination
    {
        string Id { get; }

        IEnumerable<ILocationDenomination> Children { get; }
        void Sort();
    }


    /// <summary>
    /// 
    /// </summary>
    public class Aisle : ILocationDenomination
    {
        public string Id { get; private set; }
        public List<Rack> Racks = new List<Rack>();
        public IEnumerable<ILocationDenomination> Children { get => Racks.Select(x => x as ILocationDenomination); }
        public int MaxBins {
            get
            {
                int max = -1;
                foreach(var rack in Racks)
                {
                    int maxBins = rack.MaxBins;
                    if (maxBins > max)
                        max = maxBins;
                }
                return max;
            }
        }


        public Aisle(string id)
        {
            Id = id;
        }

        public void Sort()
        {

        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Rack : ILocationDenomination
    {
        public string Id { get; private set; }
        public List<Level> Levels = new List<Level>();
        public IEnumerable<ILocationDenomination> Children { get => Levels.Select(x => x as ILocationDenomination); }

        /// <summary>
        /// Returns the highest number of bins that will be found in any level of this rack.
        /// Effectively giving max bin-column count of this rack.
        /// </summary>
        public int MaxBins
        {
            get
            {
                int max = -1;
                foreach (var level in Levels)
                {
                    int maxBins = level.Bins.Count;
                    if (maxBins > max)
                        max = maxBins;
                }
                return max;
            }
        }

        public Rack(string id)
        {
            Id = id;
        }

        public void Sort()
        {

        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Level : ILocationDenomination
    {
        public string Id { get; private set; }
        public List<Bin> Bins = new List<Bin>();
        public IEnumerable<ILocationDenomination> Children { get => Bins.Select(x => x as ILocationDenomination); }

        public Level(string id)
        {
            Id = id;
        }

        public void Sort()
        {

        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Bin : ILocationDenomination
    {
        public string Id { get; private set; }
        public IEnumerable<ILocationDenomination> Children { get => null; }
        //public List<StorageSpace Locations = new List<StorageSpace>();

        public Bin(string id)
        {
            Id = id;
        }

        public void Sort()
        {

        }
    }
}
