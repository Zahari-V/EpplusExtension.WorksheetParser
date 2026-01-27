using EpplusExtension.WorksheetParser.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EpplusExtension.WorksheetParser.Parser.Maps;

public static class PropertyMapResolver
{
    public static Dictionary<string, PropertyMap> Resolve(Type type)
    {
        if (type == null) throw new ArgumentNullException($"{nameof(type)} cannot be null!");

        return type.GetProperties()
            .Where(pi => pi.GetCustomAttribute<WorksheetParserColumnAttribute>(false)?.Ignore != true)
            .ToDictionary(
                pi => pi.GetCustomAttribute<WorksheetParserColumnAttribute>(false)?.Name ?? pi.Name,
                pi => new PropertyMap(pi),
                StringComparer.OrdinalIgnoreCase);
    }
}
