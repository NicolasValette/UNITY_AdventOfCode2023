using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace AdventOfCode.Solver.Day19
{
    public class Workflows
    {
        private Dictionary<string, Workflow> _rules;

        public Workflows()
        {
            _rules = new Dictionary<string, Workflow>();
        }
        public void AddWorkflow(Workflow wf)
        {
            if (!_rules.ContainsKey(wf.Label))
            {
                _rules.Add(wf.Label, wf);
            }
        }
        public void ComputePart(Part part)
        {
            bool isFinished = false;
            string curWF = part.CurrentWorkflow;
            while (!isFinished)
            {
                Workflow r = _rules[curWF];
                string nextWF = r.ComputeWorkflow(part);
                if (nextWF == "R")
                {
                    part.Reject();
                    isFinished = true;
                }
                if (nextWF == "A")
                {
                    part.Accept();
                    isFinished = true;
                }
                curWF = nextWF;
            }
        }
        public void ComputePart(RangePart part)
        {
            bool isFinished = false;
            string curWF = part.CurrentWorkflow;
            while (!isFinished)
            {
                Workflow r = _rules[curWF];
                string nextWF = r.ComputeWorkflow(part);
                if (nextWF == "R")
                {
                    part.Reject();
                    isFinished = true;
                }
                if (nextWF == "A")
                {
                    part.Accept();
                    isFinished = true;
                }
                curWF = nextWF;
            }
        }

        public long ComputePart2(string label, RangePart part)
        {
            long result = 0;
            switch (label)
            {
                case "A":
                    return part.GetNumberComb();
                case "R":
                    return 0;
            }

            var wf = _rules[label];

            foreach (var rule in wf.Rules)
            {
                switch (rule.Operator)
                {
                    case Operator.Greaterthan:
                        if (part[rule.Type].MinValue > rule.Value)
                        {
                            result += ComputePart2(rule.NewWorkflow, part.GetNewRangePart());
                            return result;
                        }
                        if (part[rule.Type].MaxValue > rule.Value)
                        {

                            var rangepart = part.GetNewRangePart(rule.Type, rule.Value + 1, part[rule.Type].MaxValue);
                            result += ComputePart2(rule.NewWorkflow, rangepart);
                            part.UpdateRange(rule.Type, part[rule.Type].MinValue, rule.Value);
                        }
                        break;
                    case Operator.Lesserthan:
                        if (part[rule.Type].MaxValue < rule.Value)
                        {
                            result += ComputePart2(rule.NewWorkflow, part.GetNewRangePart());
                            return result;
                        }
                        if (part[rule.Type].MinValue < rule.Value)
                        {

                            var rangepart = part.GetNewRangePart(rule.Type, part[rule.Type].MinValue, rule.Value - 1);
                            result += ComputePart2(rule.NewWorkflow, rangepart);
                            part.UpdateRange(rule.Type, rule.Value, part[rule.Type].MaxValue);
                        }
                        break;
                    default:
                        // No conditions - recursively process the next workflow.
                        result += ComputePart2(rule.NewWorkflow, part.GetNewRangePart());
                        break;
                }
               
            }
            return result;
        }
    }

}