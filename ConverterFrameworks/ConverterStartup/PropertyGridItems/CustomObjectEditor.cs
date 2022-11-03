using ConverterTools.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ConverterStartup.PropertyGridItems
{
    /// <summary>
    /// Этот класс пока не используется. Основное назначение - сделать единый редактор для ParameterDescriprot для сложных свойств.
    /// </summary>
    public class CustomObjectEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return base.EditValue(context, provider, value);
        }
    
        //public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        //{
        //    //context.Instance as EditableKeyValuePair<string, object>;
        //    Type t = context.Instance.GetType();
        //    if (t.IsEnum)
        //    {
        //        return UITypeEditorEditStyle.DropDown;
        //    }
        //    else if (!t.IsPrimitive && t != typeof(string))
        //    {
        //        return UITypeEditorEditStyle.Modal;
        //    }

        //    return base.GetEditStyle(context);
        //}
    }
}
