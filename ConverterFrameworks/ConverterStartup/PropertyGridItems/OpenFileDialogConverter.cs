using ConverterTools.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConverterStartup.PropertyGridItems
{
    class OpenFileDialogConverter : TypeConverter
    {
        SerializableOpenFileDialog serializableOpenFileDialog;
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(SerializableOpenFileDialog))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>/// И только так/// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,  object value, Type destType)
        {
            SerializableOpenFileDialog serializableOpenFileDialog = value as SerializableOpenFileDialog;
            if (serializableOpenFileDialog != null)
            {
                this.serializableOpenFileDialog = serializableOpenFileDialog;
                return serializableOpenFileDialog.Multiselect ? (string.Join(" | ", serializableOpenFileDialog.FileNames ?? Enumerable.Empty<string>())) : serializableOpenFileDialog.FileName ?? string.Empty;
            }
            return value;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string sVal = value as string;
            if (sVal != null)
            {
                
                if (this.serializableOpenFileDialog.Multiselect)
                {
                    this.serializableOpenFileDialog.FileNames = sVal.Split('|');
                }
                else
                {
                    this.serializableOpenFileDialog.FileName = sVal;
                }
                this.serializableOpenFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(sVal.Split('|')[0]);
                return this.serializableOpenFileDialog;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
