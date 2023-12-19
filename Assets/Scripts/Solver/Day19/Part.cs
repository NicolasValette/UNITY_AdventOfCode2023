using AdventOfCode.Datas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Solver.Day19
{
    public enum PartType
    {
        None,
        X,
        M,
        A,
        S
    }
    public enum Status
    {
        InTreatment,
        Accepted,
        Rejected
    }

    public class RangePart
    {

        public Status Status { get; private set; }
        public string CurrentWorkflow { get; private set; }
        private Dictionary<PartType, Range> _ratings;

        public Range this[PartType p]
        {
            get { return _ratings[p]; }
            set { _ratings[p] = value; }
        }
        public RangePart(Range x, Range m, Range a, Range s)
        {

            Status = Status.InTreatment;

            CurrentWorkflow = "in";
            _ratings = new Dictionary<PartType, Range>();
            _ratings[PartType.X] = x;
            _ratings[PartType.M] = m;
            _ratings[PartType.A] = a;
            _ratings[PartType.S] = s;
        }
        public void Accept()
        {
            Status = Status.Accepted;
        }
        public void Reject()
        {
            Status = Status.Rejected;
        }
        public void UpdateRange(PartType p, int min, int max)
        {
            _ratings[p].UpdateRange(min, max);
        }
        public RangePart GetNewRangePart(PartType p, int min, int max)
        {
            RangePart rp = GetNewRangePart();
            rp.UpdateRange(p, min, max);
            return rp;
        }
        public RangePart GetNewRangePart()
        {
            return new RangePart(
                new Range(_ratings[PartType.X].MinValue, _ratings[PartType.X].MaxValue),
                new Range(_ratings[PartType.M].MinValue, _ratings[PartType.M].MaxValue),
                new Range(_ratings[PartType.A].MinValue, _ratings[PartType.A].MaxValue),
                new Range(_ratings[PartType.S].MinValue, _ratings[PartType.S].MaxValue));
        }
        public long GetNumberComb ()
        {
            return _ratings[PartType.X].Count * _ratings[PartType.M].Count * _ratings[PartType.A].Count * _ratings[PartType.S].Count;
        }
      
    }

    public class Part
    {
       
        public Status Status { get; private set; }
        public string CurrentWorkflow { get; private set; }
        private Dictionary<PartType, int> _ratings;

        public int this[PartType p]
        {
            get { return _ratings[p]; }
            set { _ratings[p] = value; }
        }
        public Part(int x, int m, int a, int s)
        {
            
            Status = Status.InTreatment;
            
            CurrentWorkflow = "in";
            _ratings = new Dictionary<PartType, int>();
            _ratings[PartType.X] = x;
            _ratings[PartType.M] = m;
            _ratings[PartType.A] = a;
            _ratings[PartType.S] = s;
        }
        public void Accept()
        {
            Status = Status.Accepted;
        }
        public void Reject()
        {
            Status = Status.Rejected;
        }
        public override string ToString()
        {
            return $"part : {{x={_ratings[PartType.X]},m={_ratings[PartType.M]},a={_ratings[PartType.A]},s={_ratings[PartType.S]}}} : {Status}";
        }
        public int AddRatings ()
        {
            return _ratings[PartType.X] + _ratings[PartType.M] + _ratings[PartType.A] + _ratings[PartType.S];
        }
    }
}