using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConverterStartup.PropertyGridItems;
using ConverterTools.Configurations;

namespace ConverterStartup
{
    public partial class ConfigForm : Form
    {
        private readonly IConfiguration configuration;
        public ConfigForm(IConfiguration configuration)
        {
            this.configuration = configuration;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            ICustomTypeDescriptor typeDescriptor = new ConfigurationTypeDescriptor(configuration);
            this.propGrid.SelectedObject = typeDescriptor;
            this.propGrid.PropertySort = PropertySort.Categorized;
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                this.configuration.Save();
            }
            base.OnClosed(e);
        }
    }
}
