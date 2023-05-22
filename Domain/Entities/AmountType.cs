using System;

namespace Domain.Entities
{
    public class AmountType
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime DateCreate { get; set; }

        public virtual DateTime? DateChange { get; set; }

        #region Methods
        public override bool Equals(object obj)
        {
            var o = obj as AmountType;
            return Id == o?.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}
