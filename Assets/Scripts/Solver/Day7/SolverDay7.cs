using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;


namespace AdventOfCode.Solver.Day7
{
    public class SolverDay7 : Solver
    {


        private List<Hand> _hands = new List<Hand>();
        // Start is called before the first frame update
        void Start()
        {
            ReadDatas(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);
        }
        private void ReadDatas(bool verbose = false)
        {
            StringReader reader = new StringReader(_input);
            string strread = reader.ReadLine();
            while (strread != null && strread != string.Empty)
            {
                string[] line = strread.Split(' ');
                Hand hand = new Hand(line[0].Trim(), long.Parse(line[1].Trim()));
                _hands.Add(hand);
                strread = reader.ReadLine();
                //hand.Sort();
                if (verbose) Debug.Log(hand.ToString());
            }
            Debug.Log("Reading Data Complete");


        }
        private void SolvePart1(bool verbose = false)
        {
            if (verbose) Debug.Log("Start sorting");
            _hands.Sort();
            long solution = 0;
            for (int i = 0; i < _hands.Count; i++)
            {
                solution += (i + 1) * _hands[i].Bid;
            }
            Debug.Log("Solution Part 1 : " + solution);
        }
        private void SolvePart2(bool verbose = false)
        {
            if (verbose) Debug.Log("Start sorting");
            for (int i = 0; i < _hands.Count; i++)
            {
                _hands[i].Part2 = true;
            }
            _hands.Sort();
            long solution = 0;
            for (int i = 0; i < _hands.Count; i++)
            {
                if (verbose) Debug.Log(_hands[i].ToString());
                solution += (i + 1) * _hands[i].Bid;
            }

            Debug.Log("Solution Part 2 : " + solution);
        }
    }
}