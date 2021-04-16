using BuildingFood.systems;

namespace BuildingFood.core
{
    public class AnimationMachine : ISystem
    {
        public BuildingFoodGame ParentGame { get; set; }
        
        public AnimationMachine(BuildingFoodGame parentGame)
        {
            
        }
    }
}