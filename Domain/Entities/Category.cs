using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Category
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime DateCreate { get; set; }

        public virtual DateTime? DateChange { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }

        #region Methods
        public override bool Equals(object obj)
        {
            var o = obj as Category;
            return Id == o?.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion

    }
}
