using AdventOfCode.Datas;
using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace AdventOfCode.Solver.Day10
{
    public enum TypeOfPipe
    {
        Vertical,
        Horizontal,
        LShape,
        JShape,
        SevenShape,
        FShape,
        Ground,
        Start
    }
    public enum Direction
    {
        North,
        South,
        East,
        West,
        Northeast,
        Northwest,
        Southeast,
        Southwest,
        None
    }
    public class Pipe
    {
        private int line;
        private int column;
        public TypeOfPipe Type { get; set; }
        public Pipe(TypeOfPipe orientation, int l, int c)
        {
            line = l;
            column = c;
            Type = orientation;
        }
    }
    public class SolverDay10 : Solver
    {
        private Graph<Pipe> _graph;

        private string _cleanInput;
        // Start is called before the first frame update
        void Start()
        {
            ReadData(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);
        }

        private TypeOfPipe GetPipe(char pipe)
        {
            if (pipe == '|')
            {
                return TypeOfPipe.Vertical;
            }
            else if (pipe == '-')
            {
                return TypeOfPipe.Horizontal;
            }
            else if (pipe == 'L')
            {
                return TypeOfPipe.LShape;
            }
            else if (pipe == 'J')
            {
                return TypeOfPipe.JShape;
            }
            else if (pipe == '7')
            {
                return TypeOfPipe.SevenShape;
            }
            else if (pipe == 'F')
            {
                return TypeOfPipe.FShape;
            }
            else if (pipe == 'S')
            {
                return TypeOfPipe.Start;
            }
            else
            {
                return TypeOfPipe.Ground;
            }
        }
        private Direction GetNextDirection(Direction from, char pipe)
        {
            TypeOfPipe pipeType = GetPipe(pipe);

            if (pipeType == TypeOfPipe.Vertical)
            {
                if (from == Direction.South)
                {
                    return Direction.South;
                }
                if (from == Direction.North)
                {
                    return Direction.North;
                }
            }
            else if (pipeType == TypeOfPipe.Horizontal)
            {
                if (from == Direction.East)
                {
                    return Direction.East;
                }
                if (from == Direction.West)
                {
                    return Direction.West;
                }
            }
            else if (pipeType == TypeOfPipe.LShape)
            {
                if (from == Direction.South)
                {
                    return Direction.East;
                }
                if (from == Direction.West)
                {
                    return Direction.North;
                }
            }
            else if (pipeType == TypeOfPipe.JShape)
            {
                if (from == Direction.South)
                {
                    return Direction.West;
                }
                if (from == Direction.East)
                {
                    return Direction.North;
                }
            }
            else if (pipeType == TypeOfPipe.SevenShape)
            {
                if (from == Direction.North)
                {
                    return Direction.West;
                }
                if (from == Direction.East)
                {
                    return Direction.South;
                }
            }
            else if (pipeType == TypeOfPipe.FShape)
            {
                if (from == Direction.North)
                {
                    return Direction.East;
                }
                if (from == Direction.West)
                {
                    return Direction.South;
                }
            }
            return Direction.None;
        }
        private void AddPipeToGraph(Pipe from, Pipe to)
        {
            _graph.AddEdge(from, to);
        }
        private void FloodFill(int i, int j, ref string[] inp)
        {
            if (inp[i][j] == '#')
            {
                StringBuilder sb = new StringBuilder(inp[i]);
                sb[j] = '@';
                inp[i] = sb.ToString();
                sb = null;
                if (i - 1 >= 0)
                {
                    FloodFill(i - 1, j, ref inp);
                }
                if (i + 1 < inp.Length)
                {
                    FloodFill(i + 1, j, ref inp);
                }
                if (j - 1 >= 0)
                {
                    FloodFill(i, j - 1, ref inp);
                }
                if (j + 1 < inp[i].Trim().Length)
                {
                    FloodFill(i, j + 1, ref inp);
                }
            }
        }

        private void FloodFillNonRecV2(int i, int j, ref string[] inp)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            if (inp[i][j] == '#')
            {
                stack.Push((i, j));
                while (stack.Count > 0)
                {
                    (int, int) pair = stack.Pop();
                    if (inp[pair.Item1][pair.Item2] == '#')
                    {
                        int west = pair.Item2;
                        int east = pair.Item2;
                        while (inp[pair.Item1][west] == '#' && west - 1 >= 0)
                        {
                            west--;
                        }
                        while (inp[pair.Item1][east] == '#' && east + 1 < inp[pair.Item1].Trim().Length)
                        {
                            east++;
                        }
                        for (int k = west; k < east; k++)
                        {
                            StringBuilder sb = new StringBuilder(inp[pair.Item1]);
                            sb[k] = '@';


                            if (pair.Item1 - 1 >= 0 && inp[pair.Item1 - 1][pair.Item2] == '#')
                            {
                                stack.Push((pair.Item1 - 1, pair.Item2));
                            }
                            if (pair.Item1 + 1 < inp.Length && inp[pair.Item1 + 1][pair.Item2] == '#')
                            {
                                stack.Push((pair.Item1 + 1, pair.Item2));
                            }
                            inp[pair.Item1] = sb.ToString();
                        }
                    }
                }
            }
        }
        private void FloodFillNonRec(int i, int j, ref string[] inp)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            if (inp[i][j] == '#')
            {
                stack.Push((i, j));
                while (stack.Count > 0)
                {
                    (int, int) pair = stack.Pop();
                    StringBuilder sb = new StringBuilder(inp[pair.Item1]);
                    sb[pair.Item2] = '@';
                    inp[pair.Item1] = sb.ToString();

                    if (pair.Item1 - 1 >= 0 && inp[pair.Item1 - 1][pair.Item2] == '#')
                    {
                        stack.Push((pair.Item1 - 1, pair.Item2));
                    }
                    if (pair.Item1 + 1 < inp.Length && inp[pair.Item1 + 1][pair.Item2] == '#')
                    {
                        stack.Push((pair.Item1 + 1, pair.Item2));
                    }
                    if (pair.Item2 - 1 >= 0 && inp[pair.Item1][pair.Item2 - 1] == '#')
                    {
                        stack.Push((pair.Item1, pair.Item2 -1));
                    }
                    if (pair.Item2 + 1 < inp[pair.Item1].Trim().Length && inp[pair.Item1][pair.Item2 +1] == '#')
                    {
                        stack.Push((pair.Item1, pair.Item2 + 1));
                    }

                }


            }
        }
        private string EraseNonLoopPipe(bool[,] bgraph)
        {
            StringBuilder sb = new StringBuilder();
            string[] lines = _input.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                StringBuilder sbTemp = new StringBuilder(lines[i].Trim());
                for (int j = 0; j < lines[i].Trim().Length; j++)
                {
                    if (!bgraph[i, j])
                    {
                        sbTemp[j] = '.';
                    }
                }
                sb.AppendLine(sbTemp.ToString().Trim());
            }
            return sb.ToString();
        }
        private string ComputeInput()
        {
            StringBuilder sb = new StringBuilder();
            string[] lines = _cleanInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                StringBuilder sbTemp = new StringBuilder();
                StringBuilder sbTemp2 = new StringBuilder();
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '.')
                    {
                        sbTemp.Append(".#");
                        sbTemp2.Append("##");
                    }
                    if (lines[i][j] == 'S')
                    {
                        sbTemp.Append("S-");
                        sbTemp2.Append("|#");
                    }
                    if (lines[i][j] == '-')
                    {
                        sbTemp.Append("--");
                        sbTemp2.Append("##");
                    }
                    if (lines[i][j] == '|')
                    {
                        sbTemp.Append("|#");
                        sbTemp2.Append("|#");
                    }
                    if (lines[i][j] == '7')
                    {
                        sbTemp.Append("7#");
                        sbTemp2.Append("|#");
                    }
                    if (lines[i][j] == 'J')
                    {
                        sbTemp.Append("J#");
                        sbTemp2.Append("##");
                    }
                    if (lines[i][j] == 'L')
                    {
                        sbTemp.Append("L-");
                        sbTemp2.Append("##");
                    }
                    if (lines[i][j] == 'F')
                    {
                        sbTemp.Append("F-");
                        sbTemp2.Append("|#");
                    }
                }
                sb.AppendLine(sbTemp.ToString());
                sb.AppendLine(sbTemp2.ToString());
            }
            return sb.ToString();
        }
        private void ReadData(bool verbose)
        {

            _graph = new Graph<Pipe>();
            string[] lines = _input.Split('\n');
            bool[,] boolGraph = new bool[lines.Length, lines[0].Trim().Length];


            int ind = 0;
            int movingIndex = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                ind = lines[i].IndexOf('S');
                if (ind != -1)
                {
                    movingIndex = i;
                    break;
                }
            }
            bool isFinished = false;
            Direction way1 = Direction.None, way2;

            if ((lines[movingIndex][ind - 1] == 'F')
                || (lines[movingIndex][ind - 1] == 'L')
                || (lines[movingIndex][ind - 1] == '-'))
            {
                way1 = Direction.West;
            }
            else if ((lines[movingIndex][ind + 1] == '-')
                || (lines[movingIndex][ind + 1] == '7')
                || (lines[movingIndex][ind + 1] == 'J'))
            {
                way1 = Direction.East;
            }
            else if ((lines[movingIndex - 1][ind] == '|')
                || (lines[movingIndex - 1][ind] == '7')
                || (lines[movingIndex - 1][ind] == 'F'))
            {
                way1 = Direction.North;
            }
            else if ((lines[movingIndex + 1][ind] == '|')
                || (lines[movingIndex + 1][ind] == 'J')
                || (lines[movingIndex + 1][ind] == 'L'))
            {
                way1 = Direction.South;
            }
            Pipe startPipe = new Pipe(TypeOfPipe.Start, movingIndex, ind);
            Pipe prevPipe = new Pipe(TypeOfPipe.Start, movingIndex, ind);
            Pipe actPipe;
            while (!isFinished)
            {
                boolGraph[movingIndex, ind] = true;
                if (way1 == Direction.North)
                {
                    movingIndex += -1;
                }
                else if (way1 == Direction.South)
                {
                    movingIndex += 1;
                }
                else if (way1 == Direction.East)
                {
                    ind += 1;
                }
                else if (way1 == Direction.West)
                {
                    ind += -1;
                }
                actPipe = new Pipe(GetPipe(lines[movingIndex][ind]), movingIndex, ind);
                _graph.AddEdge(prevPipe, actPipe);


                way1 = GetNextDirection(way1, lines[movingIndex][ind]);
                if (actPipe.Type == TypeOfPipe.Start || way1 == Direction.None)
                {
                    isFinished = true;
                }
                else
                {
                    prevPipe = actPipe;
                }

            }
            _graph.InitGraph(startPipe, new List<Pipe> { startPipe });
            _cleanInput = EraseNonLoopPipe(boolGraph);
            Debug.Log("Reading Date finished");
        }

        private void SolvePart1(bool verbose = false)
        {
            Debug.Log("Solution Part 1: " + _graph.Count / 2);
        }
        private void SolvePart2(bool verbose = false)
        {
            string str = ComputeInput();
            string[] strTab = str.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i + 1 < strTab.Length; i += 2)
            {
                //FloodFill(i, 0, ref strTab);
                //FloodFill(i, strTab[i].Trim().Length -1 , ref strTab);
                FloodFillNonRec(i, 0, ref strTab);
                FloodFillNonRec(i, strTab[i].Trim().Length - 1, ref strTab);

            }
            for (int i = 0; i < strTab[0].Trim().Length; i++)
            {
                //FloodFill(0, i, ref strTab);
                //FloodFill(strTab.Length -1, i, ref strTab);
                FloodFillNonRec(0, i, ref strTab);
                FloodFillNonRec(strTab.Length - 1, i, ref strTab);

            }
            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < strTab.Length; i++)
            {
                stb.Append(strTab[i]);
            }
            string strFill = stb.ToString();
            Debug.Log("Solution Part 2: " + CountOccur(strFill, "\\.#"));

        }
        private int CountOccur(string input, string substr)
        {
            return Regex.Matches(input, substr).Count;
        }
    }
}