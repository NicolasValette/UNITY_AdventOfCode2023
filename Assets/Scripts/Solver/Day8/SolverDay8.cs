using AdventOfCode.Datas;
using AdventOfCode.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;

namespace AdventOfCode.Solver.Day8
{
    public class SolverDay8 : Solver
    {
        private string _instructions;
        private Graph<string> _graph;
        private List<string> _listOfStart;
        private List<string> _listOfFinish;
        // Start is called before the first frame update
        void Start()
        {
            ReadData(_verbose);
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            if (_part1)
            {
                stopWatch.Start();

                SolvePart1(_verbose);

                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                if (_verbose) Debug.Log($"Runtime PART 1 {elapsedTime}");
            }
            stopWatch.Reset();
            if (_part2)
            {
                stopWatch.Start();

                SolvePart2(_verbose);

                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                if (_verbose) Debug.Log($"Runtime PART 2 {elapsedTime}");
            }
        }

        private void ReadData(bool verbose = false)
        {
            _graph = new Graph<string>();
            _listOfStart = new List<string>();
            _listOfFinish = new List<string>();
            StringReader reader = new StringReader(_input);
            string strread = reader.ReadLine();
            _instructions = strread;
            strread = reader.ReadLine();
            while (strread == string.Empty)
            {
                strread = reader.ReadLine();
            }
            while (strread != null && strread != string.Empty)
            {
                string[] line = strread.Split('=');

                string node = line[0].Trim();
                //Edges
                line[1] = line[1].Replace('(', ' ');
                line[1] = line[1].Replace(')', ' ');
                string[] edges = line[1].Split(",");
                if (node.EndsWith('A'))
                {
                    _listOfStart.Add(node);
                }
                if (node.EndsWith('Z'))
                {
                    _listOfFinish.Add(node);
                }
                _graph.AddEdge(node, edges[0].Trim());
                _graph.AddEdge(node, edges[1].Trim());
                strread = reader.ReadLine();
            }
            Debug.Log("Reading Data finished !");
        }
        private void SolvePart1(bool verbose = false)
        {
            List<string> finals = new List<string>
            {
                "ZZZ"
            };
            _graph.InitGraph("AAA", finals);
            if (verbose) Debug.Log(_graph.ToString());
            bool arrived = false;
            while (!arrived)
            {
                arrived = _graph.Move(_instructions);
            }
            Debug.Log("Solution Part 1 " + _graph.NumberOfMove);
        }
        private void SolvePart2(bool verbose = false)
        {
            //if (verbose) Debug.Log(_graph.ToString());
            List<long> steps = new List<long>();
            for (int i = 0; i < _listOfStart.Count; i++)
            {
                _graph.InitGraph(_listOfStart[i], _listOfFinish);
                bool arrived = false;
                while (!arrived)
                {
                    arrived = _graph.Move(_instructions);
                }
                steps.Add(_graph.NumberOfMove);
                if (verbose) Debug.Log("Solution Temp Graph "+ _listOfStart[i] + " : " + _graph.NumberOfMove);
            }
            
            //PPCM
          
            long PPCM = MathUt.PPCM(steps);
            Debug.Log("Solution : " + PPCM.ToString());
        }
    }
}


