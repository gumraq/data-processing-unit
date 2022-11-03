using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterStartup.PropertyGridItems
{
    class GenericDictionaryConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(List<>);
        }

        /// <summary>/// И только так/// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,  object value, Type destType)
        {
            //value as List<>
            return "< Справочник... >";
        }
    }
}
