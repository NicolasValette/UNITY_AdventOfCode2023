using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day12
{
    public class DemiNonogram
    {
        private char[] _charLine;
        private int[] _instructions;
        public string Line { get => new string(_charLine); } 
        public List<int> Instruction { get => _instructions.ToList(); }
    
       
        public DemiNonogram(string line, int[]instructions)
        {
            _charLine = new char[line.Length];
            _charLine = line.ToCharArray();
            _instructions = new int[instructions.Length];
            _instructions = instructions;                              
        }
        public void SetNewNonogramm (string line, int[] instructions)
        {
            _charLine = new char[line.Length];
            _charLine = line.ToCharArray();
            _instructions = new int[instructions.Length];
            _instructions = instructions;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_instructions[0]);
            for (int i=1; i< _instructions.Length;i++)
            {
                sb.Append($", {_instructions[i]}");
            }
            return $"{new string(_charLine)} {sb.ToString()}";
        }
    }
}