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
    /// <summary>
    /// Класс позволяет преобразовывать object в строки и обратно.
    /// В общем случае это нерешаемая задача и по необходимости его можно расширять.
    /// Пока что класс работает с примитивными типами и строками.
    /// </summary>
    class CustomObjectConverter : TypeConverter
    {
        Type objectType;
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>/// И только так/// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,  object value, Type destType)
        {
            this.objectType = value.GetType();
            if (this.objectType == typeof(object))
            {
                this.objectType = typeof(string);
            }
            return base.ConvertTo(context, culture, value, destType);
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
            TypeConverter typeConverter = TypeDescriptor.GetConverter(this.objectType);
            return typeConverter.ConvertFrom(context, culture, value); 
        }
    }
}
