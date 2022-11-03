using ConverterTools.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ConverterStartup.PropertyGridItems
{

    /// <summary>
    /// Редактирование свойтва Путик папке
    /// </summary>
    /// <remarks>Штатный класс FolderNameEditor не подходит, потому что не умее запоминать выбранный путь и у него
    /// меньше свойст (например нет возможность вывести кнопку "создать папку")</remarks>
    public class FolderBrowserDialogEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value is SerializableFolderBrowserDialog)
            {
                SerializableFolderBrowserDialog serializableFolderBrowserDialog = (SerializableFolderBrowserDialog)value;
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.Description = serializableFolderBrowserDialog.Description;
                folderBrowserDialog.RootFolder = serializableFolderBrowserDialog.RootFolder;
                folderBrowserDialog.SelectedPath = serializableFolderBrowserDialog.SelectedPath;
                folderBrowserDialog.ShowNewFolderButton = serializableFolderBrowserDialog.ShowNewFolderButton;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    serializableFolderBrowserDialog.Description = folderBrowserDialog.Description;
                    serializableFolderBrowserDialog.RootFolder = folderBrowserDialog.RootFolder;
                    serializableFolderBrowserDialog.SelectedPath = folderBrowserDialog.SelectedPath;
                    serializableFolderBrowserDialog.ShowNewFolderButton = folderBrowserDialog.ShowNewFolderButton;
                }

                return serializableFolderBrowserDialog;
            }
            return base.EditValue(context, provider, value);
        }

        /// <summary>
        /// Возвращает UITypeEditorEditStyle, используемый в методе EditValue
        /// </summary>
        /// <param name="context">Контекст вызова</param>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
