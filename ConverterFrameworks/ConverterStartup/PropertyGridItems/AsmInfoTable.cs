using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Diagnostics;

namespace ConverterStartup.PropertyGridItems
{
    public class AsmInfoTable : DataTable
    {
        public AsmInfoTable()
        {
            this.Columns.Add("ParamName", typeof(String));
            this.Columns.Add("ParamVal", typeof(String));

            this.PrimaryKey = new DataColumn[] { this.Columns[0] };
        }

        public void ClearAsmInfo()
        {
            this.Clear();
        }

        public void LoadAsmInfo(string filePath)
        {
            ClearAsmInfo();
            Assembly asm = Assembly.LoadFile(filePath);

            if (asm!=null)
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                AssemblyTitleAttribute ata = asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute;
                AssemblyDescriptionAttribute ada = asm.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute;
                DateTime buildDate = new DateTime(2000, 1, 1).AddDays(fvi.FileBuildPart).AddSeconds(fvi.FilePrivatePart * 2);
                this.Rows.Add(new Object[] { "Описание", ata.Title });
                this.Rows.Add(new Object[] { "Версия", string.Format("{0} ({1})", fvi.FileVersion, buildDate.ToString("dd.MM.yyyy HH:mm:ss")) });
                this.Rows.Add(new Object[] { "Путь к загруженной сборке", asm.CodeBase });
                this.Rows.Add(new Object[] { "Полное имя", asm.FullName });
                this.Rows.Add(new Object[] { "Подробное описание", ada.Description });
            }
        }
    }
}
