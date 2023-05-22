using System;

namespace Domain.Entities
{
    public class RecipeIngredient
    {
        public virtual int RecipeId { get; set; }
        
        public virtual int IngredientId { get; set; }

        public virtual int AmountTypeId { get; set; }
        
        public virtual int Amount { get; set; }

        public virtual DateTime DateCreate { get; set; }

        public virtual DateTime? DateChange { get; set; }

        public virtual Recipe Recipe { get; set; }
        
        public virtual Ingredient Ingredient { get; set; }
        
        public virtual AmountType AmountType { get; set; }

        #region Methods
        public override bool Equals(object obj)
        {
            var o = obj as RecipeIngredient;
            return RecipeId == o?.RecipeId && IngredientId == o.IngredientId;
        }

        public override int GetHashCode()
        {
            return RecipeId.GetHashCode() + IngredientId.GetHashCode();
        }
        #endregion
    }
}
