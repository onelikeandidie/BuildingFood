using System;
using System.Collections.Generic;
using System.Linq;
using BuildingFood.core;
using BuildingFood.systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BuildingFood
{
    public class BuildingFoodGame : Game
    {
        private SpriteBatch spriteBatch;
        public int ScreenWidth;
        public int ScreenHeight;

        public Dictionary<String, Texture2D> Texture;
        public Dictionary<String, SoundEffect> Sound;

        private KeyboardState keyboardPrev = new KeyboardState();

        public List<Meal> Meals;
        public Dictionary<String,Ingredient> Ingredients;
        public SpriteFont Font;

        private Color clearColor;

        public GameState GameState;
        public UIManager UIManager;
        public FoodManager FoodManager;
        public ParticleSystem ParticleSystem;
        public BackgroudManager BackgroudManager;
        public SoundManager SoundManager;
        
        public BuildingFoodGame()
        {
            GraphicsDeviceManager graphicsDeviceManager = new GraphicsDeviceManager(this);
            IsFixedTimeStep = true;

            ScreenWidth = 1280;
            ScreenHeight = 720;

            graphicsDeviceManager.PreferredBackBufferWidth = ScreenWidth;
            graphicsDeviceManager.PreferredBackBufferHeight = ScreenHeight;
        }

        protected override void Initialize()
        {
            Meals = new List<Meal>();
            Ingredients = new Dictionary<string,Ingredient>();
            Sound = new Dictionary<string, SoundEffect>();
            clearColor = Color.Beige;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameState = new GameState(this);
            UIManager = new UIManager(this);
            FoodManager = new FoodManager(this);
            ParticleSystem = new ParticleSystem(this);
            BackgroudManager = new BackgroudManager(this);
            SoundManager = new SoundManager(this);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Content.RootDirectory = "./Content";
            // Load Default Font
            Font = Content.Load<SpriteFont>("JetBrainsMono");

            // Create the texture dictionary
            Texture = new Dictionary<String, Texture2D>();
            // Add each UI texture
            Texture.Add("UI_up", Content.Load<Texture2D>("up"));
            Texture.Add("UI_left", Content.Load<Texture2D>("left"));
            Texture.Add("UI_down", Content.Load<Texture2D>("down"));
            Texture.Add("UI_right", Content.Load<Texture2D>("right"));
            Texture.Add("UI_combo1", Content.Load<Texture2D>("combo_1"));
            Texture.Add("UI_combo2", Content.Load<Texture2D>("combo_2"));
            Texture.Add("UI_combo3", Content.Load<Texture2D>("combo_3"));
            Texture.Add("UI_combo4", Content.Load<Texture2D>("combo_4"));
            Texture.Add("UI_combo5", Content.Load<Texture2D>("combo_5"));
            Texture.Add("UI_ticket", Content.Load<Texture2D>("ticket"));
            Texture.Add("UI_bar_red", Content.Load<Texture2D>("bar_red"));
            Texture.Add("UI_bar_yellow", Content.Load<Texture2D>("bar_yellow"));
            Texture.Add("UI_bar_green", Content.Load<Texture2D>("bar_green"));
            Texture.Add("UI_clock", Content.Load<Texture2D>("clock"));
            Texture.Add("UI_clock_hand", Content.Load<Texture2D>("clock_hand"));
            Texture.Add("UI_coin", Content.Load<Texture2D>("coin"));
            Texture.Add("UI_note", Content.Load<Texture2D>("note"));
            Texture.Add("UI_notes", Content.Load<Texture2D>("notes"));
            Texture.Add("UI_emote_happy", Content.Load<Texture2D>("emote_happy"));
            Texture.Add("UI_emote_meh", Content.Load<Texture2D>("emote_meh"));
            Texture.Add("UI_emote_unhappy", Content.Load<Texture2D>("emote_unhappy"));
            // Load particle textures
            Texture.Add("particle_knife", Content.Load<Texture2D>("particle_knife"));
            Texture.Add("particle_whisk", Content.Load<Texture2D>("particle_whisk"));
            Texture.Add("particle_fork", Content.Load<Texture2D>("particle_fork"));
            Texture.Add("particle_fire1", Content.Load<Texture2D>("particle_fire1"));
            Texture.Add("particle_fire2", Content.Load<Texture2D>("particle_fire2"));
            Texture.Add("particle_fire3", Content.Load<Texture2D>("particle_fire3"));
            Texture.Add("particle_coin", Content.Load<Texture2D>("particle_coin"));
            Texture.Add("particle_note", Content.Load<Texture2D>("particle_note"));
            Texture.Add("particle_bag", Content.Load<Texture2D>("particle_bag"));
            // Load our fucked font
            Texture.Add("font_0", Content.Load<Texture2D>("font_0"));
            Texture.Add("font_1", Content.Load<Texture2D>("font_1"));
            Texture.Add("font_2", Content.Load<Texture2D>("font_2"));
            Texture.Add("font_3", Content.Load<Texture2D>("font_3"));
            Texture.Add("font_4", Content.Load<Texture2D>("font_4"));
            Texture.Add("font_5", Content.Load<Texture2D>("font_5"));
            Texture.Add("font_6", Content.Load<Texture2D>("font_6"));
            Texture.Add("font_7", Content.Load<Texture2D>("font_7"));
            Texture.Add("font_8", Content.Load<Texture2D>("font_8"));
            Texture.Add("font_9", Content.Load<Texture2D>("font_9"));
            Texture.Add("font_dollar", Content.Load<Texture2D>("font_dollar"));
            // Add each ingredient texture
            Texture.Add("plate", Content.Load<Texture2D>("plate"));
            Texture.Add("spaget", Content.Load<Texture2D>("spaget"));
            Texture.Add("sauce", Content.Load<Texture2D>("sauce"));
            Texture.Add("meatballs", Content.Load<Texture2D>("meatballs"));
            Texture.Add("bottom_bun", Content.Load<Texture2D>("bottom_bun"));
            Texture.Add("burger", Content.Load<Texture2D>("burger"));
            Texture.Add("cheese", Content.Load<Texture2D>("cheese"));
            Texture.Add("tomato", Content.Load<Texture2D>("tomato"));
            Texture.Add("letuce", Content.Load<Texture2D>("letuce"));
            Texture.Add("top_bun", Content.Load<Texture2D>("top_bun"));
            // Add each meal texture
            Texture.Add("meal_meatTower", Content.Load<Texture2D>("meatTower"));
            Texture.Add("meal_ballParty", Content.Load<Texture2D>("ballParty"));
            Texture.Add("meal_pizza", Content.Load<Texture2D>("pizza"));
            Texture.Add("meal_noodleSoup", Content.Load<Texture2D>("noodleSoup"));
            Texture.Add("meal_sushi", Content.Load<Texture2D>("sushi"));
            // Add each background texture
            Texture.Add("back_restaurant_pb", Content.Load<Texture2D>("restaurant_pb"));
            Texture.Add("back_restaurant_ps", Content.Load<Texture2D>("restaurant_ps"));
            Texture.Add("back_restaurant_pt", Content.Load<Texture2D>("restaurant_pt"));
            Texture.Add("back_restaurant_jp", Content.Load<Texture2D>("restaurant_jp"));
            Texture.Add("back_restaurant_sh", Content.Load<Texture2D>("restaurant_sh"));
            Texture.Add("back_restaurant_pub", Content.Load<Texture2D>("restaurant_pub"));
            
            // Add each sound asset
            Sound.Add("knifeSlice1", Content.Load<SoundEffect>("knifeSlice"));
            Sound.Add("knifeSlice2", Content.Load<SoundEffect>("chop"));
            Sound.Add("knifeSlice3", Content.Load<SoundEffect>("drawKnife1"));
            Sound.Add("handleCoins", Content.Load<SoundEffect>("handleCoins"));
            Sound.Add("metalClick", Content.Load<SoundEffect>("metalClick"));

            SoundEffect.MasterVolume = 0.2f;
            
            // Create each ingredient non-procedurally cuz we lazy
            Ingredients.Add("spaget",
                new Ingredient("spaget",
                    "Spaghetti",
                    new Combination(Key.Left,
                        Key.Down,
                        Key.Down,
                        Key.Right)));
            Ingredients.Add("sauce",
                new Ingredient("sauce",
                    "Ball Sauce",
                    new Combination(Key.Left,
                        Key.Up,
                        Key.Right,
                        Key.Down)));
            Ingredients.Add("meatballs",
                new Ingredient("meatballs",
                    "Balls",
                    new Combination(Key.Left,
                        Key.Right,
                        Key.Left,
                        Key.Right)));
            Ingredients.Add("bottom_bun",
                new Ingredient("bottom_bun",
                    "Rear Bun",
                    new Combination(Key.Right,
                        Key.Down,
                        Key.Left,
                        Key.Down)));
            Ingredients.Add("burger",
                new Ingredient("burger",
                    "Meat Cylinder",
                    new Combination(Key.Left,
                        Key.Down,
                        Key.Up,
                        Key.Right)));
            Ingredients.Add("cheese",
                new Ingredient("cheese",
                    "America",
                    new Combination(Key.Right,
                        Key.Up,
                        Key.Down,
                        Key.Down)));
            Ingredients.Add("letuce",
                new Ingredient("letuce",
                    "Paper",
                    new Combination(Key.Right,
                        Key.Left,
                        Key.Down,
                        Key.Left)));
            Ingredients.Add("tomato",
                new Ingredient("tomato",
                    "TomAto",
                    new Combination(Key.Right,
                        Key.Down,
                        Key.Down,
                        Key.Left)));
            Ingredients.Add("top_bun",
                new Ingredient("top_bun",
                    "Top Rack",
                    new Combination(Key.Right,
                        Key.Up,
                        Key.Left,
                        Key.Up)));
            
            // Create the meals non-procedurally because it isn't procJam
            List<Ingredient> bologneseI = new List<Ingredient>();
            bologneseI.Add(Ingredients["spaget"]);
            bologneseI.Add(Ingredients["sauce"]);
            bologneseI.Add(Ingredients["meatballs"]);
            Meals.Add(new Meal(bologneseI, "Ball Party", "meal_ballParty"));
            
            List<Ingredient> burgerI = new List<Ingredient>();
            burgerI.Add(Ingredients["bottom_bun"]);
            burgerI.Add(Ingredients["burger"]);
            burgerI.Add(Ingredients["cheese"]);
            //burgerI.Add(Ingredients["letuce"]);
            //burgerI.Add(Ingredients["tomato"]);
            burgerI.Add(Ingredients["top_bun"]);
            Meals.Add(new Meal(burgerI, "Meat Tower", "meal_meatTower"));

            // Add the first order
            GameState.AddRandomOrder();
            
            // Load textures into systems that need them
            FoodManager.LoadContent();
            
            // Set the background
            BackgroudManager.SetBackground("back_restaurant_ps");

            // Basecly Doone
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            ProcessInput();

            GameState.Update(gameTime);
            UIManager.Update(gameTime);
            FoodManager.Update(gameTime);
            ParticleSystem.Update(gameTime);
            
            base.Update(gameTime);
        }
        
        private void ProcessInput()
        {
            KeyboardState keyboardCurr = Keyboard.GetState();
            if (keyboardCurr != keyboardPrev)
            {
                keyboardPrev = keyboardCurr;
                if (keyboardPrev.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
                if (keyboardPrev.IsKeyDown(Keys.Up) || keyboardPrev.IsKeyDown(Keys.W))
                {
                    KeyPressed(Key.Up);
                    return;
                }
                if (keyboardPrev.IsKeyDown(Keys.Down) || keyboardPrev.IsKeyDown(Keys.S))
                {
                    KeyPressed(Key.Down);
                    return;
                }
                if (keyboardPrev.IsKeyDown(Keys.Left) || keyboardPrev.IsKeyDown(Keys.A))
                {
                    KeyPressed(Key.Left);
                    return;
                }
                if (keyboardPrev.IsKeyDown(Keys.Right) || keyboardPrev.IsKeyDown(Keys.D))
                {
                    KeyPressed(Key.Right);
                    return;
                }
#if DEBUG
                if (keyboardPrev.IsKeyDown(Keys.OemComma))
                {
                    KeyPressed(Key.Skip);
                    return;
                }
                if (keyboardPrev.IsKeyDown(Keys.OemPeriod))
                {
                    KeyPressed(Key.Period);
                    return;
                }
                if (keyboardPrev.IsKeyDown(Keys.M))
                {
                    KeyPressed(Key.M);
                    return;
                }
#endif
            }
        }

        private void KeyPressed(Key keyPressed)
        {
            GameState.KeyPressed(keyPressed);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(clearColor);

            // Begin Drawing
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null);
            BackgroudManager.Draw(spriteBatch);
            FoodManager.Draw(spriteBatch);
            UIManager.Draw(spriteBatch);
            ParticleSystem.Draw(spriteBatch);
#if DEBUG
            //DrawDebug(spriteBatch);
#endif
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void DrawParticles(SpriteBatch batch)
        {
            
        }

#if DEBUG
        private void DrawDebug(SpriteBatch batch)
        {
            batch.DrawString(Font, GameState.TimeItTookForOneMeal.ToString(), Vector2.Zero, Color.Black);
            batch.DrawString(Font, "x" + GameState.Momentum.ToString(), new Vector2(0, 10), Color.Black);
            batch.DrawString(Font, "$" + GameState.Money, new Vector2(0, 30), Color.Black);
            for (var index = 0; index < GameState.Orders.Count; index++)
            {
                Order order = GameState.Orders.Items[index];
                float lenghtOfName = Font.MeasureString(order.Meal.Name).X;
                batch.DrawString(Font, order.Meal.Name, new Vector2(ScreenWidth - lenghtOfName - 20, 40 * index), Color.Black);
                int calculatedPrice = GameState.CalculatePay(order);
                float lenghtOfDescription =
                    Font.MeasureString("$" + calculatedPrice + " " + (int)  order.RemainingTime + "s /" + order.Time + "s").X;
                batch.DrawString(Font, "$" + calculatedPrice + " " + (int) order.RemainingTime + "s /" + order.Time + "s", new Vector2(ScreenWidth - lenghtOfDescription - 20, 40 * index + 20), Color.Black);
            }
            batch.DrawString(Font, Ingredients["burger"].Name + " Progress:" + Ingredients["burger"].Progress, new Vector2(0, 50), Color.Black);
            batch.DrawString(Font, Meals[1].Ingredients[1].Name + " Progress:" + Meals[1].Ingredients[1].Progress, new Vector2(0, 70), Color.Black);
            if (!GameState.Orders.Empty)
            {
                batch.DrawString(Font, "Ingredient Progress:" + GameState.Orders.First.IngredientProgress, new Vector2(0, 100), Color.Black);
                batch.DrawString(Font, "Meal Progress:" + GameState.Orders.First.MealProgress, new Vector2(0, 120), Color.Black);
            }
            else
            {
                batch.DrawString(Font, "Ingredient Progress: No Order", new Vector2(0, 100), Color.Black);
                batch.DrawString(Font, "Meal Progress: No Order", new Vector2(0, 120), Color.Black);
            }
        }
#endif
    }
}
