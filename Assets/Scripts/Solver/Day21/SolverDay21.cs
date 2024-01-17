using AdventOfCode.Datas;
using AdventOfCode.Solver.Day19;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AdventOfCode.Solver.Day21
{
    public class SolverDay21 : Solver
    {
        [SerializeField]
        private int _steps = 1;

        private List<List<char>> _garden;
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

            int ind = 0;
            while (!_stream.EndOfStream)
            {
                string line = _stream.ReadLine();
                if (_garden == null)
                {
                    _garden = new List<List<char>>(line.Length);
                }
                _garden.Add(new List<char>(line.ToList()));
                ind++;
            }
            if (verbose) Debug.Log("Finish");
        }

        private IntCoords FindStartingPos()
        {
            
             for (int i = 0; i < _garden.Count; i++)
            {
                if (_garden[i].Contains('S'))
                {

                    int y = _garden[i].IndexOf('S');
                    return new IntCoords(i, y);
                }
            }
            return new IntCoords(-1,-1);
        }

        private long MakeAStep(Stack<IntCoords> potentialPosition)
        {
            long steps = 0L;
            while (potentialPosition.TryPop(out IntCoords pos))
            {
                if (_verbose) Debug.Log($"potential position ({pos.X},{pos.Y})");

                //North
                _garden[5][5] = 'k';
                if (pos.X - 1 >= 0 && _garden[pos.X - 1][pos.Y] != '#')
                {
                    _garden[pos.X - 1][pos.Y] = 'O';
                    steps++;
                }
                // South
                if (pos.X + 1 < _garden.Count && _garden[pos.X + 1][pos.Y] != '#')
                {
                    _garden[pos.X + 1][pos.Y] = 'O';
                    steps++;
                }
                //West
                if (pos.Y - 1 >= 0 && _garden[pos.X][pos.Y - 1] != '#')
                {
                    _garden[pos.X][pos.Y - 1] = 'O';
                    steps++;
                }
                //East
                if (pos.Y + 1 < _garden[pos.X].Count && _garden[pos.X][pos.Y + 1] != '#')
                {
                    _garden[pos.X][pos.Y + 1] = 'O';
                    steps++;
                }

                _garden[pos.X][pos.Y] = '.';

            }
            return steps;
        }
        

        private Stack<IntCoords> GetPotentialPosition()
        {
            Stack<IntCoords> position = new Stack<IntCoords>();

            for (int i = 0; i < _garden.Count; i++)
            {
                for (int j = 0; j < _garden[i].Count; j++)
                {
                    if (_garden[i][j] == 'O' || _garden[i][j] == 'S')
                    {
                        IntCoords pos = new IntCoords(i, j);
                        position.Push(pos);
                    }
                }
            }

            return position;
        }

        private long GetNumberOfGardenPlotReached()
        {
            long sol = 0L;
            for (int i = 0; i < _garden.Count; i++)
            {
                for (int j = 0; j < _garden[i].Count; j++)
                {
                    if (_garden[i][j] == 'O')
                    {
                        sol++;
                    }
                }
            }
            return sol;
        }

        private int GetReachable (IntCoords pos, int numberOfSteps)
        {
            Queue<(IntCoords, int)> queue = new Queue<(IntCoords, int)> ();
            List <IntCoords> seen = new List <IntCoords> ();
            List<IntCoords> ans = new List<IntCoords>();
            queue.Enqueue((pos, numberOfSteps));
            seen.Add(pos);

            while(queue.TryDequeue(out var next))
            {
                if (next.Item2 %2 == 0)  // if it remains an even number of steps, we can stop here.
                {
                    ans.Add(next.Item1);
                }
                if (next.Item2 == 0)
                {
                    continue;
                }
                List<IntCoords> nextstep = new List<IntCoords> ();
                nextstep.Add(new IntCoords(next.Item1.X + 1, next.Item1.Y));
                nextstep.Add(new IntCoords(next.Item1.X - 1, next.Item1.Y));
                nextstep.Add(new IntCoords(next.Item1.X, next.Item1.Y + 1));
                nextstep.Add(new IntCoords(next.Item1.X, next.Item1.Y - 1));

                foreach (var step in nextstep)
                {
                    if ((step.X < 0) || (step.X >= _garden.Count) || (step.Y < 0) || (step.Y >= _garden[0].Count) ||
                            _garden[step.X][step.Y] == '#' || seen.Contains(step))
                    {
                        continue;
                    }
                    seen.Add(step);
                    queue.Enqueue((step, next.Item2 - 1));
                }

            }
            return ans.Count();
        }
        private void SolveOldPart1(bool verbose)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var startingPos = FindStartingPos();
            for (int i = 0; i < _steps; i++)
            {
                if (verbose) Debug.Log($"--- Step number {i + 1}---");
                MakeAStep(GetPotentialPosition());
               

                for (int j = 0; j < _garden.Count; j++)
                {

                    stringBuilder.AppendLine(new string(_garden[j].ToArray()));

                }
                stringBuilder.AppendLine("*****************");
            }
            if (verbose)
            {
                
                Debug.Log(stringBuilder.ToString());
            }
            Debug.Log($"Solution Part 1 : {GetNumberOfGardenPlotReached()}");

    
        }

        private void SolvePart1(bool verbose)
        {
            var startingPos = FindStartingPos();
            Debug.Log($"Solution Part1 : {GetReachable(startingPos, 64)}");
        }

        // thanks to https://www.youtube.com/watch?v=9UOMZSL0JTg
        private void SolvePart2(bool verbose)
        {
            var startingPos = FindStartingPos();
            int gridWidth = _steps / _garden.Count - 1;
            
            double odd = Math.Pow(((gridWidth / 2) * 2 + 1), 2);
            double even = Math.Pow((((gridWidth +1)/ 2) * 2), 2);

            double oddPoints = GetReachable(startingPos, _garden.Count * 2 + 1);
            double evenPoints = GetReachable(startingPos, _garden.Count * 2);

            double cornerTop = GetReachable(new IntCoords(_garden.Count - 1, startingPos.Y), _garden.Count - 1);
            double cornerRight = GetReachable(new IntCoords(startingPos.X, 0), _garden.Count - 1);
            double cornerBottom = GetReachable(new IntCoords(0, startingPos.Y), _garden.Count - 1);
            double cornerLeft = GetReachable(new IntCoords(startingPos.X, _garden.Count - 1), _garden.Count - 1);

            double smalltr = GetReachable(new IntCoords(_garden.Count - 1, 0), _garden.Count / 2 - 1);
            double smalltl = GetReachable(new IntCoords(_garden.Count - 1, _garden.Count - 1), _garden.Count / 2 - 1);
            double smallbr = GetReachable(new IntCoords(0, 0), _garden.Count / 2 - 1);
            double smallbl = GetReachable(new IntCoords(0, _garden.Count - 1), _garden.Count / 2 - 1);

            double largetr = GetReachable(new IntCoords(_garden.Count - 1, 0), _garden.Count * 3 / 2 - 1);
            double largetl = GetReachable(new IntCoords(_garden.Count - 1, _garden.Count - 1), _garden.Count * 3 / 2 - 1);
            double largebr = GetReachable(new IntCoords(0, 0), _garden.Count * 3 / 2 - 1);
            double largebl = GetReachable(new IntCoords(0, _garden.Count - 1), _garden.Count * 3 / 2 - 1);



            double solution = odd * oddPoints + even * evenPoints
                + cornerBottom + cornerLeft + cornerRight + cornerTop
                + (gridWidth + 1) * (smalltr + smalltl + smallbr + smallbl)
                + gridWidth * (largetr + largetl + largebr + largebl);
            Debug.Log($"Solution part2 : {solution}");
        }
    }
}