using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SAOT
{
    /// <summary>
    /// Loads and saves app-wide settings including the location of the database directory to use.
    /// </summary>
    public static class Config
    {
        #region ID strings for commonly accessed config data
        public const string DatabaseConfigId = "DatabaseDir";
        public const string MaterialImportCountId = "MaterialImportCount";
        public const string MaterialId = "Material";
        public const string LastDirectory = "LastDirId";
        public static string ExcelReader = "ExcelReader";
        public static string DrawingsRootDirId = "DrawingsRootDir";
        #endregion


        public static string ConfigFilename = "config.ini";
        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string ConfigPath => Path.Combine(AppPath, ConfigFilename);

        static Dictionary<string, string> ConfigMapStr = new Dictionary<string, string>(10);


        public static string DatabaseDir { get => ReadConfigStr(DatabaseConfigId); private set => ChangeConfigStrDirectory(DatabaseConfigId, value); }
        public static bool IsValidConfig => !string.IsNullOrEmpty(DatabaseDir);
        public static string DirectoryOfDb(string filename) => string.IsNullOrEmpty(DatabaseDir) ? string.Empty : Path.Combine(DatabaseDir, filename);




        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void ChangeConfigStr(string configId, string value)
        {
            if (!string.IsNullOrEmpty(value))
                return;

            ConfigMapStr[configId] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void ChangeConfigStrDirectory(string configId, string value)
        {
            if (!string.IsNullOrEmpty(value) && !Directory.Exists(value))
                return;

            ConfigMapStr[configId] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static string ReadConfigStr(string configId, string defaultValue = null)
        {
            if (ConfigMapStr.TryGetValue(configId, out string value))
                return value;
            else if(!string.IsNullOrEmpty(defaultValue))
            {
                WriteConfigStr(configId, defaultValue);
                SaveConfig();
                return defaultValue;
            }
            return null;
        }

        public static void WriteConfigStr(string configId, string value)
        {
            ConfigMapStr[configId] = value;
        }


        static StringBuilder FileoutputText = new StringBuilder(100);
        /// <summary>
        /// Writes current app settings back to a config file located in the app's root directory.
        /// </summary>
        public static void SaveConfig()
        {
            FileoutputText.Clear();

            foreach (var kvp in ConfigMapStr)
            {
                FileoutputText.Append(kvp.Key);
                FileoutputText.Append("=");
                FileoutputText.Append(kvp.Value);
                FileoutputText.Append(Environment.NewLine);
            }

            File.WriteAllText(ConfigPath, FileoutputText.ToString());
        }

        /// <summary>
        /// Loads a config file from the app's root directory.
        /// </summary>
        public static void LoadConfig()
        {
            string text = null;
            try
            {
                text = File.ReadAllText(ConfigPath);
            }
            catch (Exception e)
            {
                return;
            }
            if (string.IsNullOrEmpty(text)) return;


            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(x => x.Trim());

            foreach (var readLine in lines)
            {
                var line = readLine.Trim();
                if (string.IsNullOrEmpty(line)) continue; //skip empty lines
                if (line.StartsWith("//")) continue; //skip comment lines

                var parts = line.Split('=').Select(x => x.Trim()).ToArray();
                if (parts == null || parts.Length != 2) continue;

                string key = parts[0];
                string value = parts[1];
                ConfigMapStr[key] = value;
            }
        }


    }

}
