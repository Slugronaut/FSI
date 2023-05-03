using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
namespace SAOT.Model
{
    /// <summary>
    /// Utility used for importing old material and inventory data from FSI 1.0
    /// </summary>
    public class OldDataImporter
    {
        static readonly string AppPath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\";
#if USE_DEBUG_DIR
        static readonly string Path = AppPath;
#else
        static readonly string Path = "X:\\Ext\\";
#endif
        const string MatFile = "ItemDefs.txt";
        const string InvFile = "InvDefs.txt";
        public static readonly string MatPath = Path + MatFile;
        public static readonly string InvPath = Path + InvFile;

        public static List<List<string>> MatData = new List<List<string>>();
        public static List<List<string>> InvData = new List<List<string>>();


        public static List<string> SplitRawItemList(string raw)
        {
            return raw.Split(new[] { "\r" }, StringSplitOptions.None).Select(x => x.Trim()).ToList();

        }

        public static List<List<string>> ProcurePickInfoFromItemList(List<string> ids)
        {
            List<List<string>> output = new List<List<string>>();
            foreach (var id in ids)
                output.Add(GetPickLine(id));

            return output;
        }


        #region Convertion
        /// <summary>
        /// Gets the material line data for a given kelox key.
        /// </summary>
        /// <param name="kelox"></param>
        /// <returns></returns>
        static List<string> GetMatDataFromKelox(string kelox)
        {
            if (string.IsNullOrEmpty(kelox)) return null;

            foreach (var mat in MatData)
            {
                if (mat != null && mat.Count > 0 && mat[0] == kelox)
                    return mat;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kelox"></param>
        /// <returns></returns>
        static List<string> GetAllSapFromKelox(string kelox)
        {
            var matData = GetMatDataFromKelox(kelox);
            if (matData != null && matData.Count > 3 && !string.IsNullOrEmpty(matData[1]))
                return new List<string>(matData[1].Split(';'));
            else return new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kelox"></param>
        /// <returns></returns>
        static string GetDescFromKelox(string kelox)
        {
            if (string.IsNullOrEmpty(kelox)) return string.Empty;

            var matData = GetMatDataFromKelox(kelox);
            return matData[2];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kelox"></param>
        /// <returns></returns>
        static string GetLocFromKelox(string kelox)
        {
            if (string.IsNullOrEmpty(kelox)) return string.Empty;

            foreach (var loc in InvData)
            {
                if (loc != null && loc.Count > 0)
                {
                    var keloxes = GetAllKeloxFromLoc(loc[0]);
                    if (keloxes.Contains(kelox))
                        return loc[0];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sap"></param>
        /// <returns></returns>
        static string GetKeloxFromSap(string sap)
        {
            if (string.IsNullOrEmpty(sap)) return string.Empty;

            foreach (var mat in MatData)
            {
                if (mat != null && mat.Count > 0)
                {
                    var saps = GetAllSapFromKelox(mat[0]);
                    if (saps.Contains(sap))
                        return mat[0];
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sap"></param>
        /// <returns></returns>
        static string GetDescFromSap(string sap)
        {
            return GetDescFromKelox(GetKeloxFromSap(sap));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sap"></param>
        /// <returns></returns>
        static string GetLocFromSap(string sap)
        {
            return GetLocFromKelox(GetKeloxFromSap(sap));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        static List<string> GetAllKeloxFromLoc(string loc)
        {
            List<string> kelox = new List<string>();
            if (string.IsNullOrEmpty(loc)) return kelox;

            foreach (var inv in InvData)
            {
                if (inv != null && inv.Count > 2 && inv[0] == loc)
                {
                    var k = inv[1].Split(';');
                    if (k != null && k.Length > 0)
                        kelox.AddRange(k);
                }
            }

            return kelox;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        static List<string> GetAllSapFromLoc(string loc)
        {
            var keloxes = GetAllKeloxFromLoc(loc);
            var saps = new List<string>();
            if (string.IsNullOrEmpty(loc)) return saps;

            foreach (var kelox in keloxes)
                saps.AddRange(GetAllSapFromKelox(kelox));

            return saps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        static List<string> GetAllDescFromLoc(string loc)
        {
            var keloxes = GetAllKeloxFromLoc(loc);
            var descs = new List<string>();
            if (string.IsNullOrEmpty(loc)) return descs;

            foreach (var kelox in keloxes)
                descs.Add(GetDescFromKelox(kelox));

            return descs;
        }
        #endregion

        /// <summary>
        /// Returns a list of mat info in order of: [kelox id, location, item description]
        /// </summary>
        public static List<string> GetPickLine(string id)
        {
            
            var kelox = GetKeloxFromId(id);
            var loc = GetLocFromKelox(kelox);
            var matData = GetMatDataFromKelox(kelox);
            var desc = (matData == null) ? "Unknown Item" : matData[2];

            return new List<string> { kelox, string.IsNullOrEmpty(loc) ? "No Loc" : loc, desc };
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<List<string>> LoadData(string path)
        {
            string text = File.ReadAllText(path);
            var lines = text.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();

            var data = new List<List<string>>();
            foreach (var line in lines)
                data.Add(line.Split(',').ToList());

            return data;
        }

    }
}
*/