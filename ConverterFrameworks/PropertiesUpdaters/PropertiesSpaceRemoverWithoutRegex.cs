using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ConverterTools.PropertiesUpdaters;

namespace PropertiesUpdaters
{
    public class PropertiesSpaceRemoverWithoutRegex : PropertiesUpdater
    {

        public PropertiesSpaceRemoverWithoutRegex()
        {
        }

        protected override Action<object> ActUpdate()
        {
            ConstantExpression ceNull = Expression.Constant(null, typeof (object));
            ConstantExpression ceThis = Expression.Constant(this);
            MemberExpression meThisRegex = Expression.Field(ceThis, "reSpaces");
            MemberExpression meStrEmpty = Expression.Field(null, typeof(string), "Empty");
            ParameterExpression peItem = Expression.Parameter(typeof(object), "item");
            IEnumerable<ConditionalExpression> ceCheckProps = base.props.Select(prop =>
            {
                MemberExpression meProp = Expression.Property(Expression.Convert(peItem, base.itemType), prop);
                MethodCallExpression mceRegexReplace = Expression.Call(meThisRegex, typeof(Regex).GetMethod("Replace",new []{typeof(string),typeof(string)}), meProp, meStrEmpty);
                BinaryExpression beAssignReplace = Expression.Assign(meProp, mceRegexReplace);
                ConditionalExpression ceIdPropNotNull = Expression.IfThen(Expression.NotEqual(meProp, ceNull), beAssignReplace);
                return ceIdPropNotNull;
            });

            BlockExpression beLambdaBody = Expression.Block(ceCheckProps);
            Expression<Action<object>> le = Expression.Lambda<Action<object>>(beLambdaBody, peItem);
            return le.Compile();
        }
    }
}
