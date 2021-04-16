using System;

namespace BuildingFood.systems
{
    public class SoundManager : ISystem
    {
        public SoundManager(BuildingFoodGame parentGame)
        {
            ParentGame = parentGame;
        }

        public BuildingFoodGame ParentGame { get; set; }

        public void PlayRandomSound(SoundTypes type)
        {
            var random = new Random();
            int index;
            switch (type)
            {
                case SoundTypes.Tools:
                    index = random.Next(1, 3);
                    ParentGame.Sound["knifeSlice" + index].CreateInstance().Play();
                    break;
                case SoundTypes.Money:
                    index = random.Next(0, 10);
                    ParentGame.Sound["handleCoins"].CreateInstance().Play();
                    break;
            }
        }
    }

    public enum SoundTypes
    {
        Tools, Fire, Money
    }
}