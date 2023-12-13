using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Interfaces
{
    public interface IMove
    {
        public void Move(Vector2 direction);
        public void DepthMove(float value);
    }
}