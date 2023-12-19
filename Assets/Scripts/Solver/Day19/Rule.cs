using AdventOfCode.Datas;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AdventOfCode.Solver.Day19
{
    public enum Operator
    {
        None,
        R,
        A,
        Greaterthan,
        Lesserthan
    }

    public class Rule
    {
        private PartType _partType;
        public PartType Type {get { return _partType;}}
        public string NewWorkflow { get; private set; }
        public Operator Operator { get; private set; }
        public int Value { get; private set; }
        public Rule(PartType p, string newWorkflow, Operator op, int value)
        {
            _partType = p;
            NewWorkflow = newWorkflow;
            Operator = op;
            Value = value;
        }
        public bool ComputeRule(Part part, out string next)
        {
            next = "R";
            switch (Operator)
            {
                case Operator.Greaterthan:
                    if (part[_partType] > Value)
                    {
                        next = NewWorkflow;
                        return true;
                    }
                    else
                    {
                        next = "R";
                        return false;
                    }
                case Operator.Lesserthan:
                    if (part[_partType] < Value)
                    {
                        next = NewWorkflow;
                        return true;
                    }
                    else
                    {
                        next = "R";
                        return false;
                    }
                case Operator.A:
                    next = "A";
                    return true;
                case Operator.R:
                    next = "R"; 
                    return false;
                case Operator.None:
                    next = NewWorkflow;
                    return true;

            }
            return false;
        }
        public long ComputeRule(RangePart part, out string next)
        {
            next = "R";
            switch (Operator)
            {
                case Operator.Greaterthan:
                    Range r = part[_partType];


                    if (part[_partType].MinValue > Value)
                    {
                        next = NewWorkflow;
                        return 0L;
                    }
                    else if (part[_partType].MaxValue < Value)
                    {
                        next = "R";
                        return 0L;
                    }
                    else
                    {
                        part[_partType].UpdateRange(Value + 1, part[_partType].MaxValue);
                        next = NewWorkflow;
                        return 0L;
                    }
                case Operator.Lesserthan:
                    if (part[_partType].MaxValue < Value)
                    {
                        next = NewWorkflow;
                        return 0L;
                    }
                    else if (part[_partType].MinValue > Value)
                    {
                        next = "R";
                        return 0L;
                    }
                    else
                    {
                        part[_partType].UpdateRange(part[_partType].MinValue, Value - 1);
                        next = NewWorkflow;
                        return 0L;
                    }
                case Operator.A:
                    next = "A";
                    return part.GetNumberComb();
                //case Operator.R:
                //    next = "R";
                //    return false;
                //case Operator.None:
                //    next = NewWorkflow;
                //    return true;

            }
            //return false;
            return 0L;
        }

    }
}