using System;

namespace EpplusExtension.WorksheetParser.Parser;

public class WorksheetParser<T> where T : class, new()
{
    public WorksheetParser(WorksheetParserExecutionContext executionContext)
    {
        ExecutionContext = executionContext ?? throw new ArgumentNullException($"{nameof(executionContext)} cannot be null!");
    }

    public WorksheetParserExecutionContext ExecutionContext { get; private init; }

    public bool CanParse => ExecutionContext.CanMove;

    public T ParseRow()
    {
        if (!CanParse) throw new InvalidOperationException($"{nameof(CanParse)} property is false!");

        ExecutionContext.MoveToNextRow();

        var obj = new T();

        foreach (var propertyMap in ExecutionContext.ClassMap.PropertyMapCollection.Values)
        {
            string cellValue = ExecutionContext.GetCellValue(propertyMap.ColumnIndex);

            try
            {
                propertyMap.SetValue(obj, cellValue);
            }
            catch (Exception ex)
            {
                if (!ExecutionContext.Config.SuppressConvertionException) throw;
            }
        }

        return obj;
    }
}
