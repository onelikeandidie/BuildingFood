using System;

namespace BuildingFood.core
{
    public readonly struct Ingredient
    {
        public string TextureName { get; }
        public string Name { get; }
        public Combination Combo { get; }
        public byte Progress { get; }

        public Ingredient(string textureName, string name, Combination combo)
        {
            this.TextureName = textureName;
            this.Name = name;
            this.Combo = combo;
            this.Progress = 0;
        }
    }
}