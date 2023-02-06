using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MineMP
{
    public class ConfigFile
    {
        public const string Encode = "UTF-16";
        private readonly static Encoding encoding = Encoding.GetEncoding(Encode);

        public Dictionary<string, string> configs { get; private set; } = new Dictionary<string, string>();
        public string FilePath { get; private set; } = "";
        public bool Loaded { get; private set; } = false;

        public ConfigFile() { }

        // Fast Class Build
        public static ConfigFile FormFile(string path, bool auto_load = false)
        {
            ConfigFile config = new ConfigFile();
            config.UseFile(path);

            if (auto_load)
                config.Load();

            return config;
        }



        // File Processing

        public void UseFile(string path)
        {
            FilePath = path;
            Loaded = false;
        }
        public void Load(bool reload = false)
        {
            if (!reload)
                if (Loaded) return;

            configs.Clear();
            List<string> lines = new List<string>();

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
            }

            lines.AddRange(File.ReadAllLines(FilePath, encoding));

            foreach (string line in lines)
            {
                string key = line.Substring(0, line.IndexOf('='));
                string value = line.Substring(line.IndexOf("=") + 1);

                configs.Add(key, value);
            }

            Loaded = true;
        }
        public void Save()
        {
            List<string> lines = new List<string>();

            if (!File.Exists(FilePath))
            {
                File.CreateText(FilePath).Close();
            }

            foreach (string key in configs.Keys)
            {
                string value_of_key = configs[key];

                string formated = string.Format("{0}={1}", key, value_of_key);
                lines.Add(formated);
            }

            File.WriteAllLines(FilePath, lines.ToArray(), encoding);
        }


        // Data Processing

        public void RemoveAll() { configs.Clear(); }
        public string Remove(string key)
        {
            string value_of_removedKey = "";
            configs.Remove(key, out value_of_removedKey);
            return value_of_removedKey;
        }
        /*
         * If key Exists, then replace its value
         * If value is null, then the key's value will be String.Empty
         */
        public void Add(string key, params object[]? _value)
        {
            string value = _value[0].ToString();
            if (value == null)
                value = string.Empty;

            if (configs.ContainsKey(key))
            {
                configs[key] = value;
            }
            else
            {
                configs.Add(key, value);
            }
        }
        /*
         * If not Exists, String.Empty will be returned.
         */
        public string Get(string key) { if (configs.ContainsKey(key)) return configs[key]; return string.Empty; }
        public bool Set(string key, params object[]? _value)
        {
            string value = _value[0].ToString();
            if (value == null)
                value = string.Empty;

            if (!configs.ContainsKey(key)) return false;

            configs[key] = value;
            return true;
        }
        public void Default(string key, params object[]? _value)
        {
            string value = _value[0].ToString();
            if (value == null)
                value = string.Empty;

            if (!configs.ContainsKey(key))
            {
                Add(key, value);
                return;
            }    
            if (configs[key] == null || configs[key] == string.Empty || !configs.ContainsKey(key))
                Add(key, value);
        }

        // Data Checking
        public bool IsKeyExists(string key) { return configs.ContainsKey(key); }
        public bool IsValueExists(string key_value) { return configs.ContainsValue(key_value); }
        public bool IsKeyValueCorrect(string src_key, string target_key)
        {
            if (!configs.ContainsKey((string)src_key)) return false;

            if (configs[src_key] != target_key) return false;
            return true;
        }

        // Converting
        public bool GetBool(string value)
        {
            switch (value.ToLower())
            {
                default: break;

                case "true": return true;
                case "false": return false;
            }

            if (int.TryParse(value, out int val))
                switch (val)
                {
                    default: return true;

                    case 0: return false;
                }

            return false;
        }
        public bool KeyGetBool(string key)
        {
            return GetBool(Get(key));
        }
        public int GetInt(string value)
        {
            int.TryParse(value, out int val);
            return val;
        }
        public int KeyGetInt(string key)
        {
            return GetInt(Get(key));
        }
        public uint GetUInt(string value)
        {
            uint.TryParse(value, out uint val);
            return val;
        }
        public uint KeyGetUInt(string key)
        {
            return GetUInt(Get(key));
        }
        public string GetString(string value)
        {
            return value;
        }
        public string KeyGetString(string key)
        {
            return GetString(Get(key));
        }
        public char GetChar(string value)
        {
            return value.ToCharArray()[0];
        }
        public char KeyGetChar(string key)
        {
            return GetChar(Get(key));
        }


        // Function Alias
        public void Replace(string key, object? value) { Add(key, value); }
        public void Empty(string key) { Add(key); }
    }
}
