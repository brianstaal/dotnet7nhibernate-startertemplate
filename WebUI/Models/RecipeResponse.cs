namespace WebUI.Models;

public sealed class RecipeResponse
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int CategoryId { get; init; }

    public string CategoryName { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int Votes { get; init; }

    public DateTime DateCreate { get; init; }

    public DateTime? DateChange { get; init; }

    public IReadOnlyCollection<RecipeIngredientResponse> Ingredients { get; init; } = Array.Empty<RecipeIngredientResponse>();
}

public sealed class RecipeIngredientResponse
{
    public int IngredientId { get; init; }

    public string IngredientName { get; init; } = string.Empty;

    public int AmountTypeId { get; init; }

    public string AmountTypeName { get; init; } = string.Empty;

    public int Amount { get; init; }
}
