using EpplusExtension.WorksheetParser.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EpplusExtension.WorksheetParser.Parser.Maps;

public class ClassMap
{
    public ClassMap(Type type)
    {
        Type = type ?? throw new ArgumentNullException($"{nameof(type)} cannot be null!");
        PropertyMapCollection = PropertyMapResolver.Resolve(type);
    }

    public Type Type { get; private init; }

    public Dictionary<string, PropertyMap> PropertyMapCollection { get; private init; }
}

public static class ClassMapExtension
{
    public static IEnumerable<string> GetPropertiesKeyWithUndefinedIndex(this ClassMap classMap)
        => classMap.PropertyMapCollection.Where(m => m.Value.ColumnIndex == WorksheetParserConstant.UNDEFINED_INDEX)
            .Select(m => m.Key);

    public static bool IsPropertyHeaderFound(this ClassMap classMap)
        => classMap.PropertyMapCollection.Any(m => m.Value.HeaderRowIndex >= WorksheetParserConstant.MIN_INDEX_EPPLUS);

    public static bool IsAllPropertiesIndexValid(this ClassMap classMap)
        => classMap.PropertyMapCollection.Values.All(m => m.ColumnIndex >= WorksheetParserConstant.MIN_INDEX_EPPLUS);
}
