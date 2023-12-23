using AdventOfCode.Datas;
using AdventOfCode.Solver.Day19;
using AdventOfCode.Utility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace AdventOfCode.Solver.Day23
{
    public interface IMeasurable
    {
        public int GetLength();
    }
    public class Node : IMeasurable
    {
        public IntCoords Point { get; private set; }
        public int Lenght { get; set; }

        public Node (int x, int y, int l)
        {
            Point = new IntCoords (x, y);
            Lenght = l;
        }
        public int GetLength() => Lenght;
    }
    public class SolverDay23 : Solver
    {
        private Graph<IntCoords> _graph;
        private Graph<Node> _graphPart2;
        private IntCoords _start;
        private IntCoords _end;
        // Start is called before the first frame update
        void Start()
        {
            ReadData(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);

        }
        private int GetNumberCrossreads(string[] input, int i, int j)
        {
            int crossroad = 0;
            if (input[i - 1][j] != '#')
            {
                crossroad++;
            }
            if ( input[i + 1][j] != '#')
            {
                crossroad++;
            }
            if (input[i][j - 1] != '#')
            {
                crossroad++;
            }
            if (input[i][j + 1] != '#')
            {
                crossroad++;
            }
            return crossroad;
        }
        private (Node StartingNode, IntCoords ArrivingPoint) FindPath(string[] input, int i, int j, bool[,] visited)
        {
            int pathLength = 0;
            Node node = new Node(i, j, 0);
            while (GetNumberCrossreads(input, i, j) == 2)
            {
                visited[i, j] = true;

                if (!visited[i - 1, j] && input[i - 1][j] != '#')
                {
                    i--;
                    pathLength++;
                }
                else if (!visited[i + 1, j] && input[i + 1][j] != '#')
                {
                    i++;
                    pathLength++;
                }
                else if (!visited[i, j - 1] && input[i][j - 1] != '#')
                {
                    j--;
                    pathLength++;
                }
                else if (!visited[i, j + 1] && input[i][j + 1] != '#')
                {
                    j++;
                    pathLength++;
                }
            }
            node.Lenght = pathLength;
            return (node, new IntCoords(i, j));
        }
        private void ReadDataPart2(bool verbose)
        {
            string path = Path.Combine(Application.streamingAssetsPath, _filename);
            if (_verbose) Debug.Log($"Reading file : {path}");
            StreamReader stream = new StreamReader(path);
            string[] lines = stream.ReadToEnd().Split('\n');
            _graphPart2 = new Graph<Node>();
            Queue<IntCoords> queue = new Queue<IntCoords>();
            queue.Enqueue(new IntCoords(0, 1));
            bool[,] visited = new bool[lines.Length, lines[0].Trim().Length];
            while (queue.TryDequeue(out IntCoords coord)) 
            {
                var value = FindPath(lines, coord.X, coord.Y, visited).StartingNode;
               // _graphPart2.AddEdge
            }

        }
        private void ReadData(bool verbose)
        {
            string path = Path.Combine(Application.streamingAssetsPath, _filename);
            if (_verbose) Debug.Log($"Reading file : {path}");
            StreamReader stream = new StreamReader(path);
            string[] lines = stream.ReadToEnd().Split('\n');
            _graph = new Graph<IntCoords>();
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Trim().Length; j++)
                {
                    if (lines[i][j] == '#')
                    {
                        continue;
                    }
                    IntCoords point = new IntCoords(i, j);
                    if (i == 0)
                    {
                        _start = point;             //the only . char on first row is always the start
                    }
                    if (i == lines.Length - 1)
                    {
                        _end = point;             //the only . char on last row is always the start
                    }

                    if (i - 1 >= 0 && (lines[i - 1][j] == '.'))
                    {
                        _graph.AddEdge(point, new IntCoords(i - 1, j));
                    }
                    if (i + 1 < lines.Length && (lines[i + 1][j] == '.' || lines[i + 1][j] == 'v'))
                    {
                        _graph.AddEdge(point, new IntCoords(i + 1, j));
                    }
                    if (j - 1 >= 0 && (lines[i][j - 1] == '.' || lines[i][j - 1] == '<'))
                    {
                        _graph.AddEdge(point, new IntCoords(i, j - 1));
                    }
                    if (j + 1 < lines[i].Trim().Length && (lines[i][j + 1] == '.' || lines[i][j + 1] == '>'))
                    {
                        _graph.AddEdge(point, new IntCoords(i, j + 1));
                    }
                }
            }
            _graph.InitStartingNode(_start);
            _graph.AddFinalNode(_end);
            //_graphPart2.InitStartingNode(_start);
            //_graphPart2.AddFinalNode(_end);
            if (verbose) Debug.Log($"Start : {_start}, End : {_end}");
            if (verbose) Debug.Log($"Start : {_graph.Starting}, End : {_graph.Final[0]}");
            if (verbose) Debug.Log("Finish");
        }
        private void SolvePart1(bool verbose)
        {

            //  int path = _graph.FindLongestPath(_start, new HashSet<string>(), verbose);
            var list = _graph.FindLongestPathNonRec(new HashSet<string>());
            //Debug.Log($"Solution Part 1 {path}");
        }
        private void SolvePart2(bool verbose)
        {
            //int path = _graphPart2.FindLongestPath(_start, new HashSet<string>(), verbose);
            //Debug.Log($"Solution Part 2 {path}");
        }
    }
}