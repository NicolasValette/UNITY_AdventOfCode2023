using AdventOfCode.Datas;
using AdventOfCode.Solver.Day19;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

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

        private void SolvePart1(bool verbose)
        {
            StringBuilder stringBuilder = new StringBuilder();
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
        private void SolvePart2(bool verbose)
        {

        }
    }
}