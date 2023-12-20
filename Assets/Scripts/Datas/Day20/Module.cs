using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Datas.Day20
{
    public abstract class Module
    {
        public string Name { get; protected set; }
        public List<string> Targets {get; protected set;}

        public Module (string name, List<string> targets)
        {
            Name = name;
            Targets = new List<string>(targets);
        }
        public virtual IEnumerable<BeamPulse> Pulse(string source, bool isHigh)
        {
            foreach (var target in Targets)
            {
                yield return new BeamPulse { Source = Name, Target = target, IsHigh = isHigh };
            }
        }

    }
}