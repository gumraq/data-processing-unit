using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ConverterTools.Configurations;

namespace ConverterStartup.PropertyGridItems
{
    /// <summary>
    /// Для предоставления информации о свойствах
    /// </summary>
    class DynamicProperyDescriptor : PropertyDescriptor
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="name"></param>
        /// <param name="attrs"></param>
        public DynamicProperyDescriptor(IConfiguration configuration, string name, Attribute[] attrs)
            : base(name, attrs)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            this.configuration = configuration;
        }

        /// <summary>
        /// см. PropertyDescriptor
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override bool CanResetValue(object component)
        {
            return true;
        }

        /// <summary>
        /// см. PropertyDescriptor
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override object GetValue(object component)
        {
            return this.configuration[Name];
        }

        /// <summary>
        /// см. PropertyDescriptor
        /// </summary>
        /// <param name="component"></param>
        public override void ResetValue(object component)
        {
            ParamDescWithValue p = this.configuration.Parameters[Name];
            p.Value = null;            // ИСПРАВИТЬ!!!
        }

        /// <summary>
        /// см. PropertyDescriptor
        /// </summary>
        /// <param name="component"></param>
        /// <param name="value"></param>
        public override void SetValue(object component, object value)
        {
            this.configuration[Name] = value;
        }

        /// <summary>
        /// см. PropertyDescriptor
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        /// <summary>
        /// см. PropertyDescriptor
        /// </summary>
        public override Type ComponentType
        {
            get { return null; }
        }

        /// <summary>
        /// см. PropertyDescriptor
        /// </summary>
        public override bool IsReadOnly { get { return false; } }

        /// <summary>
        /// см. PropertyDescriptor
        /// </summary>
        public override Type PropertyType { get { return configuration.Parameters[Name].ParamType; } }
    }
}
