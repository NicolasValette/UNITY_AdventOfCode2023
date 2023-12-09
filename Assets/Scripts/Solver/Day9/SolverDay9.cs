using Mono.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day9
{
    public class SolverDay9 : Solver
    {
        private List<List<long>> _history = new List<List<long>>();

        // Start is called before the first frame update
        void Start()
        {
            ReadDatas(_verbose);
            Solve(_part1, _part2, _verbose);
        }

        private void ReadDatas(bool verbose = false)
        {
            StringReader reader = new StringReader(_input);
            string strread = reader.ReadLine();
            int indWhereToAdd = 0;
            while (strread != null && strread != string.Empty)
            {
                _history.Add(new List<long>());
                string[] numbers = strread.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                List<long> numbersList = new List<long>();
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbersList.Add(int.Parse(numbers[i]));
                }
                _history[indWhereToAdd] = numbersList;
                indWhereToAdd++;
                strread = reader.ReadLine();
            }
            Debug.Log("Reading Data finished !");
        }
        private void Solve(bool part1, bool part2 ,bool verbose = false) 
        {
            if (verbose) Debug.Log("Starting Solving Part1");
            long solution1 = 0, solution2=0;
            for (int i=0; i<_history.Count;i++)
            {
                bool isFinished = false;
                List<List<long>> workingSample = new List<List<long>>();
                workingSample.Add(new List<long>(_history[i]));
                List<long> workingList = new List<long>(_history[i]);
                StringBuilder sb = new StringBuilder();
                sb.Append("temp list :");
                while (!isFinished)
                {
                    List<long> tempList = new List<long>();
                    isFinished = true;
                    for (int j = 0; j + 1 < workingList.Count; j++)
                    {
                        tempList.Add(workingList[j + 1] - workingList[j]);
                        if (tempList[j] != 0)
                        {
                            isFinished = false;
                        }
                        sb.Append($" {tempList[j]}");
                    }
                    if (verbose) Debug.Log(sb.ToString());
                    workingSample.Add(new List<long>(tempList));
                    workingList.Clear();
                    workingList = new List<long>(tempList);
                    tempList.Clear();
                }
                // Extrapolate
                if (part1)
                {
                    workingSample[workingSample.Count - 1].Add(0);
                    for (int j = workingSample.Count - 2; j >= 0; j--)
                    {
                        workingSample[j].Add(workingSample[j][workingSample[j].Count - 1] + workingSample[j + 1][workingSample[j + 1].Count - 1]);
                    }
                    long tempsol = workingSample[0][workingSample[0].Count - 1];
                    if (verbose) Debug.Log($"Solution sample #{i + 1} : {tempsol}");
                    solution1 += tempsol;
                }
                //extrapolate backward
                if (part2)
                {
                    workingSample[workingSample.Count - 1].Insert(0, 0);
                    for (int j = workingSample.Count - 2; j >= 0; j--)
                    {
                        workingSample[j].Insert(0, workingSample[j][0] - workingSample[j + 1][0]);
                    }
                    long tempsol = workingSample[0][0];
                    if (verbose) Debug.Log($"Solution sample #{i + 1} : {tempsol}");
                    solution2 += tempsol;
                }
            }
            if (part1) Debug.Log("Solution Part 1 : " + solution1);
            if (part2) Debug.Log("Solution Part 2 : " + solution2);
        }
     
    }
}