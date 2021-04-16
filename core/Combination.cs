using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace BuildingFood.core
{
    public class Combination : List<Key>
    {
        public Combination(Key k1, Key k2, Key k3, Key k4) : base(4)
        {
            this.Add(k1);
            this.Add(k2);
            this.Add(k3);
            this.Add(k4);
        }
    }

    public enum Key
    {
        Up, Right, Down, Left
#if DEBUG
        , Skip, Period, M
#endif
    }
}