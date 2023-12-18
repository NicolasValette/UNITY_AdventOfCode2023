using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Algorithms
{
    public class GenericAlgo
    {
        public static void FloodFill(int i, int j, ref string[,] inp, int iLength, int jLenght)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            if (inp[i,j] == ".")
            {
                stack.Push((i, j));
                while (stack.Count > 0)
                {
                    (int, int) pair = stack.Pop();
                    inp[pair.Item1,pair.Item2] = "@";

                    if (pair.Item1 - 1 >= 0 && inp[pair.Item1 - 1,pair.Item2] == ".")
                    {
                        stack.Push((pair.Item1 - 1, pair.Item2));
                    }
                    if (pair.Item1 + 1 < iLength && inp[pair.Item1 + 1,pair.Item2] == ".")
                    {
                        stack.Push((pair.Item1 + 1, pair.Item2));
                    }
                    if (pair.Item2 - 1 >= 0 && inp[pair.Item1,pair.Item2 - 1] == ".")
                    {
                        stack.Push((pair.Item1, pair.Item2 - 1));
                    }
                    if (pair.Item2 + 1 < jLenght && inp[pair.Item1,pair.Item2 + 1] == ".")
                    {
                        stack.Push((pair.Item1, pair.Item2 + 1));
                    }

                }


            }
        }
    }
}