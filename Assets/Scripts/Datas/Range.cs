using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Datas
{
    public class Range
    {
        public int MinValue { get; private set; }
        public int MaxValue { get; private set;}
        public long Count { get => ((MaxValue - MinValue) + 1L); }
        public Range (int min, int max)
        {
            MinValue = min;
            MaxValue = max;
        }
        public void UpdateRange (int min, int max)
        {
            MinValue = min;
            MaxValue = max;
        }
    }
}