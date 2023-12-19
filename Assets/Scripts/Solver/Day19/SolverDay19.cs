using AdventOfCode.Datas;
using AdventOfCode.Solver.Day18;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace AdventOfCode.Solver.Day19
{
    public class SolverDay19 : Solver
    {
        private Workflows _workflows;
        private List<Part> _parts;
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

            _workflows = new Workflows();
            _parts = new List<Part>();
            //Rules

           
            while (!_stream.EndOfStream)
            {
                string label, strPart, next;
                int ratings=0;
                Operator op = Operator.R;
                PartType type = PartType.None;
                string line = _stream.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }
                string[] str = line.Split('{');
                label = str[0];
                line = str[1].TrimEnd('}');
                string[] rules = line.Split(',');
                Workflow wf = new(label);
                for (int i =0; i < rules.Length;i++)
                {
                    Rule r;
                    if (rules[i] == "R")
                    {
                        r = new Rule(PartType.None, "R", Operator.R, 0);
                    }
                    else if (rules[i] == "A")
                    {
                        r = new Rule(PartType.None, "A", Operator.A, 0);
                    }
                    else if (rules[i].Contains(">") || rules[i].Contains("<"))
                    { //ex{x>10:one,m<20:two,a>30:R,A}
                        string[] rulepart = rules[i].Split(":");
                        string t = "";
                        if (rulepart[0].Contains("<"))
                        {
                            strPart = rulepart[0].Split('<')[0];
                            ratings = int.Parse(rulepart[0].Split('<')[1]);
                            t = rulepart[0].Split('<')[0];
                            
                            op = Operator.Lesserthan;
                        }
                        else if (rulepart[0].Contains(">"))
                        {
                            strPart = rulepart[0].Split('>')[0];
                            ratings = int.Parse(rulepart[0].Split('>')[1]);
                            t = rulepart[0].Split('>')[0];
                            op = Operator.Greaterthan;
                        }
                        if (t == "x")
                        {
                            type = PartType.X;
                        }
                        if (t == "m")
                        {
                            type = PartType.M;
                        }
                        if (t == "a")
                        {
                            type = PartType.A;
                        }
                        if (t == "s")
                        {
                            type = PartType.S;
                        }
                        next = rulepart[1];
                        r = new Rule(type, next, op, ratings);

                    }
                    else
                    {
                        r = new Rule(PartType.None, rules[i], Operator.None, 0);
                    }
                    wf.AddRule(r);
                }
                _workflows.AddWorkflow(wf);
            }

            // Parts
            while (!_stream.EndOfStream)
            {
                string line = _stream.ReadLine();
                line = line.TrimStart('{');
                line = line.TrimEnd('}');
                string[] str = line.Split(',');
                Part p = new Part(
                    int.Parse(str[0].Split('=')[1]),
                    int.Parse(str[1].Split('=')[1]),
                    int.Parse(str[2].Split('=')[1]),
                    int.Parse(str[3].Split('=')[1]));
                _parts.Add(p);
            }
            Debug.Log("Finish");
        }
        private void SolvePart1(bool verbose)
        {
            foreach(Part part in _parts)
            {
                _workflows.ComputePart(part);
                Debug.Log(part.ToString());
            }
            long solution = _parts.Where(x => x.Status == Status.Accepted).Sum(x => x.AddRatings());
            Debug.Log($"Solution Part1 : {solution}");
        }
        private void SolvePart2(bool verbose)
        {
            RangePart rangePart = new RangePart(new Range(1, 4000), new Range(1, 4000), new Range(1, 4000), new Range(1, 4000));
            long solution = _workflows.ComputePart2("in", rangePart);
            
            Debug.Log($"Solution Part2 : {solution}");
        }

    }
}
