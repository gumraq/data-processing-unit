using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ConverterConsoleStartup.Classes
{
   public class AssemblyResolver
    {
        private readonly string _assemblyPath;
        public AssemblyResolver(AppDomain domain,string assemblyPath)
        {
            _assemblyPath = assemblyPath;
            domain.AssemblyResolve += AssemblyResolve;
        }
        private Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.IndexOf("resources", StringComparison.Ordinal) == -1)
            {
                int z = args.Name.IndexOf(",", StringComparison.Ordinal);
                if (z > 0)
                {
                    string curModulePath = GetCurrentModulePath();
                    if (!String.IsNullOrEmpty(curModulePath))
                    {
                        String newName = Path.Combine(GetCurrentModulePath(),
                            args.Name.Substring(0, z).TrimEnd() + ".dll");
                        if (File.Exists(newName))
                        {
                            return Assembly.LoadFrom(newName);
                        }
                    }
                }
            }
            return null;
        }
        private string GetCurrentModulePath()
        {
            string currDir = Path.GetDirectoryName(_assemblyPath);
            if (null == currDir)
            {
                var entryAsm = Assembly.GetEntryAssembly();
                if (null != entryAsm)
                {
                    currDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                }
            }

            return currDir;
        }
    }
}
