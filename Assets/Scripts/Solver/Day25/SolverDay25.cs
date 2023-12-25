using AdventOfCode.Datas;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day25
{
    public class SolverDay25 : Solver
    {
        private UndirectedGraph<string> _graph;
        // Start is called before the first frame update
        void Start()
        {

            ReadData(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);
        }

        private string InputToGraphiz()
        {
            string path = Path.Combine(Application.streamingAssetsPath, _filename);
            if (_verbose) Debug.Log($"Reading file : {path}");
            StreamReader stream = new StreamReader(path);
            StringBuilder sb = new StringBuilder();
            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine().Split(':').Select(x => x.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)).ToList();
                string nodename = line[0][0];
                foreach (var str in line[1])
                {
                    sb.AppendLine($"{nodename} -> {str}");
                    _graph.AddEdge(nodename, str);

                }
            }
            string graphizstr = sb.ToString();
            return graphizstr;
        }
        private void ReadData(bool verbose)
        {
            _graph = new UndirectedGraph<string>();
            string str = InputToGraphiz();
            // Debug.Log(str);
            // fxk - bcf
            // xhl - shj
            // ZGP - CGT
            _graph.RemoveEdge("fxk", "bcf");
            _graph.RemoveEdge("xhl", "shj");
            _graph.RemoveEdge("zgp", "cgt");


            //test
            //_graph.RemoveEdge("hfx", "pzl");
            //_graph.RemoveEdge("bvb", "cmg");
            //_graph.RemoveEdge("nvd", "jqt");

        }
        private void SolvePart1(bool verbose)
        {
            long visited = _graph.BFS("hxf");
            //test
            //long visited = _graph.BFS("rsh");
            Debug.Log($"Part 1: {visited * (_graph.Count - visited)}");
        }
        private void SolvePart2(bool verbose)
        {
        }
    }
}