using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day15
{
    public class SolverDay15 : Solver
    {
        private List<char[]> _initializationStrings;
        private Dictionary<int, OrderedDictionary> _boxes;

        // Start is called before the first frame update
        void Start()
        {
            ReadData(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);

        }
        private void ReadData(bool verbose)
        {
            string path = Path.Combine(Application.streamingAssetsPath, _filename);
            if (_verbose) Debug.Log($"Reading file : {path}");
            StreamReader _stream = new StreamReader(path);
            _input = _stream.ReadToEnd();
            string[] strings = _input.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
            _initializationStrings = new List<char[]>();
            for (int i = 0; i < strings.Length; i++)
            {
                char[] str = new char[strings[i].Trim().Length];
                str = strings[i].Trim().ToCharArray();
                _initializationStrings.Add(str);
            }
            Debug.Log("Data reading finished !");
        }
        private int ComputeValue(char c, int currentValue)
        {
            currentValue += (int)c;
            currentValue *= 17;
            currentValue %= 256;
            return currentValue;
        }
        private void SolvePart1(bool verbose)
        {
            long solution = 0L;
            for (int i = 0; i < _initializationStrings.Count; i++)
            {
                int value = 0;
                for (int j = 0; j < _initializationStrings[i].Length; j++)
                {
                    value = ComputeValue(_initializationStrings[i][j], value);
                }
                solution += value;
                if (verbose) Debug.Log($"{new string(_initializationStrings[i])} becomes {value}");
            }
            Debug.Log($"Solution Part 1 : {solution}");
        }
        private void SolvePart2(bool verbose)
        {
            long solution = 0L;
            _boxes = new Dictionary<int, OrderedDictionary>();
            for (int i = 0; i < _initializationStrings.Count; i++)
            {
                int value = 0;
                StringBuilder label = new StringBuilder();
                for (int j = 0; j < _initializationStrings[i].Length; j++)
                {
                    if (_initializationStrings[i][j] == '=')
                    {
                        OrderedDictionary box;
                        j++;
                        if (_boxes.ContainsKey(value))
                        {
                            _boxes[value][label.ToString()] = int.Parse(_initializationStrings[i][j].ToString());
                        }
                        else
                        {
                            box = new OrderedDictionary();
                            box.Add(label.ToString(), int.Parse(_initializationStrings[i][j].ToString()));
                            _boxes.Add(value, box);
                        }

                    }
                    else if (_initializationStrings[i][j] == '-')
                    {
                        if (_boxes.ContainsKey(value))
                        {
                            if (_boxes[value].Contains(label.ToString()))
                            {

                                _boxes[value].Remove(label.ToString());
                                if (_boxes[value].Count == 0)
                                {
                                    _boxes.Remove(value);
                                }
                            }
                        }
                    }
                    else
                    {
                        label.Append(_initializationStrings[i][j]);
                        value = ComputeValue(_initializationStrings[i][j], value);
                    }
                }
            }

            foreach (KeyValuePair<int, OrderedDictionary> box in _boxes)
            {
                int i = 1;
                
                IDictionaryEnumerator enume = box.Value.GetEnumerator();
                while (enume.MoveNext())
                {
                    int focusingPower = (box.Key + 1) * i * ((int)(enume.Value));
                    if (verbose) Debug.Log($"{enume.Key} in box #{box.Key}, slot #{i}, focal length of {enume.Value} = {focusingPower}");
                    solution += focusingPower;
                    i++;
                }
            }
            Debug.Log($"Solution Part 2 : {solution}");
        }

    }
}