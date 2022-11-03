using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ConverterTools;
using ConverterTools.Configurations;

namespace DataSourceProviders
{
    /// <summary>
    /// Подготовка выходной БД из шаблона.
    /// </summary>
    /// <remarks>Предварительно копируется шаблон выходной БД в папку "FolderOut"</remarks>
    public class OutFromTemplateSourceProvider : IDataSourceProvider<string>
    {
        private readonly string outFilePath;

        public OutFromTemplateSourceProvider(IConfiguration configuration, string outDirPath)
        {
            string folderOut = ((string)configuration["FolderOut"]).TrimEnd('\\', '/');
            string technoFile = (string)configuration["TechnoDb"];
            string fileName = string.Concat(Path.GetFileNameWithoutExtension(technoFile), "_out.mdb");
            this.outFilePath = Path.Combine(folderOut, fileName);

            if (!Directory.Exists(folderOut))
            {
                Directory.CreateDirectory(folderOut);
            }

            File.Copy(Path.Combine(outDirPath, "out.mdb"), outFilePath, true);
        }


        public string Source()
        {
            return this.outFilePath;
        }
    }
}
