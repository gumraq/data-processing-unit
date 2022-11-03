using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConverterConsoleStartup.Classes
{
    public class ShellConfig
    {
        public ShellConfig(string converterAssemblyPath, string converterConfigurationPath)
        {
            ConverterAssemblyPath = converterAssemblyPath;
            ConverterConfigurationPath = converterConfigurationPath;
        }

        public string ConverterAssemblyPath { get; set; }
        public string ConverterConfigurationPath { get; set; }
        public static ShellConfig Parse(string[] args)
        {
            if (args.Length != 2) throw new ArgumentException();
            var result = ArgumentParser.Parse(args);
            return new ShellConfig(
                result["ConverterAssemblyPath"], 
                result.TryGetValue("ConverterConfigurationPath", out var cfg)? cfg:null);
        }

        private static class ArgumentParser
        {
            private static Regex _regex = new Regex("(?i)(?<Key>^[^=]+)=(?<Value>.+)", RegexOptions.IgnoreCase);

            public static Dictionary<string, string> Parse(string[] args)
            {
                var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var arg in args)
                {
                    string key = null;
                    string value = null;
                    foreach (Group group in _regex.Match(arg).Groups)
                    {
                        if (group.Name == "Key")
                        {
                            key = group.Value;
                        }
                        if (group.Name == "Value")
                        {
                            value = group.Value;
                        }
                    }
                    result.Add(key, value);
                }
                return result;
            }
        }
    }
}
