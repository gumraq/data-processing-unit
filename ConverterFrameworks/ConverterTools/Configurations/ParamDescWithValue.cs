using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace ConverterTools.Configurations
{
    /// <summary>
    /// Разветнутое описание параметра со значением
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class ParamDescWithValue: IXmlSerializable
    {
        /// <summary>
        /// Имя параметра
        /// </summary>
        public String ParamName { get; private set; }

        /// <summary>
        /// Тип значения параметра
        /// </summary>
        public Type ParamType { get; private set; }

        /// <summary>
        /// Описание параметра
        /// </summary>
        public String ParamDescription { get; private set; }
        /// <summary>
        /// Является ли параметр пользовательским
        /// </summary>
        public Boolean IsUserParam { get; private set; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public object Value { get; set; }

        #region Конструкторы

        private ParamDescWithValue()
        {
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="paramName">Имя параметра</param>
        public ParamDescWithValue(String paramName)
            : this(paramName, typeof(String), string.Empty, default(object), true)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="paramName">Имя параметра</param>
        /// <param name="paramType">Тип параметра</param>
        public ParamDescWithValue(String paramName, Type paramType)
            : this(paramName, paramType, string.Empty, default(object), true)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="paramName">Имя параметра</param>
        /// <param name="paramType">Тип параметра</param>
        public ParamDescWithValue(String paramName, Type paramType, String paramDescription)
            : this(paramName, paramType, paramDescription, default(object), true)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramType"></param>
        /// <param name="paramDescription"></param>
        /// <param name="value"></param>
        public ParamDescWithValue(string paramName, Type paramType, string paramDescription, object value)
            : this(paramName, paramType, paramDescription, value, true)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramType"></param>
        /// <param name="paramDescription"></param>
        /// <param name="value"></param>
        public ParamDescWithValue(string paramName, Type paramType, string paramDescription, object value, bool isUserParam)
        {
            ParamName = paramName;
            ParamType = paramType;
            ParamDescription = paramDescription;
            Value = value;
            IsUserParam = isUserParam;
        }

        #endregion

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == this.GetType().Name)
            {
                this.ParamName = reader.GetAttribute("name");
                this.ParamDescription = reader.GetAttribute("description");
                this.IsUserParam = bool.Parse(reader.GetAttribute("visible"));
                this.ParamType = Type.GetType(reader.GetAttribute("type") ?? "object");
                if (reader.Read() && reader.LocalName == "DefaultValue")
                {
                    reader.Read();
                    XmlSerializer serializer = new XmlSerializer(this.ParamType);
                    this.Value = serializer.Deserialize(reader);
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", this.ParamName);
            writer.WriteAttributeString("type", this.ParamType.AssemblyQualifiedName);
            writer.WriteAttributeString("description", this.ParamDescription);
            writer.WriteAttributeString("visible", this.IsUserParam.ToString());
            writer.WriteStartElement("DefaultValue");

            new XmlSerializer(this.ParamType).Serialize(writer, this.Value);
            writer.WriteEndElement();
        }
    }
}
