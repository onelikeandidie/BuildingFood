using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingFood.systems
{
    public class BackgroudManager : ISystem
    {
        public BuildingFoodGame ParentGame { get; set; }

        public string BackgroundName;

        public BackgroudManager(BuildingFoodGame parentGame)
        {
            ParentGame = parentGame;
            BackgroundName = "";
        }
        
        public void Update(GameTime gameTime)
        {
            
        }
        
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(ParentGame.Texture[BackgroundName], new Rectangle(0,0, ParentGame.ScreenWidth, ParentGame.ScreenHeight), Color.Gray);
        }

        public void SetBackground(string backRestaurantPs)
        {
            BackgroundName = backRestaurantPs;
        }
    }
}