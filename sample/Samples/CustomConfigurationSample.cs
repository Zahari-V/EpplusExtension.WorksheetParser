using EpplusExtension.WorksheetParser.Parser;
using EpplusExtension.WorksheetParser.Sample.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EpplusExtension.WorksheetParser.Sample.Samples;

public static class CustomConfigurationSample
{
    public static void Execute()
    {
        var fileStream = File.OpenRead("..\\..\\..\\Files\\CoctailFile-CustomConfiguration.xlsx");

        using ExcelPackage package = new ExcelPackage(fileStream);
        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault() ?? throw new ArgumentNullException("Worksheet not found!");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var records = new List<CocktailModel>();

        var worksheetParserConfig = new WorksheetParserConfiguration()
        {
            //UseHeaders = false, //Use when all properties of the class have a WorksheetParserColumnAttribute with an Index property.
            StartSearchHeaderRowIndex = 1,
            EndSearchHeaderRowIndex = 10,
            //SuppressConvertionException = false, //Use when ParseRow needs to throw an exception on failed conversion, instead of using the default property value.
        };

        var worksheetParser = worksheet.GetParser<CocktailModel>(worksheetParserConfig);

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
