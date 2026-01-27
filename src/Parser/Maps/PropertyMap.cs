using EpplusExtension.WorksheetParser.Constants;
using System;
using System.Reflection;

namespace EpplusExtension.WorksheetParser.Parser.Maps;

public class PropertyMap
{
    public PropertyMap(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo ?? throw new ArgumentNullException($"{nameof(propertyInfo)} cannot be null!");
        CellValueHandler = WorksheetParserConverter.GetCellValueHandler(propertyInfo.PropertyType);
    }

    public PropertyInfo PropertyInfo { get; private init; }

    public CellValueHandler CellValueHandler { get; private init; }

    public int ColumnIndex { get; set; } = WorksheetParserConstant.UNDEFINED_INDEX;

    public int HeaderRowIndex { get; set; } = WorksheetParserConstant.UNDEFINED_INDEX;

    public bool IsIndexUndefined => ColumnIndex == WorksheetParserConstant.UNDEFINED_INDEX;

    public void SetValue(object obj, string cellValue)
    {
        object convertedCellValue = CellValueHandler(cellValue);
        PropertyInfo.SetValue(obj, convertedCellValue);
    }
}
