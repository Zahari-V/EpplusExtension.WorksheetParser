using EpplusExtension.WorksheetParser.Sample.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EpplusExtension.WorksheetParser.Sample.Samples;

public static class BulkSample
{
    public static void Execute()
    {
        var fileStream = File.OpenRead("..\\..\\..\\Files\\CoctailFile-Bulk.xlsx");

        using ExcelPackage package = new ExcelPackage(fileStream);
        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault() ?? throw new ArgumentNullException("Worksheet not found!");
        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var worksheetParser = worksheet.GetParser<CocktailModel>();

        var records = new List<CocktailModel>();

        while (worksheetParser.CanParse)
        {
            var record = worksheetParser.ParseRow();
            records.Add(record);
        }

        stopwatch.Stop();

        Console.WriteLine();
        Console.WriteLine($"------------ Count: {records.Count} ------------");
        Console.WriteLine($"------------ {nameof(stopwatch)} Elapsed: {stopwatch.Elapsed} ------------");
        Console.WriteLine($"------------ {nameof(StopwatchMeasurement.PropertySetValueStopwatch)} Elapsed: {StopwatchMeasurement.PropertySetValueStopwatch.Elapsed} ------------");
        Console.WriteLine();
    }
}
