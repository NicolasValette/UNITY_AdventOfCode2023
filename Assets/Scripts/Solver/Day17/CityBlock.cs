using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Solver.Day17
{
    public class CityBlock
    { 
        public int IPos { get; private set; }
        public int JPos { get; private set; }
        public int HeatLoss { get; private set; }
        public CityBlock(int iPos, int jPos, int heatLoss)
        {
            IPos = iPos;
            JPos = jPos;
            HeatLoss = heatLoss;
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        public override bool Equals(object obj)
        {
            CityBlock o = obj as CityBlock;
            return string.Equals(ToString(), o.ToString());
        }
        public override string ToString()
        {
            return $"({IPos};{JPos})-{HeatLoss}°";
        }
    }
}
