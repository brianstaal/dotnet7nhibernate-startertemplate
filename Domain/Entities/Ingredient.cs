using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Ingredient
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; } = string.Empty;

        public virtual DateTime DateCreate { get; set; }

        public virtual DateTime? DateChange { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        #region Methods
        public override bool Equals(object obj)
        {
            var o = obj as Ingredient;
            return Id == o?.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}
