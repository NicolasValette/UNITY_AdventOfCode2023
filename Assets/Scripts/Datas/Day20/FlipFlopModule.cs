using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Datas.Day20
{
    public class FlipFlopModule : Module
    {
        private bool _isOn = false;

        public FlipFlopModule(string name, List<string> targets) : base(name, targets)
        {
        }

        public override IEnumerable<BeamPulse> Pulse(string source, bool isHigh)
        {
            if (!isHigh)
            {
                _isOn = !_isOn;
                foreach (var target in Targets)
                {
                    yield return new BeamPulse { Source = Name, Target = target, IsHigh = _isOn };
                }
                

            }
        }
    }
}