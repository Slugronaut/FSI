using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAOT
{
    public static class DrawingLookup
    {

        const string regStr = @"\(([^)]*)\)";
        static Regex reg = new Regex(regStr, RegexOptions.IgnoreCase | RegexOptions.Compiled);


        public static void OpenPdfDoc(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                Dialog.Message($"No document found for {file}.");
                return;
            }
            else
            {
                //Dialog.Message($"{file}.");
                //ThreadStart ths = new ThreadStart(delegate () { System.Diagnostics.Process.Start(file); });
                System.Diagnostics.Process.Start(file);
            }
        }
        
        public static string[] ModIdAndRev(string docId)
        {
            string rev = "0";
            var token = Regex.Replace(docId, @"\s", "");
            if (token.Contains("KR002"))
            {
                int revIndex = token.LastIndexOf("_");
                if (revIndex > -1)
                    rev = token.Substring(revIndex + 1, 1);

                var modIndex = token.IndexOf("DR");
                modIndex += 2;
                string mod = token.Substring(modIndex, 3);
                string processType = token.Substring(modIndex - 3, 1);

                string partType = token.Substring(modIndex + 3, 1); //p, s, c, g?
                string partNum = token.Substring(modIndex + 4, 3);
                return new string[] { mod, processType, partType, partNum, rev };
            }

            return new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docId"></param>
        public static void StartLookupDrawingTask(string docId)
        {
            if (string.IsNullOrEmpty(docId))
                return;

            Task t = Task.Run(() =>
            {
                var modAndRev = DrawingLookup.ModIdAndRev(docId);
                if (modAndRev.Length == 5 && !string.IsNullOrEmpty(modAndRev[0]))
                {
                    var file = DrawingLookup.FindDrawingFromModId(modAndRev[0], modAndRev[1], modAndRev[2], modAndRev[3], modAndRev[4]);
                    DrawingLookup.OpenPdfDoc(file);
                }
                else
                {
                    Dialog.Message($"Could not determine Mod ID, part type, part number, or revision number.\nProcess: {modAndRev[1]}\nMod: {modAndRev[0]}\nPart Type: {modAndRev[2]}\nPart No: {modAndRev[3]}\nRev: {modAndRev[4]}");
                    return;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modId"></param>
        /// <returns></returns>
        public static string FindDrawingFromModId(string modId, string processType, string partType, string partNum, string rev)
        {
            var rootDir = Config.ReadConfigStr(Config.DrawingsRootDirId);
            if (string.IsNullOrEmpty(rootDir) || !Directory.Exists(rootDir))
            {
                Dialog.Message("The directory for the document drawings has not been properly set.");
                return string.Empty;
            }


            //compile a list of all files found in all directories
            List<string> files = new List<string>(100);
            if (modId == "000")
            {
                files.AddRange(Directory.EnumerateFiles($"{rootDir}", $"*kr002*{processType}*{modId}*{partType}*{partNum}*.pdf", SearchOption.AllDirectories));
            }
            else
            {
                var dirs = Directory.GetDirectories(rootDir, $"{modId}_BOM*_REV*", SearchOption.TopDirectoryOnly);
                foreach (var dir in dirs)
                {
                    files.AddRange(Directory.GetFiles(dir, $"kr002*{processType}*{modId}*{partType}*{partNum}*.pdf", SearchOption.AllDirectories));
                }
            }

            //nothing found
            if(files.Count < 1)
            {
                Dialog.Message($"Could not locate a drawing for {processType}{modId} {partType}{partNum}_{rev}.");
                return null;
            }

            //now sift through all of these files and find the one that is the closest match to our desired filename
            string baseFile = $"kr002*{processType}*{modId}*{partType}*{partNum}*{rev}.pdf";
            int bestDist = int.MaxValue;
            int bestIndex = 0;
            for (int i = 0; i < files.Count; i++)
            {
                int dist = LevenshteinDistance.Compute(files[i], baseFile);
                if(dist < bestDist)
                {
                    bestDist = dist;
                    bestIndex = i;
                }
            }

            return files[bestIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modId"></param>
        /// <returns></returns>
        public static string FindDrawingFromModIdOLD(string modId, string partType, string partNum, string rev)
        {
            var rootDir = Config.ReadConfigStr(Config.DrawingsRootDirId);
            if (string.IsNullOrEmpty(rootDir) || !Directory.Exists(rootDir))
            {
                Dialog.Message("The directory for the document drawings has not been properly set.");
                return string.Empty;
            }
            //gather a list of all folders for a given mod, for any revision level
            int revCount = string.IsNullOrEmpty(rev) ? 0 : int.Parse(rev);
            while (revCount >= 0)
            {
                string[] dirs = null;
                if (modId == "000")
                {
                    foreach (var file in Directory.EnumerateFiles($"{rootDir}", $"*kr002*{modId}*{partType}*{partNum}*.pdf", SearchOption.AllDirectories))
                    {
                        return file;
                    }
                    Dialog.Message("Searching for drawings of triple-zero materials is not currently implemented.");
                    return null;
                }
                else dirs = Directory.GetDirectories(rootDir, $"{modId}_BOM*_REV*{revCount}", SearchOption.TopDirectoryOnly);
                if (dirs.Length == 0)
                {
                    //we have one last trick up our sleeve, maybe the revision number is too low!
                    //try finding ANY folder with the given info, regadless of the revision
                    dirs = Directory.GetDirectories(rootDir, $"{modId}_BOM*_REV*", SearchOption.TopDirectoryOnly);
                    if (dirs.Length == 0)
                    {
                        Console.WriteLine($"No directories found for mod {modId} at revision {rev}.");

                    }
                }
                foreach (var dir in dirs)
                {
                    //try all dirs that match
                    var files = Directory.GetFiles(dir, $"kr002*{modId}*{partType}*{partNum}*.pdf", SearchOption.AllDirectories);

                    if (files.Length > 1)
                        Console.WriteLine("Warning: For some reason there is more than one g-level drawing for mod " + modId + "Rev + " + rev);
                    foreach (var file in files)
                    {
                        //check for a pdf that is a g-level drawing
                        Console.WriteLine("Found a drawing file at: " + file);
                        return file;
                    }
                }
                revCount--;
            }


            return string.Empty;
        }
    }


    /// <summary>
    /// Contains approximate string matching
    /// </summary>
    static class LevenshteinDistance
    {
        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}
