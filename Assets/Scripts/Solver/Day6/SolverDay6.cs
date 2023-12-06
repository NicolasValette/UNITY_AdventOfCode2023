using AdventOfCode.Solver.Day5;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using System.Linq;

namespace AdventOfCode.Solver.Day6
{

    public class RaceResult
    {
        public long Dist { get; set; }
        public long ButtonTime { get; set; }
    }
        public class SolverDay6 : MonoBehaviour
    {

        [TextArea(17, 1000)]
        [SerializeField]
        private string _input;

        private List<long> _raceTimeLimit = new List<long>();
        private List<long> _raceDistance = new List<long>();

        private long _timeLimitePart2;
        private long _racedistancePart2;
        // Start is called before the first frame update
        void Start()
        {
            ReadData();
            SolvePart1();
            SolvePart2();
        }

        private void ReadData()
        {
            StringReader reader = new StringReader(_input);
            string strread = reader.ReadLine();
            string[] time = strread.Split(':')[1].Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            _timeLimitePart2 = long.Parse(String.Concat(strread.Split(':')[1].Where(c => !Char.IsWhiteSpace(c))));
            for (int i = 0; i < time.Length; i++)
            {
                _raceTimeLimit.Add(long.Parse(time[i]));
            }

            strread = reader.ReadLine();
            string[] distance = strread.Split(':')[1].Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            _racedistancePart2 = long.Parse(String.Concat(strread.Split(':')[1].Where(c => !Char.IsWhiteSpace(c))));
            for (int i = 0; i < distance.Length; i++)
            {
                _raceDistance.Add(long.Parse(distance[i]));
            }
            Debug.Log("Reading data complete");
        }

        private List<RaceResult> GetBestDistance(long raceDistance, long timeLimite)
        {
            List<RaceResult> results = new List<RaceResult>();
            for (int i=0; i < timeLimite; i++)
            {
                RaceResult r = new RaceResult();
                long speed = i;
                long dist = (timeLimite - i) * speed;
                if (dist > raceDistance)
                {
                    r.Dist = dist;
                    r.ButtonTime = i;
                    results.Add(r);
                }
            }
            return results;
        }
        private void SolvePart1()
        {
            long solution = 1;
            for (int i =0; i < _raceDistance.Count; i++)
            {
                List<RaceResult> results = GetBestDistance(_raceDistance[i], _raceTimeLimit[i]);
                solution *= results.Count;
                Debug.Log($"For the Race {i + 1}, we get {results.Count} ways to win");
            }
            Debug.Log($"Solution : {solution}");
        }
        private void SolvePart2()
        {
            List<RaceResult> results = GetBestDistance(_racedistancePart2, _timeLimitePart2);
            Debug.Log($"Solution Part 2 : {results.Count}");
        }

    }
}