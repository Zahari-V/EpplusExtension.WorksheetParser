using EpplusExtension.WorksheetParser.Constants;
using System;

namespace EpplusExtension.WorksheetParser.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class WorksheetParserColumnAttribute : Attribute
{
    private string _name;
    private int _index = WorksheetParserConstant.UNDEFINED_INDEX;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{nameof(Name)} cannot be null or white space!");

            _name = value;
        }
    }

    public int Index
    {
        get => _index;
        set
        {
            if (value < WorksheetParserConstant.MIN_INDEX_EPPLUS)
                throw new ArgumentException($"{nameof(Index)} cannot be zero or negative number!");

            _index = value;
        }
    }

    public bool Ignore { get; set; }
}
