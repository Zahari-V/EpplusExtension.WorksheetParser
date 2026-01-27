using EpplusExtension.WorksheetParser.Sample.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EpplusExtension.WorksheetParser.Sample.Samples;

public static class BaseSample
{
    public static void Execute()
    {
        var fileStream = File.OpenRead("..\\..\\..\\Files\\CoctailFile-Base.xlsx");

        using ExcelPackage package = new ExcelPackage(fileStream);
        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault() ?? throw new ArgumentNullException("Worksheet not found!");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var records = new List<CocktailModel>();
        var worksheetParser = worksheet.GetParser<CocktailModel>();

        while (worksheetParser.CanParse)
        {
            var record = worksheetParser.ParseRow();
            records.Add(record);
        }

        stopwatch.Stop();

        Console.WriteLine();
        Console.WriteLine($"------------ Count: {records.Count} ------------");
        Console.WriteLine($"------------ Custom Elapsed: {stopwatch.Elapsed} ------------");
        Console.WriteLine();

        var index = 0;

        foreach (var item in records.Take(10))
        {
            Console.WriteLine($"{nameof(CocktailModel)}[{index}].{nameof(CocktailModel.Id)} = {item.Id}");
            Console.WriteLine($"{nameof(CocktailModel)}[{index}].{nameof(CocktailModel.Name)} = {item.Name}");
            Console.WriteLine($"{nameof(CocktailModel)}[{index}].{nameof(CocktailModel.Alcohol)} = {item.Alcohol}");
            Console.WriteLine($"{nameof(CocktailModel)}[{index}].{nameof(CocktailModel.Version)} = {item.Version}");
            Console.WriteLine($"{nameof(CocktailModel)}[{index}].{nameof(CocktailModel.Description)} = {item.Description}");
            Console.WriteLine();
            index++;
        }
    }
}
