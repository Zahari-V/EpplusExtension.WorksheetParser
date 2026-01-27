using System;

namespace EpplusExtension.WorksheetParser.Parser;

public delegate object CellValueHandler(string cellValue);

public static class WorksheetParserConverter
{
    public static CellValueHandler GetCellValueHandler(Type type)
    {
        if (type == null) throw new ArgumentNullException($"{nameof(type)} cannot be null!");

        var nullableUnderlyingType = Nullable.GetUnderlyingType(type);

        if (!type.IsAssignableTo(typeof(IConvertible)) && !(nullableUnderlyingType?.IsAssignableTo(typeof(IConvertible)) == true))
            throw new NotSupportedException($"Parser converter doesn't support {type.Name}.\n" +
                $"Supported types are {nameof(IConvertible)} and {nameof(Nullable)}<{nameof(IConvertible)}> property types.");

        if (nullableUnderlyingType != null && nullableUnderlyingType.IsEnum)
        {
            return (cellValue) =>
            {
                if (string.IsNullOrWhiteSpace(cellValue))
                    return null;

                return Enum.Parse(nullableUnderlyingType, cellValue);
            };
        }
        
        if (nullableUnderlyingType != null)
        {
            return (cellValue) =>
            {
                if (string.IsNullOrWhiteSpace(cellValue))
                    return null;

                return Convert.ChangeType(cellValue, nullableUnderlyingType);
            };
        }
        
        if (type.IsEnum)
        {
            return (cellValue) => Enum.Parse(type, cellValue);
        }
        
        return (cellValue) => Convert.ChangeType(cellValue, type);
    }
}
