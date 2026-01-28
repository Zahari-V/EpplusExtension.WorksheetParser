using System.Linq.Expressions;
using System.Reflection;

namespace EpplusExtension.WorksheetParser.Parser.ClassMapper;

public delegate void PropertySetValueHandler(object obj, object value);

public static class ExpressionManager
{
    public static PropertySetValueHandler CreatePropertySetValueHandler(PropertyInfo propertyInfo)
    {
        ParameterExpression objectParamExpr = Expression.Parameter(typeof(object), "obj");
        ParameterExpression valueParamExpr = Expression.Parameter(typeof(object), "value");

        return Expression.Lambda<PropertySetValueHandler>(
                Expression.Assign(Expression.Property(Expression.Convert(objectParamExpr, propertyInfo.DeclaringType), propertyInfo), Expression.Convert(valueParamExpr, propertyInfo.PropertyType))
                , objectParamExpr
                , valueParamExpr)
            .Compile();
    }
}
