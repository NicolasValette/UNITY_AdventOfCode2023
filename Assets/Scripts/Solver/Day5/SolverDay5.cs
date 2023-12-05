using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;


namespace AdventOfCode.Solver.Day5
{
    public class Range
    {
        public long StartDestination { get; private set; }
        public long StartSource { get; private set; }
        public long TotalRange { get; private set; }

        public Range (long startDestination, long startSource, long range)
        {
            StartDestination = startDestination;
            StartSource = startSource;
            TotalRange = range;
        }
      
    }

    public class Maps
    {
        private List<Range> _ranges = new List<Range>();

        public void AddRange (long start, long source, long range)
        {
            Range r = new Range(start, source, range);
            _ranges.Add(r);
        }
        public long GetDestination(long from)
        {
            for (int i=0; i< _ranges.Count; i++)
            {
                if ( from >= _ranges[i].StartSource && from <= (_ranges[i].StartSource + _ranges[i].TotalRange))
                {
                    long offset = from - _ranges[i].StartSource;
                    return _ranges[i].StartDestination + offset;
                }
            }
            return from;
        }

    }

    public class SolverDay5 : MonoBehaviour
    {
        private enum MapType
        {
            soil,
            fertilizer,
            water,
            light,
            temperature,
            humidity,
            location
        }

        [TextArea(17, 1000)]
        [SerializeField]
        private string _input;

        private Maps _soil = new Maps();
        private Maps _fertilizer = new Maps();
        private Maps _water = new Maps();
        private Maps _light = new Maps();
        private Maps _temperature = new Maps();
        private Maps _humidity = new Maps();
        private Maps _location = new Maps();

        private List<long> _seeds = new List<long>();

        // Start is called before the first frame update
        void Start()
        {
            ReadData();
            SolvePart1();
            //SolvePart2();
            //count();
        }

        private void ReadMap(StringReader reader, ref Maps map)
        {
            string str = reader.ReadLine();
            while (str != string.Empty && str != null)
            {
                long destinationStart = 0,
                    sourceStart = 0,
                    range = 0;
                string[] rangestr = str.Trim().Split(" ");
                destinationStart = long.Parse(rangestr[0].Trim());
                sourceStart = long.Parse(rangestr[1].Trim());
                range = long.Parse(rangestr[2].Trim());
                map.AddRange(destinationStart, sourceStart, range);

                str = reader.ReadLine();
            }

        }
        // the destination range start, the source range start, and the range length.
        private void ReadData()
        {
            StringReader reader = new StringReader(_input);
            string strread = reader.ReadLine();
            string[] seeds = strread.Split(':')[1].Trim().Split(' ');
            for (int i = 0; i < seeds.Length; i++)
            {
                _seeds.Add(long.Parse(seeds[i]));
            }
            Debug.Log("Number of seeds : " + _seeds.Count);
            strread = reader.ReadLine();
            while (strread != null)
            {
                if (strread.Contains("seed-to-soil"))
                {
                    ReadMap(reader, ref _soil);
                }
                else if (strread.Contains("soil-to-fertilizer"))
                {
                    ReadMap(reader, ref _fertilizer);
                }
                else if (strread.Contains("fertilizer-to-water"))
                {
                    ReadMap(reader, ref _water);
                }
                else if (strread.Contains("water-to-light"))
                {
                    ReadMap(reader, ref _light);
                }
                else if (strread.Contains("light-to-temperature"))
                {
                    ReadMap(reader, ref _temperature);
                }
                else if (strread.Contains("temperature-to-humidity"))
                {
                    ReadMap(reader, ref _humidity);
                }
                else if (strread.Contains("humidity-to-location"))
                {
                    ReadMap(reader, ref _location);
                }
                strread = reader.ReadLine();
            }
            Debug.Log("Reading finished");
        }

        private long GetLocation (long seed)
        {
            long value = 0;
            StringBuilder strbld = new StringBuilder();
            strbld.Append("Seeds " + seed + ", ");
            //seed to soil
            value = _soil.GetDestination(seed);
            strbld.Append($"soil {value}, ");
            //soil to fertilizer
            value = _fertilizer.GetDestination(value);
            strbld.Append($"fertilizer {value}, ");
            //fertilizer to water
            value = _water.GetDestination(value);
            strbld.Append($"water {value}, ");
            //water to light
            value = _light.GetDestination(value);
            strbld.Append($"light {value}, ");
            //light to temp
            value = _temperature.GetDestination(value);
            strbld.Append($"temperature {value}, ");
            //temp to humidity
            value = _humidity.GetDestination(value);
            strbld.Append($"humidity {value}, ");
            //humidity to location
            value = _location.GetDestination(value);
            strbld.Append($"location {value}.");
            Debug.Log(strbld.ToString());
            return value;
        }
        private void SolvePart1()
        {
            List<long> location = new List<long>();
            long solution = 0;
            StringBuilder strbld = new StringBuilder();
            for (int i = 0; i < _seeds.Count; i++)
            {
                long value = GetLocation(_seeds[i]);

                location.Add(value);
                if (solution == 0 || value < solution)
                {
                    solution = value;
                }
            }
            
            Debug.Log("Solution Part 1 : " + solution);
        }
        private void SolvePart2()
        {
            long solution = 0;
            for (int i=0; i+1< _seeds.Count;i = i+2) 
            {
                for (long j = 0; j < _seeds[i + 1]; j++)
                {
                    long value = GetLocation(_seeds[i] + j);

                    if (solution == 0 || value < solution)
                    {
                        solution = value;
                    }
                }
            }
            Debug.Log("Solution Part 2: " + solution);
        }

        private void count()
        {
            long numb1 = 0, numb2 = 0;
            for (int i = 0; i + 1 < _seeds.Count; i = i + 2)
            {
                for (long j = 0; j < _seeds[i + 1]; j++)
                {
                    numb1++;
                    if (IsFirst(_seeds[i] + j, i))
                    {
                        numb2++;
                    }
                }
            }
            Debug.Log("Numbre 1 : " + numb1 + " nombre 2 : " + numb2);
        }
        private bool IsFirst(long number, int seed)
        {
            for (int i = 0; i + 1 < seed; i = i + 2)
            {
                if (number >= _seeds[seed] && number <= (_seeds[seed] + _seeds[i+1]))
                {
                    return false;
                }
            }
            return true;
        }
    
    }
}