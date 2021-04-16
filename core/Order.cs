namespace BuildingFood.core
{
    public class Order
    {
        public int OrderNumber;
        public int Price;
        public int Time;
        public float RemainingTime;
        public PriceSlide RemainingPrice;
        public Meal Meal;

        public byte IngredientProgress;
        public byte MealProgress;

        public Order()
        {
            IngredientProgress = 0;
            MealProgress = 0;
        }

        public void IngredientContinue()
        {
            IngredientProgress++;
            if (IngredientIsDone)
            {
                IngredientDone();
            }
        }

        public void IngredientFailed()
        {
            IngredientProgress = 0;
        }

        public void IngredientDone()
        {
            MealProgress++;
            IngredientProgress = 0;
        }

        public bool IngredientIsDone => IngredientProgress >= 4;

        public bool MealIsDone => MealProgress >= Meal.IngredientAmount;
    }

    public enum PriceSlide
    {
        Low, Medium, Max
    }
}