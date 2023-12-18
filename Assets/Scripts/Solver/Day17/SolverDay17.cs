using AdventOfCode.Algorithms;
using AdventOfCode.Datas;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdventOfCode.Solver.Day17
{
    public class SolverDay17 : Solver
    {
        private UndirectedGraph<CityBlock> _graph;
        CityBlock _start;
        CityBlock _finish;
        // Start is called before the first frame update
        void Start()
        {
            ReadData(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);

        }
        private void ReadData(bool verbose)
        {
            CityBlock currentBlock;
            string path = Path.Combine(Application.streamingAssetsPath, _filename);
            if (_verbose) Debug.Log($"Reading file : {path}");
            StreamReader _stream = new StreamReader(path);
            _input = _stream.ReadToEnd();
            string[] lines = _input.Split('\n');

            _graph = new UndirectedGraph<CityBlock>();
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] >= '0' && lines[i][j] <= '9')
                    {
                        currentBlock = new CityBlock(i, j, int.Parse(lines[i][j].ToString()));
                        if (i - 1 >= 0)
                        {
                            _graph.AddEdge(currentBlock, new CityBlock(i - 1, j, int.Parse(lines[i - 1][j].ToString())), true);
                        }
                        if (j - 1 >= 0)
                        {
                            _graph.AddEdge(currentBlock, new CityBlock(i, j-1, int.Parse(lines[i][j-1].ToString())), true);
                        }
                    }
                }
            }
            List<string> nodes = _graph.NodesLabel;
            _start = new CityBlock(0, 0, int.Parse(lines[0][0].ToString()));
            _finish = new CityBlock(lines.Length - 1, lines[lines.Length-1].Trim().Length-1, int.Parse(lines[lines.Length-1][lines[lines.Length - 1].Trim().Length-1].ToString()));
            Debug.Log("Data reading finished!");
        }
        private void SolvePart1(bool verbose)
        {

            DijkstraDay17 dijkstra = new DijkstraDay17(_graph, _start, _finish);
            Stack <CityBlock> a = dijkstra.Run();
            Debug.Log("Fini");
        }
        private void SolvePart2(bool verbose)
        {

        }
    }
}