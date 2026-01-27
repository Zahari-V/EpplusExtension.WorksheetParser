using EpplusExtension.WorksheetParser.Sample.Samples;
using OfficeOpenXml;

ExcelPackage.License.SetNonCommercialPersonal("My Name"); //This will also set the Author property to the name provided in the argument.

// ========================== Epplus custom extension base example ==========================
//BaseSample.Execute();

// ========================== Epplus custom extension parser configuration example ==========================
//CustomConfigurationSample.Execute();

// ========================== Epplus custom extension with fluent validation example ==========================
//ValidationSample.Execute();

// ========================== Epplus custom extension bulk example (800K rows) ==========================
BulkSample.Execute();

// ========================== Epplus custom extension broken file example ==========================
//BrokenFileSample.Execute();
