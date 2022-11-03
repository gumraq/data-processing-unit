using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ConverterTools.Configurations
{
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class SerializableObjectDictionary : Dictionary<string, object>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read() &&
                   !(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == this.GetType().Name))
            {
                string name = reader["key"];
                if (name == null)
                {
                    throw new FormatException();
                }
                    
                Type type = Type.GetType(reader["valueType"]);
                reader.Read();
                XmlSerializer serializer = new XmlSerializer(type);
                this[name] = serializer.Deserialize(reader);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (string key in Keys)
            {
                writer.WriteStartElement("Pair");
                writer.WriteAttributeString("key", key);
                writer.WriteAttributeString("valueType", this[key].GetType().AssemblyQualifiedName);
                //writer.WriteStartElement("DefaultValue");
                new XmlSerializer(this[key].GetType()).Serialize(writer, this[key]);
                writer.WriteEndElement();
            }
        }
    }
}
