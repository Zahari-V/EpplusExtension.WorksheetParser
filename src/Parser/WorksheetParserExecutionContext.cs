using EpplusExtension.WorksheetParser.Attributes;
using EpplusExtension.WorksheetParser.Constants;
using EpplusExtension.WorksheetParser.Exceptions;
using EpplusExtension.WorksheetParser.Parser.Maps;
using OfficeOpenXml;
using System;
using System.Linq;
using System.Reflection;

namespace EpplusExtension.WorksheetParser.Parser;

public class WorksheetParserExecutionContext
{
    private readonly ExcelWorksheet _worksheet;

    public WorksheetParserExecutionContext(ExcelWorksheet worksheet,
        WorksheetParserConfiguration config,
        Type classMapType)
    {
        _worksheet = worksheet ?? throw new ArgumentNullException($"{nameof(worksheet)} cannot be null!");
        Config = config ?? throw new ArgumentNullException($"{nameof(Config)} cannot be null!");
        ClassMap = new ClassMap(classMapType);

        Build();
        ValidateBuild();
    }

    internal WorksheetParserConfiguration Config { get; private init; }

    internal ClassMap ClassMap { get; private init; }

    public int HeaderRowIndex { get; private set; } = WorksheetParserConstant.UNDEFINED_INDEX;

    public bool IsHeaderRowIndexUndefined => HeaderRowIndex == WorksheetParserConstant.UNDEFINED_INDEX;

    public int CurrentRowIndex { get; private set; } = WorksheetParserConstant.UNDEFINED_INDEX;

    /// <summary>
    /// Indicate wheather can move to next row.
    /// </summary>
    public bool CanMove => CurrentRowIndex < _worksheet.Dimension.End.Row;

    /// <summary>
    /// Increment CurrentRowIndex.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    internal void MoveToNextRow()
    {
        if (!CanMove) throw new InvalidOperationException($"{nameof(CanMove)} property is false!");

        CurrentRowIndex++;
    }

    /// <summary>
    /// Get cell value from worksheet on current row index and passed column index.
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public string GetCellValue(int columnIndex)
    {
        return _worksheet.GetValue<string>(CurrentRowIndex, columnIndex)?.Trim();
    }

    /// <summary>
    /// Build an execution context, preparing it for use.
    /// </summary>
    private void Build()
    {
        foreach (var propertyMap in ClassMap.PropertyMapCollection.Values)
        {
            propertyMap.ColumnIndex = propertyMap.PropertyInfo.GetCustomAttribute<WorksheetParserColumnAttribute>(false)?.Index ?? WorksheetParserConstant.UNDEFINED_INDEX;
        }

        if (!Config.UseHeaders)
        {
            CurrentRowIndex = WorksheetParserConstant.MIN_INDEX_EPPLUS - 1;
            return;
        }

        int rowCount = _worksheet.Dimension.End.Row;
        int columnCount = _worksheet.Dimension.End.Column;

        for (int rowIndex = Config.StartSearchHeaderRowIndex; rowIndex <= Config.EndSearchHeaderRowIndex && rowIndex <= rowCount; rowIndex++)
        {
            for (int columnIndex = WorksheetParserConstant.MIN_INDEX_EPPLUS; columnIndex <= columnCount; columnIndex++)
            {
                string cellValue = _worksheet.GetValue<string>(rowIndex, columnIndex)?.Trim();

                if (!string.IsNullOrWhiteSpace(cellValue)
                    && ClassMap.PropertyMapCollection.ContainsKey(cellValue)
                    && ClassMap.PropertyMapCollection[cellValue].IsIndexUndefined)
                {
                    ClassMap.PropertyMapCollection[cellValue].ColumnIndex = columnIndex;
                    ClassMap.PropertyMapCollection[cellValue].HeaderRowIndex = rowIndex;
                }
            }

            if (ClassMap.IsPropertyHeaderFound())
            {
                HeaderRowIndex = rowIndex;
                CurrentRowIndex = rowIndex;
                break;
            }
        }
    }

    /// <summary>
    /// Validates build of the execution context.
    /// If build is invalid throw exception.
    /// </summary>
    /// <exception cref="WorksheetParserException"></exception>
    private void ValidateBuild()
    {
        if (!Config.UseHeaders && CurrentRowIndex != WorksheetParserConstant.MIN_INDEX_EPPLUS - 1)
            throw new WorksheetParserException("Worksheet parser execution context is invalid!");

        if (Config.UseHeaders && IsHeaderRowIndexUndefined
            && ClassMap.IsAllPropertiesIndexValid())
            throw new WorksheetParserException("Set UseHeaders to false in order to predefine all class property indexes!");

        if (Config.UseHeaders && IsHeaderRowIndexUndefined)
            throw new WorksheetParserException("Headers not found!");

        if (Config.UseHeaders && CurrentRowIndex != HeaderRowIndex)
            throw new WorksheetParserException("Worksheet parser execution context is invalid!");

        var undefinedColumns = ClassMap.GetPropertiesKeyWithUndefinedIndex();

        if (undefinedColumns.Count() > 0)
            throw new WorksheetParserException($"Undefined columns!\n" +
                $"Undefined columns {string.Join(", ", undefinedColumns)}.");
    }
}
