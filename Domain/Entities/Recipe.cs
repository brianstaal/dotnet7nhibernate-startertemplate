using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Recipe
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual int CategoryId { get; set; }

        public virtual string Description { get; set; }

        public virtual int Votes { get; set; }

        public virtual DateTime DateCreate { get; set; }

        public virtual DateTime? DateChange { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        #region Methods
        public override bool Equals(object obj)
        {
            var o = obj as Recipe;
            return Id == o?.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}
