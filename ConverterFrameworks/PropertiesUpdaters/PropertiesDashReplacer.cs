using System;
using System.Collections.Generic;
using System.Linq;
using  System.Linq.Expressions;
using ConverterTools.PropertiesUpdaters;

namespace PropertiesUpdaters
{
    public class PropertiesDashReplacer:PropertiesUpdater
    {
        protected override Action<object> ActUpdate()
        {
            ConstantExpression ceDash = Expression.Constant("-");
            ConstantExpression ceBn = Expression.Constant("б/н");
            ConstantExpression ceBd = Expression.Constant("б/д");
            MemberExpression meStrEmpty = Expression.Field(null, typeof(string), "Empty");
            ParameterExpression peItem = Expression.Parameter(typeof(object), "item");
            IEnumerable<ConditionalExpression> ceCheckProps = base.props.Select(prop =>
            {
                MemberExpression meProp = Expression.Property(Expression.Convert(peItem, base.itemType), prop);
                BinaryExpression beDashComparer = Expression.Equal(meProp, ceDash);
                BinaryExpression beBnComparer = Expression.Equal(meProp, ceBn);
                BinaryExpression beBdComparer = Expression.Equal(meProp, ceBd);
                BinaryExpression beOr = Expression.Or(beDashComparer, Expression.Or(beBnComparer,beBdComparer));
                BinaryExpression beAssignEmpty = Expression.Assign(meProp, meStrEmpty);
                return Expression.IfThen(beOr, beAssignEmpty);
            });

            BlockExpression beLambdaBody = Expression.Block(ceCheckProps);
            Expression<Action<object>> le = Expression.Lambda<Action<object>>(beLambdaBody, peItem);
            return le.Compile();
        }
    }
}
