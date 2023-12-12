using AdventOfCode.Utility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day12
{
    public class SolverDay12 : Solver
    {
        private List<DemiNonogram> _demiNonograms;
        private Dictionary<string, long> _values;
        // Start is called before the first frame update
        void Start()
        {
            ReadData(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);
        }
        private void ReadData(bool verbose)
        {
            _demiNonograms = new List<DemiNonogram>();
            FileReader fileReader = new FileReader(_filename, true);
            string strread = fileReader.Read();
            while (strread != null && strread != string.Empty)
            {
                string[] partline = strread.Split(' ');
                string[] instr = partline[1].Split(",");
                List<int> inst = new List<int>();
                for (int i = 0; i < instr.Length; i++)
                {
                    inst.Add(int.Parse(instr[i]));
                }
                DemiNonogram dmn = new DemiNonogram(partline[0], inst.ToArray());
                _demiNonograms.Add(dmn);
                strread = fileReader.Read();
            }
            fileReader.Close();
            Debug.Log("Data reading finished");
            if (verbose)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Nonograms : ");
                for (int i = 0; i < _demiNonograms.Count; i++)
                {
                    sb.AppendLine(_demiNonograms[i].ToString());
                }
                Debug.Log(sb.ToString());
            }
        }

        private long SolveNonograms(string input, List<int> instruction)
        {
            string key = string.Concat(input, " : ", string.Join(',', instruction));
            long value;
            if (_values.TryGetValue(key, out value))
            {
                return value;
            }
            long sol = GetSolutions(input, instruction);
            _values.Add(key, sol);
            return sol;
        }
        private long GetSolutions(string input, List<int> instruction)
        {
            bool run = true;
            while (run)
            {
                // we don't have any group of # to find
                if (instruction.Count == 0)
                {
                    if (input.Contains('#'))
                    {
                        //we still have # in the input but no more group to find, it's not a solution
                        return 0;
                    }
                    else
                    {
                        return 1; //it's a match
                    }
                }
                if (string.IsNullOrEmpty(input))
                {
                    return 0; // we don't have any bad springs to match, but we still get instruction.
                }
                if (input.StartsWith('.'))
                {
                    // if the input start with., we can remove all the begenning point
                    input = input.TrimStart('.');
                    continue;
                }
                if (input.StartsWith('?'))
                {
                    //we try the two possibility
                    return SolveNonograms("#" + input.Substring(1), instruction)
                        + SolveNonograms("." + input.Substring(1), instruction);
                }
                if (input.StartsWith("#"))
                {
                    //It's the beginning of a group of #
                    if (instruction.Count == 0)
                    {
                        return 0; //no instruction, it's not a solution.
                    }
                    if (input.Length < instruction[0]) //not enough character to match
                    {
                        return 0;
                    }
                    if (input.Substring(0, instruction[0]).Contains('.'))
                    {
                        // if there is a . in the group
                        return 0;
                    }
                    if (instruction.Count > 1)
                    {
                        if (input.Length < instruction[0] + 1 || input[instruction[0]] == '#')
                        {
                            // to avoid a # after the group
                            return 0;
                        }
                        input = input.Substring(instruction[0] + 1);
                        instruction = instruction.GetRange(1, instruction.Count - 1);
                        continue;
                    }
                    input = input.Substring(instruction[0]);
                    instruction = instruction.GetRange(1, instruction.Count - 1);
                }
            }
            return 0;

        }
        private void SolvePart1(bool verbose)
        {
            long solution = 0L;
            for (int i = 0; i < _demiNonograms.Count; i++)
            {
                _values = new Dictionary<string, long>();
                long tempSol = SolveNonograms(_demiNonograms[i].Line, _demiNonograms[i].Instruction);
                solution += tempSol;
                Debug.Log($"{_demiNonograms[i].ToString()} : {tempSol} solutions");
            }
            Debug.Log("Solution : " + solution);
        }
        private void SolvePart2(bool verbose)
        {
            long solution = 0L;
            for (int i = 0; i < _demiNonograms.Count; i++)
            {
                _values = new Dictionary<string, long>();
                string str = string.Join('?', Enumerable.Repeat(_demiNonograms[i].Line, 5));
                List<int> instructions = Enumerable.Repeat(_demiNonograms[i].Instruction, 5).SelectMany(x => x).ToList();
                long tempSol = SolveNonograms(str, instructions);
                solution += tempSol;
                Debug.Log($"{_demiNonograms[i].ToString()} : {tempSol} solutions");
            }
            Debug.Log($"Solution Part 2 ; {solution}");
        }


    }




}