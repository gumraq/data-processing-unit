using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ConverterTools.Configurations;
using ConverterTools;

namespace DataSourceProviders
{
    /// <summary>
    /// Чтение шаблона из ресурса библиотеки
    /// </summary>
    /// <remarks>Предварительно копируется шаблон выходной БД в папку "FolderOut"</remarks>
    public class ResourceReaderSourceProvider : IDataSourceProvider<string>
    {
        private readonly string outFilePath;

        public ResourceReaderSourceProvider(IConfiguration configuration, byte[] resource)
        {
            string folderOut = ((string)configuration["FolderOut"]).TrimEnd('\\', '/');
            string technoFile = (string)configuration["TechnoDb"];
            string fileName = string.Concat(Path.GetFileNameWithoutExtension(technoFile), "_out.mdb");
            this.outFilePath = Path.Combine(folderOut, fileName);

            if (!Directory.Exists(folderOut))
            {
                Directory.CreateDirectory(folderOut);
            }

            using (FileStream fs = new FileStream(this.outFilePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(resource, 0, resource.Length);
            }
        }


        public string Source()
        {
            return this.outFilePath;
        }
    }

}
