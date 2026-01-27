using EpplusExtension.WorksheetParser.Attributes;
using EpplusExtension.WorksheetParser.Sample.Enums;
using FluentValidation;

namespace EpplusExtension.WorksheetParser.Sample.Models;

public class CocktailModel
{
    //[WorksheetParserColumn(Index = 1)]
    public long Id { get; set; }

    //[WorksheetParserColumn(Index = 2)]
    public string Name { get; set; }

    //[WorksheetParserColumn(Index = 3)]
    public int Alcohol { get; set; }

    //[WorksheetParserColumn(Index = 4)]
    public Version? Version { get; set; }

    //[WorksheetParserColumn(Index = 5)]
    public string Description { get; set; }
}

public class CocktailValidator : AbstractValidator<CocktailModel>
{
    public CocktailValidator()
    {
        RuleFor(cocktail => cocktail.Id).NotNull();
        RuleFor(cocktail => cocktail.Version).NotNull().IsInEnum().WithMessage("Version {PropertyValue} is not valid enum value!");
        RuleFor(cocktail => cocktail.Alcohol).NotNull().GreaterThan(5).WithMessage("Coctail alcohol {PropertyValue} is equal or less than {ComparisonValue}!");
    }
}
