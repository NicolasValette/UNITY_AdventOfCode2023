using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdventOfCode.Datas.Day20
{
    public class ConjonctionModule : Module
    {
        //Dictionnary, keep memory of all beam, init to false (=low)
        private Dictionary<string, bool> _memory = new Dictionary<string, bool>();

        public ConjonctionModule(string name, List<string> targets) : base(name, targets)
        {
            
        }

        public void AddMemory (string name)
        {
            _memory.Add(name, false);
        }
        public override IEnumerable<BeamPulse> Pulse(string source, bool isHigh)
        {
            foreach (var target in Targets)
            {
                _memory[source] = isHigh;
                if (_memory.Values.All(x => (x == true)))
                {
                    yield return new BeamPulse { Source = Name, Target = target, IsHigh = false };
                }
                else
                {
                    yield return new BeamPulse { Source = Name, Target = target, IsHigh = true };
                }
            }
        }
    }
}