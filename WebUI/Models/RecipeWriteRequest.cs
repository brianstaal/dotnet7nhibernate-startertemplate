using System.ComponentModel.DataAnnotations;

namespace WebUI.Models;

public sealed class RecipeWriteRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CategoryId { get; init; }

    public string Description { get; init; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Votes { get; init; }

    public IReadOnlyCollection<RecipeIngredientWriteRequest> Ingredients { get; init; } = Array.Empty<RecipeIngredientWriteRequest>();
}

public sealed class RecipeIngredientWriteRequest
{
    [Range(1, int.MaxValue)]
    public int IngredientId { get; init; }

    [Range(1, int.MaxValue)]
    public int AmountTypeId { get; init; }

    [Range(1, int.MaxValue)]
    public int Amount { get; init; }
}
