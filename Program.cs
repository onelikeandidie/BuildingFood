using System;
using Microsoft.Xna.Framework;
using BuildingFood;

namespace BuildingFood
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            using (BuildingFoodGame game = new BuildingFoodGame())
            {
                game.Run();
            }
        }
    }
}
