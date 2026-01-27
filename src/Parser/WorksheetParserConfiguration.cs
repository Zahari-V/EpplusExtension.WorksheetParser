using EpplusExtension.WorksheetParser.Constants;
using System;

namespace EpplusExtension.WorksheetParser.Parser;

public class WorksheetParserConfiguration
{
    private int _startSearchHeaderRowIndex = WorksheetParserConstant.DEFAULT_START_SEARCH_HEADER_ROW_INDEX;
    private int _endSearchHeaderRowIndex = WorksheetParserConstant.DEFAULT_END_SEARCH_HEADER_ROW_INDEX;

    #region Header configuration

    /// <summary>
    /// If DisableHeaders is set to true, class properties must be defined with WorksheetParserColumnAttribute(Index = value).
    /// </summary>
    public bool UseHeaders { get; set; } = true;

    /// <summary>
    /// Defines the index of the start row of the range of cells to search for the header.
    /// Used in conjunction with EndSearchHeaderRowIndex.
    /// </summary>
    public int StartSearchHeaderRowIndex
    {
        get => _startSearchHeaderRowIndex;
        set
        {
            if (value < WorksheetParserConstant.MIN_INDEX_EPPLUS)
                throw new ArgumentException($"{nameof(StartSearchHeaderRowIndex)} cannot be zero or negative number!");

            _startSearchHeaderRowIndex = value;
        }
    }

    /// <summary>
    /// Defines the index of the last row of the range of cells to search for the header.
    /// Used in conjunction with StartSearchHeaderRowIndex.
    /// </summary>
    public int EndSearchHeaderRowIndex
    {
        get => _endSearchHeaderRowIndex;
        set
        {
            if (value < WorksheetParserConstant.MIN_INDEX_EPPLUS)
                throw new ArgumentException($"{nameof(EndSearchHeaderRowIndex)} cannot be zero or negative number!");

            _endSearchHeaderRowIndex = value;
        }
    }

    #endregion

    /// <summary>
    /// If set to true, in case of of conversion exception, the value of the property will be the default value without throwing an exception.
    /// If false, an exception will be thrown in case of conversion exception.
    /// Default is true.
    /// </summary>
    public bool SuppressConvertionException { get; set; } = true;
}
