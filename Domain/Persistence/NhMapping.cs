using Domain.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Persistence
{
    public class NhMapping
    {
        public class CategoryMap : ClassMapping<Category>
        {
            public CategoryMap()
            {
                Table("Category");
                Id(x => x.Id, map => map.Generator(Generators.Identity));
                Property(x => x.Name, map => map.Length(50));
                Property(x => x.DateCreate, map => map.Generated(PropertyGeneration.Insert));
                Property(x => x.DateChange, map => map.NotNullable(false));

                Set(x => x.Recipes, set =>
                {
                    set.Key(k =>
                    {
                        k.Column("CategoryId");
                    });
                    set.Inverse(true);
                    set.Cascade(Cascade.DeleteOrphans);
                }, action => action.OneToMany());
                
            }
        }

        public class RecipeMap : ClassMapping<Recipe>
        {
            public RecipeMap()
            {
                Table("Recipe");
                Id(x => x.Id, map => map.Generator(Generators.Identity));
                Property(x => x.CategoryId, map => map.NotNullable(false));
                Property(x => x.Name, map => map.Length(50));
                Property(x => x.Description, map => map.Length(4001)); // nvarchar(max)
                Property(x => x.DateCreate, map => map.Generated(PropertyGeneration.Insert));
                Property(x => x.DateChange, map => map.NotNullable(false));

                ManyToOne(r => r.Category, r =>
                {
                    r.Column("CategoryId");
                    r.Update(false);
                    r.Insert(false);
                });

                Set(x => x.RecipeIngredients, set =>
                {
                    set.Key(k =>
                    {
                        k.Column("RecipeId");
                    });
                    set.Inverse(true);
                    set.Cascade(Cascade.DeleteOrphans);
                }, action => action.OneToMany());

            }
        }

        public class IngredientMap : ClassMapping<Ingredient>
        {
            public IngredientMap()
            {
                Table("Ingredient");
                Id(x => x.Id, map => map.Generator(Generators.Identity));
                Property(x => x.Name, map => map.Length(50));
                Property(x => x.DateCreate, map => map.Generated(PropertyGeneration.Insert));
                Property(x => x.DateChange, map => map.NotNullable(false));

                Set(x => x.RecipeIngredients, set =>
                {
                    set.Key(k =>
                    {
                        k.Column("IngredientId");
                    });
                    set.Inverse(true);
                    set.Cascade(Cascade.DeleteOrphans);
                }, action => action.OneToMany());
            }
        }

        public class AmountTypeMap : ClassMapping<AmountType>
        {
            public AmountTypeMap()
            {
                Table("AmountType");
                Id(x => x.Id, map => map.Generator(Generators.Identity));
                Property(x => x.Name, map => map.Length(50));
                Property(x => x.DateCreate, map => map.Generated(PropertyGeneration.Insert));
                Property(x => x.DateChange, map => map.NotNullable(false));

                // Relation to RecipeIngredient ignored - will newer be used!
            }
        }

        public class RecipeIngredientMap : ClassMapping<RecipeIngredient>
        {
            public RecipeIngredientMap()
            {
                Table("RecipeIngredient");
                ComposedId(map =>
                {
                    map.Property(x => x.RecipeId);
                    map.Property(x => x.IngredientId);
                });

                Property(x => x.AmountTypeId, map => map.NotNullable(true));
                Property(x => x.Amount);
                Property(x => x.DateCreate, map => map.Generated(PropertyGeneration.Insert));
                Property(x => x.DateChange, map => map.NotNullable(false));

                ManyToOne(r => r.Recipe, r =>
                {
                    r.Column("RecipeId");
                    r.Update(false);
                    r.Insert(false);
                });

                ManyToOne(r => r.Ingredient, r =>
                {
                    r.Column("IngredientId");
                    r.Update(false);
                    r.Insert(false);
                });
            }
        }


    }
}
