using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SAOT.Model
{
    public interface IProjectProvider
    {
        Project CurrentSelectedProject { get; }
    }


    /// <summary>
    /// Contains all information relating to a project including warehouse modeling data sets.
    /// </summary>
    public class Project
    {
        public readonly string Id;

        Dictionary<string, Warehouse> Warehouses = new Dictionary<string, Warehouse>();
        List<Material> Materials;
        Dictionary<string, Material> MaterialSapLookup = new Dictionary<string, Material>(500);
        Dictionary<string, Material> MaterialKeloxLookup = new Dictionary<string, Material>(500);


        public List<Material> MaterialsList { get => Materials; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Project(string id)
        {
            Id = id;

            UpdateMaterialsList();
            //ImportProjectMats(null);
            //LoadWarehouses();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void UpdateMaterialsList()
        {
            Materials = MaterialsDb.RequestAllMaterials();

            MaterialKeloxLookup.Clear();
            MaterialSapLookup.Clear();
            foreach(var mat in Materials)
            {
                MaterialKeloxLookup[mat.DocumentId] = mat;
                MaterialSapLookup[mat.MatId] = mat;
            }
        }

        /// <summary>
        /// Imports all materials spreadsheets listed in the app's config file.
        /// </summary>
        /// <param name="msg"></param>
        void ImportProjectMats(UserLoggedInEvent msg)
        {
            string currFile = string.Empty;
            string matFile = string.Empty;
            try
            {
                int.TryParse(Config.ReadConfigStr(Config.MaterialImportCountId), out int matFileCount);

                for (int i = 0; i < matFileCount; i++)
                {
                    currFile = Config.MaterialId + i;
                    matFile = Config.ReadConfigStr(currFile);

                    if (!string.IsNullOrEmpty(matFile))
                    {
                        switch (Config.ReadConfigStr(Config.ExcelReader))
                        {
                            case "FastExcel":
                                {
                                    ImportMaterialsSpreadSheetFastExcel(matFile);
                                    break;
                                }
                            case "ExcelDataReader":
                                {
                                    ImportMaterialsSpreadSheetExcelDataReader(matFile);
                                    break;
                                }
                            default:
                                {
                                    MsgDispatch.PostMessage(new FatalErrorMessage("No Excel Spreadsheet reader method was specified in the config file. This application cannot read excel sheets without one."));
                                    return;
                                }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MsgDispatch.PostMessage(new FatalErrorMessage($"There was an error pasring the material file '{matFile}' for the project {Id}\n\n{e.Message}"));
                return;
            }

        }

        /// <summary>
        /// Imports a spreadsheet containing all exported SAP materials list for a project.
        /// </summary>
        /// <param name="fileName"></param>
        void ImportMaterialsSpreadSheetFastExcel(string fileName)
        {
            Materials = new List<Material>(500);
            //string filePath = Path.Combine(Config.AppPath, fileName);
            string filePath = fileName;


            //first, import materials sheet
            var fileInfo = new FileInfo(filePath);
            using (FastExcel.FastExcel fastExcel = new FastExcel.FastExcel(fileInfo))
            {
                int rowCount = 0;
                var sheet = fastExcel.Read("sheet1");
                //var sheet = fastExcel.Worksheets[0];
                var rows = sheet.Rows;
                foreach (var row in rows)
                {
                    if (rowCount == 0)
                    {
                        rowCount++;
                        continue;
                    }
                    var cells = row.Cells.ToArray();
                    var idCell = cells[0];//row.GetCellByColumnName("Material");
                    var descCell = cells[1];//row.GetCellByColumnName("Material Description");
                    var grpCell = cells[2];// row.GetCellByColumnName("Material Group");
                    var docCell = cells[3];// row.GetCellByColumnName("Old material number");
                                           //var umCell      = row.GetCellByColumnName(

                    string id = idCell.Value as string;
                    string doc = docCell.Value as string;
                    string desc = descCell.Value as string;
                    var um = "pc";
                    var grp = grpCell.Value as string;

                    var mat = new Material(id, doc, desc, um, grp);
                    Materials.Add(mat);
                    if (string.IsNullOrEmpty(doc))
                        doc = id;

                    MaterialKeloxLookup[doc] = mat;
                    MaterialSapLookup[id] = mat;

                    rowCount++;
                }
            }


        }


        /// <summary>
        /// Imports a spreadsheet containing all exported SAP materials list for a project.
        /// </summary>
        /// <param name="fileName"></param>
        void ImportMaterialsSpreadSheetExcelDataReader(string fileName)
        {
            Materials = new List<Material>(3000);


            //string filePath = Path.Combine(Config.AppPath, fileName);
            string filePath = fileName;
            //first, import materials sheet
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    //we are only going to read the first sheet
                    while (reader.Name != "Sheet1")
                        reader.NextResult();

                    if (reader.Name == "Sheet1")
                    {
                        Console.WriteLine("Valid materials sheet found. Importing material data now...");
                        //var result = reader.AsDataSet();
                        Dictionary<string, int> columnMap = new Dictionary<string, int>();
                        int rowCount = 0;
                        while (reader.Read())
                        {
                            if (rowCount == 0)
                            {
                                //this is just the header for column titles
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string colName = reader.GetString(i);
                                    if (string.IsNullOrEmpty(colName))
                                        continue;
                                    columnMap[colName] = i;
                                }
                            }
                            else
                            {
                                int idCol;
                                int docCol;
                                int descCol;
                                int uomCol;
                                int grpCol;
                                try
                                {
                                    idCol = columnMap["Material"];
                                    docCol = columnMap["Old material number"];
                                    descCol = columnMap["Material Description"];
                                    uomCol = columnMap["UM"];
                                    //grpCol = columnMap["Material Group"];
                                }
                                catch(Exception e)
                                {
                                    MessageBox.Show("Incorrect header format for materials list\n" + e.Message);
                                    return;
                                }

                                string id;
                                string doc;
                                string desc;
                                string um;
                                //string grp;
                                int x = 0;
                                try
                                {
                                    id = reader.GetString(idCol);
                                    if (string.IsNullOrEmpty(id))
                                        continue;
                                    x++;
                                    doc = reader.GetString(docCol);
                                    x++;
                                    desc = reader.GetString(descCol);
                                    x++;
                                    um = reader.GetString(uomCol);
                                    x++;
                                    //grp = reader.GetString(grpCol);
                                    //x++;
                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                                var mat = new Material(id, doc, desc, um, string.Empty);
                                Materials.Add(mat);
                                if (string.IsNullOrEmpty(doc))
                                    doc = id;

                                MaterialKeloxLookup[doc] = mat;
                                MaterialSapLookup[id] = mat;

                            }
                            rowCount++;
                        }
                        Console.WriteLine("... material sheet import complete.");
                    }
                    else
                    {
                        throw new Exception("No valid material spreadsheet could be found. Please be sure that all information is contained on a sheet entitled 'Sheet1' and that the following columns exist:\n\tComponent number\n\tDocument / Drawing\n\tObject description\n\tUn\n\tMatl Group");
                    }
                }

            }
        }

        /// <summary>
        /// Loads warehoue information from the database.
        /// </summary>
        public void LoadWarehouses()
        {
            List<string> warehouseNames = Warehouse.RequestWarehouseNames();
            foreach (var name in warehouseNames)
                Warehouses.Add(name, Warehouse.RequestWarehouseData(name));
        }

        /// <summary>
        /// Builds a cross-reference link between unique material numbers that point to related documents / parts.
        /// </summary>
        public void CrossReferenceMaterials()
        {
            throw new Exception("Not yet implemented");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Material GetMaterialFromUnknownId(string id)
        {
            Material mat;
            var kelox = GetKeloxFromId(id);
            if (string.IsNullOrEmpty(kelox))
            {
                MaterialSapLookup.TryGetValue(id, out mat);
                return mat;
            }
            MaterialKeloxLookup.TryGetValue(kelox, out mat);
            return mat;
        }

        /// <summary>
        /// Susses out if the id given is a kelox number or an SAP number
        /// and returns the associated kelox document. This method internally uses
        /// a lot of hueristics to guess when the number given is all screwed up.
        /// Returns an empty string if it can't be determined.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetKeloxFromId(string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;

            //look like the office didn't do a good job of actually reading the email they go from the vendors andwe now have some ad excuse
            //for a material number. let's see if we can't figure it out anyway
            if (id.Length == 9)
            {
                //this is probably a kelox id? but who really knows?
                if (id[5] == '.')
                    return id; //as sure as any human can be that it's a kelox id
                else if (id[5] == '-' || id[5] == '_')
                {
                    //probably a kelox id but with those lovely random seperators that vendors like to use instead of a period the way its supposed to be
                    var chrArr = id.ToCharArray();
                    chrArr[5] = '.';
                    id = new string(chrArr);
                    return id;
                }

            }


            if (id.Length < 9 && (id.IndexOf('.') == 5 || id.IndexOf('_') == 5 || id.IndexOf('-') == 5))
            {
                //it could be a kelox id? it's very possible some dumbbell
                //just didn't properly convert their fucking spreadsheet column to text
                //and the number ended in zero? again. for the one-hundreth time.
                var chrArr = id.ToCharArray().ToList();
                chrArr[5] = '.';

                while (chrArr.Count < 9)
                    chrArr.Add('0');

                id = new string(chrArr.ToArray());
                return id;
            }



            // --------- SAP MATERIAL ID -------- //
            if (id.Length == 8 && Int32.TryParse(id, out int temp))
            {
                //very likely an SAP material id
                return GetKeloxFromSap(id);
            }
            // --------- SAP MATERIAL ID -------- //



            //at this point who even knows, maybe it's a KR002 number?
            //look for obvious markers such as:
            //  KR002 at the beginning
            //  letters such as P, S, C, or G
            //
            //if so, remove all underscores and whitespace except for a single
            //underscore that occurs AFTER the part, c-level, or g-level number
            //since it signifies a revision number
            id = id.ToUpper();
            if (id.Contains("S") || id.Contains("P") || id.Contains("C") || id.Contains("G"))
            {
                int typeIndex = id.IndexOf('S');
                if (typeIndex < 0) typeIndex = id.IndexOf('P');
                if (typeIndex < 0) typeIndex = id.IndexOf('C');
                if (typeIndex < 0) typeIndex = id.IndexOf('G');
                if (typeIndex < 0) throw new Exception("The material '" + id + "' is literally impossible to identify. Please ask someone in the office to go do it again but correctly this time.");

                //yeah, this is very probably a mode and part number of some kind.
                //now, let's see if it has a revision underscore.
                id = id.Replace("rev", "_");
                int revUs = -1;
                int i = 0;
                while (revUs < typeIndex && i < id.Length)
                {
                    revUs = id.IndexOf('_', i);
                    if (revUs < 0) break;
                    i = revUs + 1;
                }

                if (revUs > -1)
                {
                    //we found a revision number, temporarily replace it with a funky symbol
                    var chrArr = id.ToCharArray();
                    chrArr[revUs] = '$';
                    id = chrArr.ToString();

                    //remove all other underscores and spaces
                    id = id.Replace("_", string.Empty);
                    id = id.Trim();
                    id = id.Replace(" ", string.Empty);
                    id = id.Replace("\n", string.Empty);
                    id = id.Replace("\r", string.Empty);

                    //restore revision id
                    id = id.Replace('$', '_');
                }

                //TODO: now we need to do some hueristics and fill in some pattern-matching
                //just in case people only gave us a mod number and part/sub number

                return id;
            }


            //okay, i give  up at this point. who knows what number this is
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Material FindMaterial(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            MaterialSapLookup.TryGetValue(id, out Material mat);
            return mat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetKeloxFromSap(string id)
        {
            if (!MaterialSapLookup.TryGetValue(id, out Material mat))
                return string.Empty;

            return mat.DocumentId;
        }

        /// <summary>
        /// Returns a list of SAP numbers for a given kelox doc number. It ignores revesion numbers
        /// and returns all version in SAP form.
        /// </summary>
        /// <param name="matId"></param>
        /// <returns></returns>
        public List<string> GetAllSapFromKelox(string matId)
        {
            throw new Exception("All SAPs from Kelox not yet implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kelox"></param>
        /// <returns></returns>
        public string GetPrimeSapFromKelox(string kelox)
        {
            throw new Exception("Prime SAP from Kelox Not yet implemented");
        }

    }
}
