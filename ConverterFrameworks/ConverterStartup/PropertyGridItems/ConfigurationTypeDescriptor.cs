using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConverterTools.Configurations;
using Wexman.Design;

namespace ConverterStartup.PropertyGridItems
{
    [TypeConverter(typeof(PropertySorter))]
    class ConfigurationTypeDescriptor : ICustomTypeDescriptor, INotifyPropertyChanged
    {
        private IConfiguration configuration;

        public event PropertyChangedEventHandler PropertyChanged;
        public ConfigurationTypeDescriptor(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptor[] pds =
                this.configuration.Parameters.Values.Select(
                    delegate (ParamDescWithValue q, int index)
                    {
                        List<Attribute> attrs = new List<Attribute> {
                        new DescriptionAttribute(q.ParamDescription),
                        new PropertyOrderAttribute((index + 1) * 10),
                        new CategoryAttribute(q.IsUserParam ? "Параметры пользователя" : "Параметры приложения") };
                        
                        if (q.ParamType == typeof(SerializableDictionary) || q.ParamType == typeof(SerializableObjectDictionary))
                        {
                            attrs.Add(new EditorAttribute(typeof(GenericDictionaryEditor<string, object>), typeof(UITypeEditor)));
                            attrs.Add(new GenericDictionaryEditorAttribute()
                            {
                                Title = $"{q.ParamName} ({q.ParamDescription})",
                                ValueConverterType = typeof(CustomObjectConverter),
                            });
                            attrs.Add(new TypeConverterAttribute(typeof(GenericDictionaryConverter)));
                        }
                        else if (q.ParamType.IsEnum && q.ParamType.IsDefined(typeof(FlagsAttribute),false))
                        {
                            // флаговое перечисление - возможность выбрать в ниспадающем списке несколько записей
                            attrs.Add(new EditorAttribute(typeof(FlagEnumEditor), typeof(UITypeEditor)));
                        }
                        else if (q.ParamType.IsGenericType && (q.ParamType.GetGenericTypeDefinition() == typeof(List<>)|| q.ParamType.GetGenericTypeDefinition() == typeof(Collection<>)))
                        {
                            attrs.Add(new TypeConverterAttribute(typeof(GenericListConverter)));
                            attrs.Add(new EditorAttribute(typeof(CollectionEditor), typeof(UITypeEditor)));
                        }
                        else if (q.ParamType == typeof(SerializableFolderBrowserDialog))
                        {
                            attrs.Add(new EditorAttribute(typeof(FolderBrowserDialogEditor), typeof(UITypeEditor)));
                            attrs.Add(new TypeConverterAttribute(typeof(FolderBrowserDialogConverter)));
                        }
                        else if (q.ParamType == typeof(SerializableOpenFileDialog))
                        {
                            attrs.Add(new EditorAttribute(typeof(OpenFileDialogEditor), typeof(UITypeEditor)));
                            attrs.Add(new TypeConverterAttribute(typeof(OpenFileDialogConverter)));
                        }
                        else if (q.ParamType == typeof(string) && this.IsValidePath((string)q.Value))
                        {
                            attrs.Add(new EditorAttribute(typeof(FileDirectoryEditor), typeof(UITypeEditor)));
                        }
                        else if (q.ParamType == typeof(bool))
                        {
                            attrs.Add(new EditorAttribute(typeof(CheckBoxBooleanEditor), typeof(UITypeEditor)));
                            attrs.Add(new TypeConverterAttribute(typeof(CheckBoxBooleanConverter))); 
                        }
                        else
                        {
                        }
                        return new DynamicProperyDescriptor(this.configuration, q.ParamName, attrs.ToArray());
                    }).Cast<PropertyDescriptor>().ToArray();
            return new PropertyDescriptorCollection
                (pds);
        }

        bool IsValidePath(string path)
        {
            return Regex.IsMatch(path ?? string.Empty, @"^[A-Za-z][:][/\\]|([/\\][/\\][^/\\]+)");
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return this.GetProperties();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this.configuration.Parameters;
        }

        /// <summary>
        /// см. INotifyPropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
