using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ConverterTools.PropertiesUpdaters;

namespace PropertiesUpdaters.Modules
{
    /// <summary>
    /// Модуль очитки строковых свойств от двойных пробелов и от прочерков (дефисов)
    /// </summary>
    public class StandartTrimStringsModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PropertiesDashReplacer2>().As<IPropertiesUpdater>();
            builder.RegisterType<PropertiesSpaceRemover>().As<IPropertiesUpdater>();
            builder.RegisterType<PropertyUpdatersContainer>();
        }
    }
}
