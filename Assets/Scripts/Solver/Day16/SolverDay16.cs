using AdventOfCode.Utility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day16
{
    public class SolverDay16 : Solver
    {
        private string[] _contraption;
        private List<string> _visited;
        private bool[,] _energized;
        // Start is called before the first frame update
        void Start()
        {
            ReadData(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);
        }

        private void ReadData(bool verbose)
        {
            string path = Path.Combine(Application.streamingAssetsPath, _filename);
            if (_verbose) Debug.Log($"Reading file : {path}");
            StreamReader _stream = new StreamReader(path);
            _input = _stream.ReadToEnd();
            _contraption = _input.Split('\n');
            _energized = new bool[_contraption.Length,_contraption[0].Trim().Length];
        }

       

        private int ComputeBeam (int starti, int startj, Direction dir)
        {
            int solution = 0;
            Beam startingBeam = new Beam(starti, startj, dir);
            _visited = new List<string>();
            _energized = new bool[_contraption.Length, _contraption[0].Trim().Length];
            while (Beam.BeamList.Count > 0)
            {

                for (int i = 0; i < Beam.BeamList.Count; i++)
                {
                    string str = $"{Beam.BeamList[i].Ipos}-{Beam.BeamList[i].Jpos}-{Beam.BeamList[i].Direction}";
                    if (_visited.Contains(str))
                    {
                        Beam.BeamList[i].AlreadyVisited();
                    }
                    else
                    {
                        _visited.Add(str);
                        if (_energized[Beam.BeamList[i].Ipos, Beam.BeamList[i].Jpos] == false)
                        {
                            solution++;
                            _energized[Beam.BeamList[i].Ipos, Beam.BeamList[i].Jpos] = true;
                        }
                        Beam.BeamList[i].Move(ref _contraption);
                    }
                }
            }
            return solution;
        }
        private void SolvePart1(bool verbose)
        {
            int solution = 0;
            Beam startingBeam = new Beam(0, 0, Direction.East);
            _visited = new List<string>();
            while (Beam.BeamList.Count > 0)
            {
                
                for (int i=0; i<Beam.BeamList.Count; i++)
                {
                    string str = $"{Beam.BeamList[i].Ipos}-{Beam.BeamList[i].Jpos}-{Beam.BeamList[i].Direction}";
                    if (_visited.Contains(str))
                    {
                        Beam.BeamList[i].AlreadyVisited();
                    }
                    else
                    {
                        _visited.Add(str);
                        if (_energized[Beam.BeamList[i].Ipos, Beam.BeamList[i].Jpos] == false)
                        {
                            solution++;
                            _energized[Beam.BeamList[i].Ipos, Beam.BeamList[i].Jpos] = true;
                        }
                        Beam.BeamList[i].Move(ref _contraption);
                    }
                }
            }
            if (verbose)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Visited tiles : ");
                for (int i = 0; i < _contraption.Length;i++)
                {
                    for (int j = 0; j < _contraption[i].Trim().Length; j++)
                    {
                        char c = (_energized[i,j]) ? '#' : '.';
                        sb.Append(c);
                    }
                    sb.Append('\n');
                }
                Debug.Log(sb.ToString());
            }
            Debug.Log($"Solution Part 1 : {solution}");
        }
        private void SolvePart2(bool verbose)
        {
            int solution = 0;
            for (int i = 0;i<_contraption.Length;i++)
            {
                int tempSol;
                tempSol = ComputeBeam(i, 0, Direction.East);
                if (tempSol > solution) 
                {
                    solution = tempSol;
                }
                tempSol = ComputeBeam(i, _contraption[i].Trim().Length-1, Direction.West);
                if (tempSol > solution)
                {
                    solution = tempSol;
                }
            }
            for (int j = 0; j < _contraption[0].Trim().Length; j++)
            {
                int tempSol;
                tempSol = ComputeBeam(0, j, Direction.South);
                if (tempSol > solution)
                {
                    solution = tempSol;
                }
                tempSol = ComputeBeam(_contraption.Length-1, j, Direction.North);
                if (tempSol > solution)
                {
                    solution = tempSol;
                }
            }
            Debug.Log($"Solution part2 : {solution}");
        }
    }
}