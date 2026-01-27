using System;

namespace EpplusExtension.WorksheetParser.Exceptions;

public class WorksheetParserException : Exception
{
    public WorksheetParserException(string message) : base(message, null)
    { }

    public WorksheetParserException(string message, Exception innerException) : base(message, innerException)
    { }
}
