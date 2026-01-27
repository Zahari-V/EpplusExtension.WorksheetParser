using EpplusExtension.WorksheetParser.Sample.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EpplusExtension.WorksheetParser.Sample.Samples;

public static class ValidationSample
{
    public static void Execute()
    {
        var fileStream = File.OpenRead("..\\..\\..\\Files\\CoctailFile-Base.xlsx");

        using ExcelPackage package = new ExcelPackage(fileStream);
        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault() ?? throw new ArgumentNullException("Worksheet not found!");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var records = new List<CocktailModel>();
        var validator = new CocktailValidator();

        var worksheetParser = worksheet.GetParser<CocktailModel>();

        while (worksheetParser.CanParse)
        {
            var record = worksheetParser.ParseRow();

            var validationResult = validator.Validate(record);

            if (!validationResult.IsValid)
            {
                Console.Write($"Row {worksheetParser.ExecutionContext.CurrentRowIndex} is invalid! ");
                Console.WriteLine(string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage)));
            }
            else
            {
                records.Add(record);
            }
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
