using System;
using System.Collections.Generic;
using System.Linq;
using BuildingFood.core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingFood.systems
{
    public class UIManager : ISystem
    {
        public BuildingFoodGame ParentGame { get; set; }

        public Point TicketListPosition;
        public List<Ticket> Tickets;

        public Point ArrowPosition;

        public Point ComboPosition;
        public Point ComboBasePosition;
        public Point ComboSize;
        public int CurrentCombo;

        public UIManager(BuildingFoodGame parentGame)
        {
            ParentGame = parentGame;

            TicketListPosition = new Point(ParentGame.ScreenWidth - 2, 2);
            Tickets = new List<Ticket>();

            ArrowPosition = new Point(ParentGame.ScreenWidth / 2, ParentGame.ScreenHeight - 50);
            
            ComboPosition = new Point(0, 0);
            ComboBasePosition = new Point(ParentGame.ScreenWidth - 300, 100);
            ComboSize = new Point(1,1);
            CurrentCombo = 0;
        }

        public void Update(GameTime gameTime)
        {
            int mmmm = (int) ParentGame.GameState.Momentum;
            if (CurrentCombo != mmmm)
            {
                SetCombo(mmmm);
            }

            if (!ParentGame.GameState.Orders.Empty)
            {
                for (var index = 0; index < Tickets.Count; index++)
                {
                    var ticket = Tickets[index];
                    Order order = ParentGame.GameState.GetOrderByID(ticket.OrderNumber);
                    ticket.Percentage = (order.RemainingTime / order.Time);
                    if (ticket.Percentage <= .80 && ticket.Percentage > .20)
                    {
                        ticket.BarColour = "yellow";
                    }
                    else if (ticket.Percentage <= .20)
                    {
                        ticket.BarColour = "red";
                    }
                    Tickets[index] = ticket;
                }
            }
        }

        private void SetCombo(int combo)
        {
            CurrentCombo = combo;
            Texture2D comboTexture = ParentGame.Texture["UI_combo" + CurrentCombo];
            ComboSize = new Point(comboTexture.Width, comboTexture.Height);
            ComboPosition = new Point(ComboBasePosition.X - comboTexture.Width / 2, ComboBasePosition.Y - comboTexture.Height / 2);
        }

        public void AddOrder(Order order)
        {
            Ticket ticket = new Ticket(order.OrderNumber);
            Tickets.Add(ticket);
            RefreshTickets();
        }
        
        public void RemoveOrder(Order order)
        {
            int success = Tickets.RemoveAll(ticket => ticket.OrderNumber == order.OrderNumber);
            if (success < 1)
            {
                throw new Exception("Order to remove not found");
            }
            if (success > 1)
            {
                throw new Exception("Too many tickets with the same order! Call Gordon Ramsay");
            }
            RefreshTickets();
        }

        private void RefreshTickets()
        {
            if (Tickets.Count >= 1)
            {
                // Set size of the first and second tickets to be bigger
                Ticket firstTicket = Tickets[0];
                Vector2 firstTicketSize = firstTicket.Size;
                firstTicketSize.X = (float) 1.2;
                firstTicketSize.Y = (float) 1.2;
                firstTicket.Size = firstTicketSize;
                firstTicket.ShowIngredients = true;
                Tickets[0] = firstTicket;
            }
            if (Tickets.Count >= 2)
            {
                Ticket secondTicket = Tickets[1];
                Vector2 secondTicketSize = secondTicket.Size;
                secondTicketSize.X = (float) 1.05;
                secondTicketSize.Y = (float) 1.05;
                secondTicket.Size = secondTicketSize;
                Tickets[1] = secondTicket;
            }
            // Anchor of the Ticket List is on the top right
            Texture2D ticketTexture = ParentGame.Texture["UI_ticket"];
            float yAddition = TicketListPosition.Y;
            for (var index = 0; index < Tickets.Count; index++)
            {
                Ticket ticket = Tickets[index];
                
                // Get the order to extract the ingredients
                var ingredients = ParentGame.GameState.GetOrderByID(ticket.OrderNumber).Meal.Ingredients.Items;
                
                float yPosition = yAddition;
                float xPosition = TicketListPosition.X - ticket.Size.X * ticketTexture.Width;
                yAddition = yAddition + (ticket.Size.Y * ticketTexture.Height);
                
                Point ticketPosition = ticket.Position;
                ticketPosition.X = (int) xPosition;
                ticketPosition.Y = (int) yPosition;
                ticket.Position = ticketPosition;
                
                Point numberPosition = ticket.NumberPosition;
                numberPosition.X = (int) (ticketPosition.X + ticket.Size.X * ticketTexture.Width - 20 * ticket.Size.X);
                numberPosition.Y = (int) (ticketPosition.Y + ticket.Size.Y * ticketTexture.Height - 20 * ticket.Size.Y);
                ticket.NumberPosition = numberPosition;
                
                Point barPosition = ticket.BarPosition;
                barPosition.X = (int) (ticketPosition.X + 20 * ticket.Size.X);
                barPosition.Y = (int) (ticketPosition.Y + 30 * ticket.Size.Y);
                ticket.BarPosition = barPosition;
                
                Point platePosition = ticket.PlatePosition;
                platePosition.X = (int) (ticketPosition.X + ticket.Size.X * ticketTexture.Width / 2);
                platePosition.Y = (int) (ticketPosition.Y + ticket.Size.Y * ticketTexture.Height / 2);
                ticket.PlatePosition = platePosition;
                
                Point ingredientBasePosition = new Point();
                ingredientBasePosition.X = (int) (ticketPosition.X + ticket.Size.X * ticketTexture.Height / 2);
                ingredientBasePosition.Y = (int) (ticketPosition.Y + ticket.Size.Y * ticketTexture.Height - 10 * ticket.Size.Y);

                Point[] ingredientPositions = new Point[ingredients.Length];
                float ingredientListWidth = 100 * ticket.Size.X;
                float ingredientListHeight = 30 * ticket.Size.Y;
                float spaceBetweenElements = ingredientListWidth / (ingredients.Length - 1);
                float xAddition = ingredientBasePosition.X;
                for (int jndex = 0; jndex < ingredients.Length; jndex++)
                {
                    ingredientPositions[jndex].X = (int) xAddition;
                    ingredientPositions[jndex].Y = (int) (ingredientBasePosition.Y - ingredientListHeight / 2);
                    xAddition += spaceBetweenElements;
                }
                
                ticket.IngredientPosition = ingredientPositions.ToList();
                ticket.IngredientListSize = new Vector2(ingredientListWidth, ingredientListHeight);
                
                Tickets[index] = ticket;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (!ParentGame.GameState.Orders.Empty && !ParentGame.GameState.Orders.First.MealIsDone)
            {
                DrawArrows(batch);
            }

            DrawCombo(batch);
            DrawTickets(batch);
        }

        private void DrawCombo(SpriteBatch batch)
        {
            Texture2D comboTexture = ParentGame.Texture["UI_combo" + CurrentCombo];
            int comboTextureX = (ComboPosition.X);
            int comboTextureY = (ComboPosition.Y);
            batch.Draw(comboTexture, new Rectangle(comboTextureX, comboTextureY, ComboSize.X, ComboSize.Y), Color.White);
        }

        private void DrawTickets(SpriteBatch batch)
        {
            //batch.DrawString(ParentGame.Font, $"Ticket Amount:{Tickets.Count}", new Vector2(300, 0), Color.Black);
            for (var index = 0; index < Tickets.Count; index++)
            {
                Ticket ticket = Tickets[index];
                Texture2D ticketTexture = ParentGame.Texture["UI_ticket"];
                batch.Draw(ticketTexture,
                    new Rectangle(
                        ticket.Position.X, 
                        ticket.Position.Y, 
                        (int) (ticket.Size.X * ticketTexture.Width), 
                        (int) (ticket.Size.Y * ticketTexture.Height)),
                    Color.White);
                batch.DrawString(ParentGame.Font, $"#{ticket.OrderNumber}",
                    new Vector2(ticket.Position.X + 20, ticket.Position.Y + (int) (ticket.Size.Y * ticketTexture.Height) - 20 * ticket.Size.Y), Color.DimGray, 0.0f, Vector2.Zero, ticket.Size.Y, SpriteEffects.None, 0.0f);
                Texture2D barTexture = ParentGame.Texture["UI_bar_" + ticket.BarColour];
                batch.Draw(barTexture, new Rectangle(
                    ticket.BarPosition.X, 
                    ticket.BarPosition.Y, 
                    (int) (ticket.Percentage * (145 * ticket.Size.X)), 
                    (int) (10 * ticket.Size.Y)),
                    Color.White);
                Order order = ParentGame.GameState.GetOrderByID(ticket.OrderNumber);
                Texture2D foodTexture = ParentGame.Texture[order.Meal.TextureName];
                int foodX = (int) (ticket.PlatePosition.X - (foodTexture.Width * ticket.Size.X / 2)) ;
                int foodY = (int) (ticket.PlatePosition.Y - (foodTexture.Height * ticket.Size.Y / 2));
                batch.Draw(foodTexture, new Rectangle(foodX, foodY, (int) (foodTexture.Width * ticket.Size.X), (int) (foodTexture.Height * ticket.Size.Y)), Color.White);
                if (ticket.ShowIngredients)
                {
                    for (var jndex = 0; jndex < order.Meal.Ingredients.Items.Length; jndex++)
                    {
                        var ingredient = order.Meal.Ingredients.Items[jndex];
                        Texture2D ingredientTexture = ParentGame.Texture[ingredient.TextureName];
                        float halfWidth = (ingredientTexture.Width / 2 * ticket.Size.X);
                        float halfHeight = (ingredientTexture.Height / 2 * ticket.Size.Y);
                        batch.Draw(ingredientTexture, new Rectangle(
                                (int) (ticket.IngredientPosition[jndex].X - halfWidth),
                                (int) (ticket.IngredientPosition[jndex].Y - halfHeight),
                                (int) halfWidth,
                                (int) halfHeight),
                            Color.White);
                    }
                }
            }
        }
        
        private void DrawArrows(SpriteBatch batch)
        {
            var firstMeal = ParentGame.GameState.Orders.First.Meal;
            var firstMealProgress = ParentGame.GameState.Orders.First.MealProgress;
            for (int i = 0; i < firstMeal.Ingredients[firstMealProgress].Combo.Count; i++)
            {
                Texture2D arrowTexture = ParentGame.Texture["UI_up"];
                switch (firstMeal.Ingredients[firstMealProgress].Combo[i])
                {
                    case Key.Up:
                        arrowTexture = ParentGame.Texture["UI_up"];
                        break;
                    case Key.Right:
                        arrowTexture = ParentGame.Texture["UI_right"];
                        break;
                    case Key.Down:
                        arrowTexture = ParentGame.Texture["UI_down"];
                        break;
                    case Key.Left:
                        arrowTexture = ParentGame.Texture["UI_left"];
                        break;
                }
                // Draw the arrow grey if it was already done
                Color arrowColour = Color.White;
                //if (i < gameState.Orders.First.Meal.Ingredients[gameState.Orders.First.Meal.Progress].Progress)
                if (i < ParentGame.GameState.Orders.First.IngredientProgress)
                {
                    arrowColour = Color.SlateGray;
                }
                // Textures for UI have all the same size of 100px
                Vector2 arrowTextureSize = new Vector2(arrowTexture.Width, arrowTexture.Height);
                int arrowTextureX = ArrowPosition.X - arrowTexture.Width * 2 + (arrowTexture.Width) * i;
                int arrowTextureY = ArrowPosition.Y - arrowTexture.Height;
                batch.Draw(arrowTexture, new Rectangle(arrowTextureX, arrowTextureY, (int) arrowTextureSize.X, (int) arrowTextureSize.Y), arrowColour);
            }
        }
    }
}