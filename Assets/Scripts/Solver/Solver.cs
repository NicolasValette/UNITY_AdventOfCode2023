using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Solver
{
    public abstract class Solver : MonoBehaviour
    {
        [SerializeField]
        protected bool _verbose = false;
        [SerializeField]
        protected bool _part1 = false;
        [SerializeField]
        protected bool _part2 = false;
        [SerializeField]
        protected string _filename;
        [TextArea(17, 1000)]
        [SerializeField]
        protected string _input;
    }
}