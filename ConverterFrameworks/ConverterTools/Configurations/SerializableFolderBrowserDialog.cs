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
    public class SerializableFolderBrowserDialog
    {
        public string Description { get; set; }
        public Environment.SpecialFolder RootFolder { get; set; }
        public string SelectedPath { get; set; }
        public bool ShowNewFolderButton { get; set; }
    }
}
