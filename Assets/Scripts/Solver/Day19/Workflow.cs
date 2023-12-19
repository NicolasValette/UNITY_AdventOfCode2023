using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Solver.Day19
{
    public class Workflow
    {
        public string Label { get; private set; }
        private List<Rule> _rules;
        public List<Rule> Rules { get { return _rules; } }
        public Workflow(string label)
        {
            Label = label;
            _rules = new List<Rule>();
        }
        public void AddRule(Rule rule)
        {
            _rules.Add(rule);
        }

       public string ComputeWorkflow(Part p)
        {
            string nextWorkflow="";
            foreach (var rule in _rules)
            {
                if (rule.ComputeRule(p, out nextWorkflow))
                    break;
            }
            return (string.IsNullOrEmpty(nextWorkflow)) ? "R" : nextWorkflow;
            
        }
        public string ComputeWorkflow(RangePart p)
        {
            string nextWorkflow = "";
            foreach (var rule in _rules)
            {
                if (rule.ComputeRule(p, out nextWorkflow)!=0)
                    break;
            }
            return (string.IsNullOrEmpty(nextWorkflow)) ? "R" : nextWorkflow;

        }
    }
}