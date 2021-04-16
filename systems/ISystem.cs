namespace BuildingFood.systems
{
    public interface ISystem
    {
        public BuildingFoodGame ParentGame { get; set; }
    }
}