using System;
using System.Collections.Generic;
using System.Linq;
using BuildingFood.core;
using Microsoft.Xna.Framework;

namespace BuildingFood.systems
{
    public class GameState : ISystem
    {
        public BuildingFoodGame ParentGame { get; set; }
        
        public double Momentum;
        public double TimeItTookForOneMeal;
        public bool Counting;
        public double TimeSinceLastOrder;
        public int Difficulty;
        public int OrdersCompleted;
        public int OrdersTotal;
        
        public OrderQueue Orders;

        public float Money;
        
        protected readonly int Seed;
        protected Random Random;

        public GameState(BuildingFoodGame parentGame)
        {
            ParentGame = parentGame;
            
            Momentum = 1;
            TimeItTookForOneMeal = 0;
            TimeSinceLastOrder = 0;
            Difficulty = 0;
            OrdersCompleted = 0;
            Money = 0;
            Counting = false;
            Orders = new OrderQueue();
            
            Random seedRandom = new Random();
            Seed = seedRandom.Next();
            Random = new Random(Seed);
        }
        
        public void Update(GameTime gameTime)
        {
            if (Counting)
            {
                TimeItTookForOneMeal += gameTime.ElapsedGameTime.TotalSeconds;
            }

            TimeSinceLastOrder += gameTime.ElapsedGameTime.TotalSeconds;

            List<Order> failedList = new List<Order>();
            for (var index = 0; index < Orders.Count; index++)
            {
                Order order = Orders.Items[index];
                order.RemainingTime = order.RemainingTime - (float) gameTime.ElapsedGameTime.TotalSeconds;
                Orders.Items[index] = order;
                if (order.RemainingTime <= 0)
                {
                    failedList.Add(order);
                }
            }

            foreach (Order failedOrder in failedList)
            {
                OrderFailed(failedOrder);
            }

            if (TimeSinceLastOrder > 10 - Difficulty)
            {
                if (Random.Next(0, 100) > 95)
                {
                    AddRandomOrder();
                };
            }

            for (var index = 0; index < Orders.Items.Length; index++)
            {
                Order order = Orders.Items[index];
                float percentageOfTimeLeft = 1 - order.RemainingTime / order.Time;
                order.RemainingPrice = PriceSlide.Max;
                if (percentageOfTimeLeft > .2 && percentageOfTimeLeft < .8)
                {
                    order.RemainingPrice = PriceSlide.Medium;
                }
                else if (percentageOfTimeLeft < .2)
                {
                    order.RemainingPrice = PriceSlide.Low;
                }
            }
        }

        public void AddRandomOrder()
        {
            OrdersTotal++;
            
            Random random = new Random();
            int nextMeal = random.Next(ParentGame.Meals.Count);
            Meal meal = ParentGame.Meals[nextMeal];
            int nextPrice = random.Next(meal.IngredientAmount * 3, meal.IngredientAmount * 5) + 10 + Difficulty;
            int nextTime = random.Next(meal.IngredientAmount * 3, meal.IngredientAmount * 5) + 10 - Difficulty;
            // Add new order
            Order order = new Order()
            {
                OrderNumber = OrdersTotal,
                Meal = meal,
                Price = nextPrice,
                Time = nextTime,
                RemainingTime = nextTime
            };
            Orders.Add(order);
            
            ParentGame.UIManager.AddOrder(order);
            
            TimeSinceLastOrder = 0;
        }

        public void KeyPressed(Key keyPressed)
        {
            if (Orders.Empty)
            {
                return;
            }
#if DEBUG
            if (keyPressed == Key.Period)
            {
                AddRandomOrder();
            }
            if (keyPressed == Key.M)
            {
                ParentGame.ParticleSystem.AddRandomParticle(new Vector2(ParentGame.ScreenWidth / 2, ParentGame.ScreenHeight / 2), ParticleTypes.Tools, 99);
            }
#endif
            Order currentOrder = Orders.First;

            if (currentOrder.MealIsDone)
            {
                return;
            }

            Counting = true;

            Ingredient currentIngredient = (Ingredient) currentOrder.Meal.Ingredients[currentOrder.MealProgress];
            
            if (currentIngredient.Combo[currentOrder.IngredientProgress] == keyPressed)
            {
                if (currentOrder.IngredientProgress + 1 >= 4)
                {
                    ParentGame.FoodManager.AddIngredient(currentIngredient);
                }
                currentOrder.IngredientContinue();
            }
            else
            {
                DecreaseMomentum();
                currentOrder.IngredientFailed();
            }
#if DEBUG
            if (keyPressed == Key.Skip)
            {
                currentOrder.IngredientDone();
                foreach (Ingredient ingredient in currentOrder.Meal.Ingredients.Items)
                {
                    currentOrder.IngredientContinue();
                }
                ParentGame.FoodManager.AddIngredient(currentIngredient);
            }
#endif
            if (currentOrder.MealIsDone)
            {
                OrderDone();
            }
        }
        
        public void OrderFailed(Order order)
        {
            Momentum = 1;
            AdjustDifficulty();
            
            Counting = false;
            TimeSinceLastOrder += 2 * Momentum;
            
            Money -= Orders.First.Price / 2;

            ParentGame.FoodManager.ResetFood();
            ParentGame.UIManager.RemoveOrder(order);
            Orders.Remove(order);
        }
        
        public void OrderDone()
        {
            AdjustMomentum();
            AdjustDifficulty();
            
            Counting = false;
            TimeItTookForOneMeal = 0;
            TimeSinceLastOrder += 2 * Momentum;
            OrdersCompleted++;

            int calculatedPay = CalculatePay(Orders.First);
            
            Money += calculatedPay;

            ParentGame.FoodManager.ResetFood();
            ParentGame.UIManager.RemoveOrder(Orders.First);
            ParentGame.ParticleSystem.AddRandomParticle(new Vector2(ParentGame.ScreenWidth - 100, 100), ParticleTypes.Money, 10);
            ParentGame.SoundManager.PlayRandomSound(SoundTypes.Money);
            Orders.Next();
        }

        public Order GetOrderByID(int orderNumber)
        {
            var order = Orders.Items.First(order => order.OrderNumber == orderNumber);
            if (order == null)
            {
                throw new Exception($"Order not found with ID: {orderNumber}");
            }
            return order;
        }

        public int CalculatePay(Order order)
        {
            float percentageOfTimeLeft = 1 - order.RemainingTime / order.Time;
            int calculatedPay = (int) Math.Floor(order.Price * Momentum);
            if (percentageOfTimeLeft > .2 && percentageOfTimeLeft < .8)
            {
                calculatedPay = (int) Math.Floor((double) ((calculatedPay / 3) * 2));
            } else if (percentageOfTimeLeft < .2)
            {
                calculatedPay = (int) Math.Floor((double) (calculatedPay / 3));
            }
            return calculatedPay;
        }

        public void AdjustMomentum()
        {
            Momentum = (int) Math.Clamp(Momentum + Math.Floor((Orders.First.Meal.IngredientAmount) / (TimeItTookForOneMeal * 5)) + 1, 1, 5);
        }

        private void DecreaseMomentum()
        {
            if (--Momentum <= 0)
            {
                Momentum = 1;
            }
        }

        public void AdjustDifficulty()
        {
            Difficulty = (int) Math.Floor((decimal) (OrdersCompleted / 10));
        }
    }
}