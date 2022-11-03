using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace ConverterTools.Configurations
{
    /// <summary>
    /// Конфигурирование конвертера
    /// </summary>
    public class ConverterConfiguration:IConfiguration
    {
        private string cfgFile;
        public Dictionary<string, ParamDescWithValue> Parameters { get; private set; }

        /// <summary>
        /// Создается конфигурация с параметрами
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cfgFile"></param>
        public ConverterConfiguration(IEnumerable<ParamDescWithValue> parameters, string cfgFile)
        {
            IEnumerable<ParamDescWithValue> paramFromFile = System.Linq.Enumerable.Empty<ParamDescWithValue>();
            if (string.IsNullOrEmpty(cfgFile))
            {
                //this.cfgFile = Path.Combine(Path.GetDirectoryName(base.ThisAssembly.Location), Path.GetFileNameWithoutExtension(base.ThisAssembly.Location) + ".config");
                this.cfgFile = "converter.config";
            }
            else
            {
                this.cfgFile = cfgFile;
                if (File.Exists(cfgFile))
                {
                    paramFromFile = this.Read();
                }
            }

            if (parameters != null)
            {
                this.Parameters = parameters.GroupJoin(paramFromFile, pTemplate => pTemplate.ParamName,
                    pFile => pFile.ParamName,
                    (pTemplate, pFile) =>
                    {
                        ParamDescWithValue p = pFile.FirstOrDefault();
                        if (p != null)
                        {
                            pTemplate.Value = p.Value;
                        }
                        return pTemplate;
                    }).ToDictionary(p => p.ParamName);
            }
            else
            {
                this.Parameters = paramFromFile.ToDictionary(p=>p.ParamName);
            }
        }

        public ConverterConfiguration(IEnumerable<ParamDescWithValue> parameters) : this(parameters, string.Empty)
        {
        }

        public ConverterConfiguration(string cfgFile): this(null, cfgFile)
        {
        }

        public ConverterConfiguration(): this(null, string.Empty)
        {
        }

        public object this[string item]
        {
            get
            {
                if (this.Parameters == null)
                {
                    throw new ArgumentNullException("parameters");
                }
                if (!Parameters.ContainsKey(item))
                {
                    throw new KeyNotFoundException("item");
                }

                return this.Parameters[item].Value;
            }
            set
            {
                if (this.Parameters == null)
                {
                    this.Parameters = new Dictionary<string, ParamDescWithValue>();
                }
                if (!Parameters.ContainsKey(item))
                {
                    Parameters.Add(item, new ParamDescWithValue(item, typeof (object), string.Empty, value));
                }
                else
                {
                    Parameters[item].Value = value;
                }
            }
        }

        /// <summary>
        /// Возвращает значение параметра если он есть и его тип соответствует, иначе возвращает значение типа по умолчанию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public T ParamValueOrDefault<T>(string paramName, string propName = null)
        {
            ParamDescWithValue param;
            if (Parameters.TryGetValue(paramName, out param))
            {
                if (string.IsNullOrEmpty(propName))
                {
                    if (param.ParamType == typeof(T))
                    {
                        return (T)param.Value;
                    }
                }
                else
                {
                    return this.GetProp<T>(param, propName)();
                }

            }

            return default;
        }

        Func<T> GetProp<T>(ParamDescWithValue param2, string propName)
        {
            try
            {
                ConstantExpression ceParamVal = Expression.Constant(param2.Value);
                UnaryExpression ueConvertParamToType = Expression.Convert(ceParamVal, param2.ParamType);
                ParameterExpression peVarItem = Expression.Parameter(param2.ParamType, "item");
                BinaryExpression beAssignParam = Expression.Assign(peVarItem, ueConvertParamToType);
                MemberExpression body = Expression.Property(peVarItem, propName);
                Expression<Func<T>> expr = Expression.Lambda<Func<T>>(body);
                return expr.Compile();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Не удалось создать Getter для свойства \"{0}.{1}\"",
                    typeof(T).FullName ?? "<null>",
                    propName ?? "<null>"
                    ), ex);
            }
        }


        public void Save()
        {
            string dir = Path.GetDirectoryName(this.cfgFile);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                NewLineHandling = NewLineHandling.Entitize,
            };

            using ( var writer = XmlWriter.Create( this.cfgFile, xmlWriterSettings ) )
            {
                writer.WriteStartDocument(true);
                writer.WriteStartElement("configuration");
                writer.WriteStartElement("configSections");
                writer.WriteStartElement("section");
                writer.WriteAttributeString("name", "converter");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("converter");
                foreach (KeyValuePair<string, ParamDescWithValue> paramDescWithValue in Parameters)
                {
                    writer.WriteStartElement("setting");
                    writer.WriteAttributeString("name", paramDescWithValue.Key);
                    writer.WriteAttributeString("serializeAs", "Xml");
                    writer.WriteStartElement("value");
                    new XmlSerializer(typeof(ParamDescWithValue)).Serialize(writer, paramDescWithValue.Value);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }
        }

        private IEnumerable<ParamDescWithValue> Read()
        {
            if (!File.Exists(this.cfgFile))
            {
                yield break;
            }

            XmlSerializer serializer = new XmlSerializer(typeof (ParamDescWithValue));
            XPathDocument document = new XPathDocument(this.cfgFile);
            string xpath = "configuration/converter/setting/value/ParamDescWithValue";
            foreach (XPathNavigator navParam in document.CreateNavigator().Select(xpath))
            {
                yield return (ParamDescWithValue) serializer.Deserialize(navParam.ReadSubtree());
            }
        }

        public string ConfigFile
        {
            get
            {
                return this.cfgFile;
            }
        }
    }
}
