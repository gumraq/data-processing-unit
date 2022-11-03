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
    class FolderBrowserDialogConverter : TypeConverter
    {
        SerializableFolderBrowserDialog serializableFolderBrowserDialog;

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(SerializableFolderBrowserDialog))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>/// И только так/// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            SerializableFolderBrowserDialog serializableFolderBrowserDialog = value as SerializableFolderBrowserDialog;
            if (serializableFolderBrowserDialog != null)
            {
                this.serializableFolderBrowserDialog = serializableFolderBrowserDialog;
                return serializableFolderBrowserDialog.SelectedPath;
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

                this.serializableFolderBrowserDialog.SelectedPath = sVal;
                return this.serializableFolderBrowserDialog;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
