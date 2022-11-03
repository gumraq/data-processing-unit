using System;
using System.Collections.Generic;
using System.Text;

namespace ConverterConsoleStartup
{
    public class ConverterSettings
    {
        public string AssemblyPath { get; private set; }
        public Dictionary<string, string> Arguments { get;  } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        
        private ConverterSettings()
        {

        }
        public static ConverterSettings Parse(string[] args)
        {
            var converterArgs = new ConverterSettings();
            string key = null;
            string value = null;
            foreach (var arg in args)
            {
                if (IsKeyValue(arg))
                {
                    key = PrettifyKeyValue(arg);
                }
                else
                {
                    value = arg;
                }
                if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value))
                {
                    if (IsConverterDllPath(key))
                    {
                        converterArgs.AssemblyPath = value;
                    }
                    else
                    {
                        converterArgs.Arguments.Add(key, value);
                    }
                    key = null;
                    value = null;
                }
            }
            return converterArgs;
        }
        static bool IsConverterDllPath(string value)
        {
            return value.Equals("ConverterDllPath", StringComparison.OrdinalIgnoreCase);
        }
        static bool IsKeyValue(string value)
        {
            return value.StartsWith("/");
        }
        static string PrettifyKeyValue(string value)
        {
            return value.Replace("/","");
        }
    }
}
