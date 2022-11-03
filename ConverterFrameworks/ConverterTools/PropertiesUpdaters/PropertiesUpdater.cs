using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConverterTools.PropertiesUpdaters
{
    /// <summary>
    /// Обновляет свойства элемента по единому свойству
    /// </summary>
    public abstract class PropertiesUpdater : IPropertiesUpdater
    {
        /// <summary>
        /// тип элемента, который нужно "почистить"
        /// </summary>
        protected Type itemType;
        protected string[] props;
        private Dictionary<string, Type> propsInfo;
        Action<object> act;

        protected abstract Action<object> ActUpdate();

        public virtual T Update<T>(T item, Type propsType) where T : class
        {
            if (ChangeType<T>() || this.props == null || this.props.Length == 0 ||
                this.propsInfo[this.props[0]] != propsType)
            {
                Type propType = propsType;
                this.props = this.propsInfo.Where(p => p.Value == propType).Select(p => p.Key).ToArray();
                this.act = this.ActUpdate();
            }
            this.act(item);
            return item;
        }

        public virtual T Update<T>(T item, string[] props) where T : class
        {
            if (ChangeType<T>() || this.props.SequenceEqual(props))
            {
                string prop = props.FirstOrDefault();
                if (!string.IsNullOrEmpty(prop) && this.propsInfo.ContainsKey(prop))
                {
                    Type propType = this.propsInfo[prop];
                    this.props = this.propsInfo.Where(p => p.Value == propType).Select(p => p.Key).ToArray();
                }
                else
                {
                    this.props = System.Linq.Enumerable.Empty<string>().ToArray();
                }
            }
            this.ActUpdate()(item);
            return item;
        }

        private bool ChangeType<T>()
        {
            bool isNewType = !(itemType == typeof(T));
            if (isNewType)
            {
                this.itemType = typeof(T);
                this.propsInfo = this.itemType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite)
                    .ToDictionary(p => p.Name, p => p.PropertyType);
            }
            return isNewType;
        }
    }
}
