using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace ConverterStartup.PropertyGridItems
{
    public class FlagEnumEditor : UITypeEditor
    {
        CheckedListBox _box = new CheckedListBox();

        public FlagEnumEditor()
        {
            _box.CheckOnClick = true;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Type edType = context.PropertyDescriptor.PropertyType;

            _box.Items.Clear();
            int i = 0;
            long lValue = Convert.ToInt64(value);
            foreach (Enum val in Enum.GetValues(edType))
            {
                _box.Items.Add(val);
                long lVal = Convert.ToInt64(val);
                if ((lValue & lVal) == lVal && (lVal != 0 || lValue == 0))
                    _box.SetItemChecked(i, true);
                i++;
            }

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider
                .GetService(typeof(IWindowsFormsEditorService));
            edSvc.DropDownControl(_box);

            lValue = 0;
            foreach (Enum val in _box.CheckedItems)
                lValue |= Convert.ToInt64(val); 

            return Enum.ToObject(edType, lValue);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}
