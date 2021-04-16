using System;

namespace BuildingFood.core
{
    public struct Recipe
    {
        public Ingredient[] Items { get; set; }
        public int Count { get; set; }
        
        public Recipe(Ingredient[] items)
        {
            Items = items;
            Count = items.Length;
        }

        public Recipe Add(Ingredient ingredient)
        {
            Ingredient[] ingredients = Items;
            Array.Resize(ref ingredients, Count + 1);
            ingredients[Count] = ingredient;
            Items = ingredients;
            Count++;
            return this;
        }

        public Ingredient this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }
    }
}