using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BuildingFood.core
{
    public struct Ticket
    {
        public Point Position { get; set; }
        public Vector2 Size { get; set; }
        public float Percentage { get; set; }
        public string BarColour { get; set; }
        public int OrderNumber { get; set; }
        
        // Child element positions
        public Point BarPosition { get; set; }
        public Point PlatePosition { get; set; }
        public bool ShowIngredients { get; set; }
        public List<Point> IngredientPosition { get; set; }
        public Vector2 IngredientListSize { get; set; }
        public Point NumberPosition { get; set; }

        public Ticket(int orderNumber)
        {
            Position = Point.Zero;
            Size = new Vector2(1, 1);
            Percentage = 100;
            BarColour = "green";
            OrderNumber = orderNumber;
            BarPosition = Point.Zero;
            PlatePosition = Point.Zero;
            ShowIngredients = false;
            IngredientPosition = new List<Point>();
            IngredientListSize = Vector2.Zero;
            NumberPosition = Point.Zero;
        }
    }
}