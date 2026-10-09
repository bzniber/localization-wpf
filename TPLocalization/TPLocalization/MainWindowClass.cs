using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TPLocalization
{
    public partial class MainWindow
    {
        struct ClassDefinition
        {
            public string typeName;
            public string sourceName;
            public string extention;
            public Dictionary<string, Func<FileDefinition, string>> stringKeys;
        }

        struct FileDefinition(string _n, List<string> _loc, DataGrid _grid)
        {
            public string functionName = _n;
            public List<string> supportedLoc = _loc;
            public DataGrid datagrid = _grid;
            public int currentLocIndex = 0;
        }

        static Dictionary<string, Func<FileDefinition, string>> globalKeys = new()
        {
            {"%FuncName%", ReturnName},
            {"%LocArray%", GetLocalisationTab },
            {"%DictCultInit%", CultureInitializersDict },
            {"%LocIteration%", IterateCurrentLocID }
        };

        //// Functions //////////////////////////////////////////////

        static string ReturnName(FileDefinition _def) { return _def.functionName; }
        static string GetLocalisationTab(FileDefinition _def)
        {
            return string.Join(",", _def.supportedLoc.ToArray().Select(x => string.Format("\"{0}\"", x)).ToList());
        }
        static string CultureInitializersDict(FileDefinition _def)
        {
            string dictCulturesInit = string.Empty;
            for (int i = 0; i < _def.supportedLoc.Count; i++)
            {
                string locID = _def.supportedLoc[i];
                dictCulturesInit += "\n\t\t{\"" + locID + "\", LoadCulture_" + locID + "}";
                if (i < _def.supportedLoc.Count - 1)
                    dictCulturesInit += ",";
            }
            return dictCulturesInit;
        }

        static string IterateCurrentLocID(FileDefinition _def)
        {
            string output = string.Empty;

            if (_def.currentLocIndex >= 0 && _def.currentLocIndex < _def.supportedLoc.Count)
            {
                output = _def.supportedLoc[_def.currentLocIndex];
                _def.currentLocIndex = (++_def.currentLocIndex) % _def.supportedLoc.Count;
            }

            return output;
        }

        static string DictionariesInitializersCsharp(FileDefinition _def)
        {
            string dicts = string.Empty;
            // string dictionaries initializers
            for (int i = 0; i < _def.supportedLoc.Count; i++)
            {
                string locID = _def.supportedLoc[i];
                string localisedFunction = "\n\tstatic void LoadCulture_" + locID +
                    "()\r\n\t{\r\n\t\tlocalisations = new Dictionary<string, string>()\r\n\t\t{";

                // iterate over all the keys
                foreach (DataRowView row in _def.datagrid.ItemsSource)
                {

                    string key = string.Empty;
                    string value = string.Empty;

                    for (int colID = 0; colID < _def.datagrid.Columns.Count; colID++)
                    {
                        string? localHeader = _def.datagrid.Columns[colID].Header.ToString()?.ToLower();

                        string? rowValue = row[colID].ToString()?.ToLower();
                        if (rowValue == null)
                            continue;

                        if (localHeader == "id")
                            key = rowValue;
                        if (localHeader == locID)
                            value = rowValue;
                    }

                    if (key != string.Empty && value != string.Empty)
                        localisedFunction += "\n\t\t\t{\"" + key + "\", \"" + value + "\"},";
                }

                localisedFunction = localisedFunction.TrimEnd(',');
                localisedFunction += "\n\t\t};\n\t}";

                dicts += localisedFunction;
            }

            return dicts;
        }

        static string CppHeaderLoader(FileDefinition _def)
        {
            string output = string.Empty;
            foreach (string loc in _def.supportedLoc)
            {
                output += $"\n\t\tstatic void LoadCulture_{loc}(LocMap&);";
            }

            return output;
        }

        static string CppMainLodaers(FileDefinition _def)
        {
            string output = string.Empty;
            foreach (string loc in _def.supportedLoc)
            {
                output += $"void {_def.functionName}::LoadCulture_{loc}(LocMap& _map)";
                output += "\n\t{\n\t\t_map.clear();\n\t\t_map = {";

                foreach (DataRowView row in _def.datagrid.ItemsSource)
                {

                    string key = string.Empty;
                    string value = string.Empty;

                    for (int colID = 0; colID < _def.datagrid.Columns.Count; colID++)
                    {
                        string? localHeader = _def.datagrid.Columns[colID].Header.ToString()?.ToLower();

                        string? rowValue = row[colID].ToString()?.ToLower();
                        if (rowValue == null)
                            continue;

                        if (localHeader == "id")
                            key = rowValue;
                        if (localHeader == loc)
                            value = rowValue;
                    }

                    if (key != string.Empty && value != string.Empty)
                        output += "\n\t\t\t{\"" + key + "\", \"" + value + "\"},";
                }

                output += "\n\t\t};\n}\n";
            }
            return output;
        }
        /////////////////////////////////////////////////////////////

        public void ExportCpp()
        {
            ClassDefinition cppDefC = new()
            {
                typeName = "Cpp Class",
                extention = ".cpp",
                sourceName = "TemplateCpp_Cpp.txt",
                stringKeys = new()
                {
                    {"%MainLoaders%", CppMainLodaers }
                }
            };
            ClassDefinition cppDefH = new()
            {
                typeName = "Cpp Header",
                extention = ".h",
                sourceName = "TemplateCpp_H.txt",
                stringKeys = new()
                {
                    {"%HeaderLoaders%", CppHeaderLoader }
                }
            };

            ExportToClass(cppDefC);
            ExportToClass(cppDefH);
        }
        public void ExportCSharp()
        {
            ClassDefinition cppDef = new()
            {
                typeName = "CSharp Class",
                extention = ".cs",
                sourceName = "TemplateCS.txt",
                stringKeys = new()
                {
                    {"%InitDictionaries%", DictionariesInitializersCsharp }
                }
            };

            ExportToClass(cppDef);
        }

        void ExportToClass(ClassDefinition _def)
        {
            // locate source file
            string sourcePath = AppDomain.CurrentDomain.BaseDirectory + "ClassTemplates\\" + _def.sourceName;
            if (!File.Exists(sourcePath))
            {
                MessageBox.Show($"Error: Template File not found\n({sourcePath})");
                return;
            }

            // ask for output path
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = $"{_def.typeName} |*{_def.extention}";
            if (saveFileDialog.ShowDialog() != true)
                return;

            // if valid, proceed
            StreamReader Class_Template = new(sourcePath);
            string pathName = saveFileDialog.FileName;
            StreamWriter Class_File = new(pathName);


            // generate list of supported languages
            List<string> supportedLoc = [];
            var columns = datagrid.Columns;
            foreach (var column in columns)
            {
                string? header = column.Header.ToString()?.ToLower();
                if (header == null || header == "comment")
                    continue;

                if (header == "id")
                    continue;

                supportedLoc.Add(header);
            }

            string funcName = GetFileNameFromPath(pathName);
            FileDefinition fileDef = new(funcName, supportedLoc, datagrid);

            string? line = Class_Template.ReadLine();
            while (line != null)
            {
                foreach (var key in globalKeys.Keys)
                {
                    if (line.Contains(key))
                    {
                        line = line.Replace(key, globalKeys[key](fileDef));
                    }
                }

                foreach (var key in _def.stringKeys.Keys)
                {
                    if (line.Contains(key))
                    {
                        line = line.Replace(key, _def.stringKeys[key](fileDef));
                    }
                }

                Class_File.WriteLine(line);
                line = Class_Template.ReadLine();
            }

            Class_File.Close();
            Class_Template.Close();

            MessageBox.Show("Done");
        }

        static string GetFileNameFromPath(string _path)
        {
            if (string.IsNullOrEmpty(_path))
                return "Localisation";

            int start = _path.LastIndexOf('\\') + 1;
            int end = _path.LastIndexOf('.');
            string fileName = _path.Substring(start, end - start);
            return fileName.First().ToString().ToUpper() + fileName.Substring(1).ToLower();
        }
    }
}
