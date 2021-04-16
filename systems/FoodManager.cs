using System.Collections.Generic;
using BuildingFood.core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingFood.systems
{
    public class FoodManager : ISystem
    {
        public BuildingFoodGame ParentGame { get; set; }
        
        public List<Food> FoodTextures;

        public Point PlatePosition;
        public Point PlateScale;

        public FoodManager(BuildingFoodGame parentGame)
        {
            ParentGame = parentGame;
            FoodTextures = new List<Food>();
            PlatePosition = new Point();
            PlateScale = new Point();
        }

        public void LoadContent()
        {
            Texture2D plateTexture = ParentGame.Texture["plate"];
            PlatePosition = new Point(ParentGame.ScreenWidth / 2 - plateTexture.Width, ParentGame.ScreenHeight / 2 - plateTexture.Height);
            PlateScale = new Point(plateTexture.Width * 2, plateTexture.Height * 2);
        }

        public void AddIngredient(Ingredient ingredient)
        {
            Texture2D foodTexture = ParentGame.Texture[ingredient.TextureName];
            Point foodPosition = new Point(0, 0);
            Point foodSize = new Point(foodTexture.Width * 2, foodTexture.Height * 2);
            Food food = new Food(ingredient.TextureName, foodPosition, foodSize);
            food.TexturePosition = CalculateFoodPosition(food);
            ParentGame.ParticleSystem.AddRandomParticle(new Vector2(ParentGame.ScreenWidth / 2, ParentGame.ScreenHeight / 2), ParticleTypes.Tools, 10);
            ParentGame.SoundManager.PlayRandomSound(SoundTypes.Tools);
            FoodTextures.Add(food);
        }

        private Point CalculateFoodPosition(Food food)
        {
            int X = PlatePosition.X + PlateScale.X / 2 - food.TextureSize.X / 2;
            int Y = PlatePosition.Y + PlateScale.Y / 2 - food.TextureSize.Y / 2 - 20 * FoodTextures.Count;
            Point point = new Point(X, Y);
            return point;
        }

        private void SetPlatePosition(Point point)
        {
            Texture2D plateTexture = ParentGame.Texture["plate"];
            PlatePosition.X = point.X + plateTexture.Width;
            PlatePosition.Y = point.Y + plateTexture.Height;

            for (var index = 0; index < FoodTextures.Count; index++)
            {
                Food food = FoodTextures[index];
                food.TexturePosition = CalculateFoodPosition(food);
                FoodTextures[index] = food;
            }
        }
        
        private void SetPlatePosition(Point point, Point size)
        {
            
        }

        public void ResetFood()
        {
            FoodTextures = new List<Food>();
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch batch)
        {
            Texture2D plateTexture = ParentGame.Texture["plate"];
            batch.Draw(plateTexture, new Rectangle(PlatePosition.X, PlatePosition.Y, PlateScale.X, PlateScale.Y), Color.White);

            if (ParentGame.GameState.Orders.Empty)
            {
                return;
            }
            
            Order currentOrder = ParentGame.GameState.Orders.First;

            for (var index = 0; index < FoodTextures.Count; index++)
            {
                Food food = FoodTextures[index];
                
                Texture2D iTexture = ParentGame.Texture[food.TextureName];
                batch.Draw(iTexture, new Rectangle(food.TexturePosition.X, food.TexturePosition.Y, food.TextureSize.X, food.TextureSize.Y), Color.White);
            }
        }
    }

    public struct Food
    {
        public string TextureName;
        public Point TexturePosition;
        public Point TextureSize;

        public Food(string textureName, Point texturePosition, Point textureSize)
        {
            TextureName = textureName;
            TexturePosition = texturePosition;
            TextureSize = textureSize;
        }
    }
}