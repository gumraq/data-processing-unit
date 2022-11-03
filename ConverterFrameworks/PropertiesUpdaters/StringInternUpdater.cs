using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Expressions;
using ConverterTools.PropertiesUpdaters;

namespace PropertiesUpdaters
{
    public class StringInternUpdater : PropertiesUpdater
    {
        protected override Action<object> ActUpdate()
        {
            ConstantExpression ceNull = Expression.Constant(null, typeof(object));
            ConstantExpression ceThis = Expression.Constant(this);
  
            ParameterExpression peItem = Expression.Parameter(typeof(object), "item");
            IEnumerable<ConditionalExpression> ceCheckProps = base.props.Select(prop =>
            {
                MemberExpression meProp = Expression.Property(Expression.Convert(peItem, base.itemType), prop);
                MethodCallExpression mceIntern = Expression.Call(ceThis, typeof(StringInternUpdater).GetMethod("Intern", new[] { typeof(string) }), meProp);
                BinaryExpression beAssignReplace = Expression.Assign(meProp, mceIntern);
                ConditionalExpression ceIdPropNotNull = Expression.IfThen(Expression.NotEqual(meProp, ceNull), beAssignReplace);
                return ceIdPropNotNull;
            });

            BlockExpression beLambdaBody = Expression.Block(ceCheckProps);
            Expression<Action<object>> le = Expression.Lambda<Action<object>>(beLambdaBody, peItem);
            return le.Compile();
        }
        public string Intern(string str)
        {
            if (str == null)
                return null;

            return string.Intern(str);
        }
    }
}
