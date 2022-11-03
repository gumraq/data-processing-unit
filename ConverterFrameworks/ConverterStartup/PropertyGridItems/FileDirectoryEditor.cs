using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ConverterStartup.PropertyGridItems
{
    public class FileDirectoryEditor : UITypeEditor
    {
        IWindowsFormsEditorService _edSvc = null;
        FolderBrowserDialog _dlgDir = new FolderBrowserDialog();
        OpenFileDialog _dlgFile = new OpenFileDialog();
        ListBox _box = new ListBox();

        public FileDirectoryEditor()
        {
            _box.BorderStyle = BorderStyle.None;
            _box.ForeColor = SystemColors.GrayText;
            _box.Items.AddRange(new object[] { "Выбрать файл...", "Выбрать папку..." });
            _box.SelectedValueChanged += (sender, e) => _edSvc.CloseDropDown();

            _dlgFile.RestoreDirectory = true;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            _box.SelectedItem = null;
            _edSvc.DropDownControl(_box);

            string path = value as string ?? string.Empty;
            FileInfo pathInfo = null;
            try
            {
                pathInfo = new FileInfo(path);
            }
            catch (Exception)
            {
            }

            if (_box.SelectedItem != null)
                switch ((string)_box.SelectedItem)
                {
                    case "Выбрать файл...":
                        if (pathInfo != null && pathInfo.Exists)
                            if ((pathInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                                _dlgFile.InitialDirectory = path;
                            else
                                _dlgFile.FileName = path;
                        return _dlgFile.ShowDialog() == DialogResult.OK
                            ? _dlgFile.FileName
                            : path;
                    case "Выбрать папку...":
                        if (pathInfo != null && pathInfo.Exists)
                            if ((pathInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                                _dlgDir.SelectedPath = path;
                            else
                                _dlgDir.SelectedPath = pathInfo.Directory.FullName;
                        return _dlgDir.ShowDialog() == DialogResult.OK
                            ? _dlgDir.SelectedPath
                            : path;
                }
            return path;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}