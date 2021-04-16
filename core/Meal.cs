using System;
using System.Collections.Generic;

namespace BuildingFood.core
{
    public struct Meal
    {
        public string Name { get; }
        public Recipe Ingredients { get; set; }
        public byte Progress { get; set; }
        public byte IngredientAmount { get; }
        public string TextureName { get; }

        public Meal(List<Ingredient> ingredients, string name, string textureName)
        {
            Name = name;
            Ingredients = new Recipe();
            for (var index = 0; index < ingredients.Count; index++)
            {
                Ingredient ingredient = (Ingredient) ingredients[index];
                Ingredients = Ingredients.Add(ingredient);
            }

            TextureName = textureName;
            IngredientAmount = (byte) ingredients.Count;
            Progress = 0;
        }
    }
}