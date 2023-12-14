using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day14
{
    public enum Direction
    {
        North,
        East,
        West,
        South
    }
    public class SolverDay14 : Solver
    {
        private char[][] _level;
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
            string[] lines = _input.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
            _level= new char[lines.Length][];
            for (int i=0; i<lines.Length; i++)
            {
                _level[i] = lines[i].Trim().ToCharArray();
            }
            Debug.Log("Reading data finished");
        }

        private void Rotate()
        {
            char[][] newLevel = new char[_level[0].Length][];
            for (int i = 0; i < newLevel.Length; i++)
            {
                newLevel[i] = new char[_level.Length];
                for (int j = 0;j<_level.Length; j++)
                {
                    newLevel[i][j] = _level[_level.Length - j -1][i];
                }
            }
            _level = newLevel;
        }
        private int GetFirstRowPos (int i, int j)
        {
            int row = i;
            while ((row > 0) && (_level[row-1][j] == '.'))
            {
                row--;
            }
            return row;
        }
        private void LetsGetRollin()
        {
            for (int i = 0; i < _level.Length; i++)
            {
                for (int j=0; j < _level[i].Length; j++)
                {
                    if (_level[i][j] == 'O')
                    {
                        int row = GetFirstRowPos(i, j);
                        if (row != i) 
                        {
                            _level[i][j] = '.';
                            _level[row][j] = 'O';
                        }
                    }
                }
            }
        }
        private void Cycle()
        {
            LetsGetRollin();
            Rotate();
            LetsGetRollin();
            Rotate();
            LetsGetRollin();
            Rotate();
            LetsGetRollin();
            Rotate();
        }

        private string PrintLevel()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _level.Length; i++)
            {
                sb.AppendLine(new string(_level[i]));
            }
            return sb.ToString();
        }
        private long CalculateLoad()
        {
            long load = 0L;
            for (int i = 0; i < _level.Length; i++)
            {
                for (int j = 0; j < _level[i].Length; j++)
                {
                    if (_level[i][j] == 'O')
                    {
                        load += _level.Length - i;
                    }
                }
            }
            return load;
        }
        private void SolvePart1(bool verbose)
        {
            if (verbose) Debug.Log(PrintLevel());
            LetsGetRollin();
            if (verbose)
            {
                Debug.Log("Level after rolling : ");
                Debug.Log(PrintLevel());
            }
            long sol = CalculateLoad();
            Debug.Log($"Solution Part 1 {sol}");
        }
        private void SolvePart2(bool verbose)
        {
            long nbCycle = 1000000000;
            List<string> seenLevel = new List<string>();
            var hashLvl = PrintLevel();
            while (!seenLevel.Contains(hashLvl) && nbCycle > 0)
            {
                seenLevel.Add(hashLvl);
                Cycle();
                nbCycle--;
                hashLvl = PrintLevel();
            }

            
            var loop = seenLevel.Count - seenLevel.IndexOf(hashLvl);
           
            nbCycle %= loop;
            if (verbose) Debug.Log($"Loop detected ! size {loop}, {nbCycle} remains");
            while (nbCycle > 0)
            {
                Cycle();
                nbCycle--;
            }
            long sol = CalculateLoad();
            Debug.Log($"Solution Part 2 : {sol}");
        }

    }
}