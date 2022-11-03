using ConverterTools.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ConverterStartup.PropertyGridItems
{
    public class OpenFileDialogEditor : FileNameEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value is SerializableOpenFileDialog)
            {
                SerializableOpenFileDialog serializableOpenFileDialog = (SerializableOpenFileDialog)value;
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    AddExtension = serializableOpenFileDialog.AddExtension,
                    CheckFileExists = serializableOpenFileDialog.CheckFileExists,
                    CheckPathExists = serializableOpenFileDialog.CheckPathExists,
                    DefaultExt = serializableOpenFileDialog.DefaultExt,
                    FileName = (serializableOpenFileDialog.Multiselect && (serializableOpenFileDialog.FileNames?? new string[0]).Skip(1).Any()) ? string.Join(" ", serializableOpenFileDialog.FileNames.Select(fn=>string.Concat("\"",System.IO.Path.GetFileName(fn),"\""))) : System.IO.Path.GetFileName(serializableOpenFileDialog.FileName),
                    //FileNames = serializableOpenFileDialog.FileNames,
                    Filter = serializableOpenFileDialog.Filter,
                    FilterIndex = serializableOpenFileDialog.FilterIndex,
                    InitialDirectory = serializableOpenFileDialog.InitialDirectory,
                    Multiselect = serializableOpenFileDialog.Multiselect,
                    RestoreDirectory = serializableOpenFileDialog.RestoreDirectory,
                    Title = serializableOpenFileDialog.Title
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    serializableOpenFileDialog.AddExtension = openFileDialog.AddExtension;
                    serializableOpenFileDialog.CheckFileExists = openFileDialog.CheckFileExists;
                    serializableOpenFileDialog.CheckPathExists = openFileDialog.CheckPathExists;
                    serializableOpenFileDialog.DefaultExt = openFileDialog.DefaultExt;
                    serializableOpenFileDialog.FileName = openFileDialog.FileName;
                    serializableOpenFileDialog.FileNames = openFileDialog.FileNames;
                    serializableOpenFileDialog.Filter = openFileDialog.Filter;
                    serializableOpenFileDialog.FilterIndex = openFileDialog.FilterIndex;
                    serializableOpenFileDialog.InitialDirectory = openFileDialog.InitialDirectory;
                    serializableOpenFileDialog.Multiselect = openFileDialog.Multiselect;
                    serializableOpenFileDialog.RestoreDirectory = openFileDialog.RestoreDirectory;
                    serializableOpenFileDialog.Title = openFileDialog.Title;

                }

                return serializableOpenFileDialog;
            }

            return base.EditValue(context, provider, value);
        }
    }
}
