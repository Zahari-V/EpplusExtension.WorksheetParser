using EpplusExtension.WorksheetParser.Parser;
using OfficeOpenXml;

namespace EpplusExtension.WorksheetParser;

public static class WorksheetParserExtension
{
    public static WorksheetParser<T> GetParser<T>(this ExcelWorksheet worksheet) where T : class, new()
    {
        return worksheet.GetParser<T>(new WorksheetParserConfiguration());
    }

    public static WorksheetParser<T> GetParser<T>(this ExcelWorksheet worksheet, WorksheetParserConfiguration config) where T : class, new()
    {
        var executionContext = new WorksheetParserExecutionContext(worksheet, config, typeof(T));
        return new WorksheetParser<T>(executionContext);
    }
}