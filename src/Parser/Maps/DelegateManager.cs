using System;
using System.Reflection;

namespace EpplusExtension.WorksheetParser.Parser.Maps;

public delegate void PropertySetValueHandler(object obj, object value);

public static class DelegateManager
{
    private static MethodInfo _createPropertySetValueMethodInfo = typeof(DelegateManager).GetMethod(nameof(CreatePropertySetValueDelegate), BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(MethodInfo) });

    public static PropertySetValueHandler CreatePropertySetValueHandler(PropertyInfo propertyInfo)
    {
        var genericCreateDelegateMethodInfo = _createPropertySetValueMethodInfo.MakeGenericMethod(propertyInfo.DeclaringType, propertyInfo.PropertyType);
        return (PropertySetValueHandler)genericCreateDelegateMethodInfo.Invoke(null, new object[] { propertyInfo.SetMethod });
    }

    private static PropertySetValueHandler CreatePropertySetValueDelegate<TClass, TProp>(MethodInfo propSetValueMethodInfo)
    {
        Action<TClass, TProp> stronglyTypedGenericDeleagete = propSetValueMethodInfo.CreateDelegate<Action<TClass, TProp>>();
        return (obj, value) => stronglyTypedGenericDeleagete((TClass)obj, (TProp)value);
    }
}
