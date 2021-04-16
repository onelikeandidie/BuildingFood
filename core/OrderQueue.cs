using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingFood.core
{
    public struct OrderQueue
    {
        public Order[] Items { get; set; }
        public int Count { get; set; }
        public bool Empty => Count <= 0;

        public OrderQueue(Order[] items)
        {
            Items = items;
            Count = items.Length;
        }

        public void Add(Order order)
        {
            Order[] orders = Items;
            Array.Resize(ref orders, Count + 1);
            orders[Count] = order;
            Items = orders;
            Count++;
        }
        
        public Order First {
            get => Items[0];
            set => Items[0] = value;
        }

        public void Next()
        {
            Items = Items.Skip(1).ToArray();
            Count--;
        }

        public void Remove(Order order)
        {
            Items = Items.Where(item => item.OrderNumber != order.OrderNumber).ToArray();
            Count--;
        }
    }
}