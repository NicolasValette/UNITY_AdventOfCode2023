using AdventOfCode.Datas.Day20;
using AdventOfCode.Solver.Day19;
using AdventOfCode.Utility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdventOfCode.Solver.Day20
{
    public class SolverDay20 : Solver
    {

        [SerializeField]
        private int _numbPush = 1;
        private Dictionary<string, Module> _modules;
        private Dictionary<string, long> _conjectionRX;
        private List<string> _moduleToWatch;
        // Start is called before the first frame update
        void Start()
        {
            ReadData(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);
        }

        private (long,long) PushButton(long cycle, bool verbose)
        {
            long low = 0L;
            long high = 0L;
            Queue<BeamPulse> pulses = new Queue<BeamPulse>();
            pulses.Enqueue(new BeamPulse { Source = "button", Target = "button", IsHigh = false });

            while (pulses.Count > 0)
            {
                BeamPulse current = pulses.Dequeue();
                if (_modules.ContainsKey(current.Target))
                {
                      
                    foreach (var module in _modules[current.Target].Pulse(current.Source, current.IsHigh))
                    {

                        string beam = module.IsHigh ?"high":"low";
                        if (verbose) Debug.Log($"[{module.Source}] - {beam} -> {module.Target}");

                    

                        if (module.Target == "vf" && module.IsHigh && !_conjectionRX.ContainsKey(module.Source))
                        {
                            _conjectionRX.Add(module.Source, cycle);
                        }
                        if (module.IsHigh)
                        {
                            high++;
                        }
                        else
                        {
                            low++;
                        }
                        
                        pulses.Enqueue(module);
                    }
                }
                
            }
            return (low, high);
        }

        private void ReadData (bool verbose)
        {
            
            string path = Path.Combine(Application.streamingAssetsPath, _filename);
            if (_verbose) Debug.Log($"Reading file : {path}");
            StreamReader _stream = new StreamReader(path);

            _conjectionRX = new Dictionary<string, long>();
            _modules = new Dictionary<string, Module>();
            while (!_stream.EndOfStream)
            {
                string line = _stream.ReadLine();
                if (line.StartsWith("broadcaster"))
                {
                    string name = line.Split("->")[0].Trim();
                    _modules.Add(name, new BroadcasterModule(name, line.Split("->")[1].Split(',').Select(x => x.Trim()).ToList()));
                }
                else if (line.StartsWith("%"))
                {
                    string name = line.Split("->")[0].Substring(1).Trim();
                    _modules.Add(name, new FlipFlopModule(name, line.Split("->")[1].Split(',').Select(x => x.Trim()).ToList()));
                }
                else if (line.StartsWith("&"))
                {
                    string name = line.Split("->")[0].Substring(1).Trim();
                    _modules.Add(name, new ConjonctionModule(name, line.Split("->")[1].Split(',').Select(x => x.Trim()).ToList()));
                }
             
            }

            _moduleToWatch = new List<string>();
            foreach (var module in _modules.Values)
            {
                foreach (var target in module.Targets)
                {
                    if (_modules.ContainsKey(target) && _modules[target] is ConjonctionModule)
                    {
                        (_modules[target] as ConjonctionModule).AddMemory(module.Name);
                    }
                }
                if (module.Targets.Contains("con")) // the node that lead to rx
                {
                    _moduleToWatch.Add(module.Name);
                }
            }


            List<string> l = new List<string>
            {
                "broadcaster"
            };
            _modules.Add("button", new ButtonModule("button", l));
        }

        private void SolvePart1 (bool verbose)
        {
            long solH = 0L, solL = 0L;
            for (int i = 0; i< _numbPush; i++)
            {
                (long, long) n = PushButton(i, _verbose);
                solL += n.Item1;
                solH += n.Item2;
            }

            Debug.Log($"Solution Part1 : (low : {solL}, high : {solH}) - l*h : {solL * solH}");
        }
        private void SolvePart2(bool verbose)
        {
            long solH = 0L, solL = 0L;
            for (int i = 1; _conjectionRX.Keys.Count < 4; i++)
            {
                (long, long) n = PushButton(i, _verbose);
                solL += n.Item1;
                solH += n.Item2;
            }
            Debug.Log($"Solution Part2 : {MathUt.PPCM(_conjectionRX.Values.ToList())}");
        }
    }
}