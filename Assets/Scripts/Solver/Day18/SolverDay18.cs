using AdventOfCode.Utility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day18
{
    public class Cell
    {
        public int Steps { get; private set; }
        public Direction Direction { get; private set; }
        
        public string Color { get; private set; }
        public char Char { get; set; }
        public int I
        {
            get
            {
                if (Direction == Direction.North)
                {
                    return -Steps;
                }
                if (Direction == Direction.South)
                {
                    return Steps;
                }
                else
                {
                    return 0;
                }
            }
        }
        public int J
        {
            get
            {
                if (Direction == Direction.West)
                {
                    return -Steps;
                }
                if (Direction == Direction.East)
                {
                    return Steps;
                }
                else
                {
                    return 0;
                }
            }
        }
        public Cell (int step, string color, char character, Direction direction)
        {
            Steps = step;
            Color = color;
            Char = character;
            Direction = direction;
        }
        public Cell(int step, char character, Direction direction)
        {
            Steps = step;
            Char = character;
            Color = "";
            Direction = direction;
        }
        public override string ToString()
        {
            //return $"<color={Color}>{Char}</color>";
            return Char.ToString();
        }
    }
    public class SolverDay18 : Solver
    {
        private string[,] _strtab;
        private long _iLength;
        private long _jLength;

        private string[,] _strtab2;
        private long _iLength2;
        private long _jLength2;
        private long _boundaries = 0L;
        private long _boundaries2 = 0L;
        private Queue<(long, long)> _queueShoelace1;
        private Queue<(long, long)> _queueShoelace2;
        // Start is called before the first frame update
        void Start()
        {
            ReadData(_verbose);
            if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);

        }


  
        private void ReadData (bool verbose)
        {
            string path = Path.Combine(Application.streamingAssetsPath, _filename);
            if (_verbose) Debug.Log($"Reading file : {path}");
            StreamReader _stream = new StreamReader(path);
            long imax = 0, imin = 0, jmax = 0, jmin = 0, i=0, j=0;
            long imax2 = 0, imin2 = 0, jmax2 = 0, jmin2 = 0, i2 = 0, j2 = 0;
            Queue<Cell> queue = new Queue<Cell>();
            _queueShoelace1 = new Queue<(long,long)>();
            _queueShoelace2 = new Queue<(long, long)>();
            while (!_stream.EndOfStream)
            {
                string line = _stream.ReadLine();
                string[] str = line.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
                string dir = str[0];
                int steps = int.Parse(str[1]);
                string color = str[2].Trim('(');
                color = color.Trim(')');
                Direction direction = Direction.None;
                if (dir == "R")
                {
                    j += steps;
                    direction = Direction.East;
                }
                else if (dir == "L")
                {
                    j -= steps;
                    direction = Direction.West;
                }
                else if (dir == "U")
                {
                    i -= steps;
                    direction = Direction.North;
                }
                else if (dir == "D")
                {
                    i+= steps;
                    direction = Direction.South;
                }
                imax = (i > imax) ? i : imax;
                imin = (i < imin) ? i : imin;
                jmax = (j > jmax) ? j : jmax;
                jmin = (j < jmin) ? j : jmin;
                Cell c = new Cell(steps, color, '#', direction);
                _queueShoelace1.Enqueue((i, j));
                _boundaries += steps;
               queue.Enqueue(c);

                //Part2
                string color2 = color.Trim('#');
                string step2 = color2.Remove(5);
                string dir2 = color.Substring(6);
                long n = long.Parse(step2, System.Globalization.NumberStyles.HexNumber);
                if (dir2 == "0")
                {
                    i2 += n;
                    direction = Direction.East;
                }
                else if (dir2 == "2")
                {
                    i2 -= n;
                    direction = Direction.West;
                }
                else if (dir2 == "3")
                {
                    j2 -= n;
                    direction = Direction.North;
                }
                else if (dir2 == "1")
                {
                    j2 += n;
                    direction = Direction.South;
                }
                imax2 = (i2 > imax2) ? i2 : imax2;
                imin2 = (i2 < imin2) ? i2 : imin2;
                jmax2 = (j2 > jmax2) ? j2 : jmax2;
                jmin2 = (j2 < jmin2) ? j2 : jmin2;
                _boundaries2 += n;
                _queueShoelace2.Enqueue((i2,j2)) ;
            }
            //ComputeCharTab(imax, imin, jmax, jmin, queue);
            //printGraph();
        
            Debug.Log("fin");
        }

        private long ShoelaceFormula (Queue<(long,long)> coords)
        {
            long solution = 0L;
            var first = coords.Dequeue();
            Debug.Log($"first : ({first.Item1};{first.Item2})");
            var old = first;
            long tmp;
            while (coords.Count> 0)
            {
                var coord = coords.Dequeue();
                //(prev_x + x) * (prev_y - y);
                //solution += (old.Item1 * coord.Item2) - (coord.Item1 * old.Item2);
                solution += (old.Item1 + coord.Item1) * (old.Item2 - coord.Item2);
                tmp = (old.Item1 + coord.Item1) * (old.Item2 - coord.Item2);
                Debug.Log($"Coord : ({coord.Item1};{coord.Item2})\n({old.Item1} + {coord.Item1}) * ({old.Item2} - {coord.Item2}) = {tmp}");
                old = coord;
            }
            //solution += (old.Item1 * first.Item2) - (first.Item1 * old.Item2);
            //solution += (old.Item1 + first.Item1) * (old.Item2 - first.Item2);
            //tmp = (old.Item1 + first.Item1) * (old.Item2 - first.Item2);
            //Debug.Log($"({old.Item1} + {first.Item2}) * ({first.Item1} - {old.Item2}) = {tmp}");
            return (long)(Mathf.Abs(solution)/2L);
        }
    
        //private void ComputeCharTab2(int imax, int imin, int jmax, int jmin, Queue<Cell> queue)
        //{
        //    _iLength2 = imax + Mathf.Abs(imin) + 1;
        //    _jLength2 = jmax + Mathf.Abs(jmin) + 1;
        //    _strtab2 = new string[_iLength2, _jLength2];
        //    int i = Mathf.Abs(imin);
        //    int j = Mathf.Abs(jmin);
        //    while (queue.Count > 0)
        //    {
        //        Cell c = queue.Dequeue();
        //        if (c.Direction == Direction.North)
        //        {
        //            for (int k = 1; k < c.Steps + 1; k++)
        //            {
        //                _strtab2[i, j] = c.ToString();
        //                i--;
        //            }
        //        }
        //        else if (c.Direction == Direction.South)
        //        {
        //            for (int k = 1; k < c.Steps + 1; k++)
        //            {
        //                _strtab2[i, j] = c.ToString();
        //                i++;
        //            }
        //        }
        //        if (c.Direction == Direction.East)
        //        {
        //            for (int k = 1; k < c.Steps + 1; k++)
        //            {
        //                _strtab2[i, j] = c.ToString();
        //                j++;
        //            }
        //        }
        //        if (c.Direction == Direction.West)
        //        {
        //            for (int k = 1; k < c.Steps + 1; k++)
        //            {
        //                _strtab2[i, j] = c.ToString();
        //                j--;
        //            }
        //        }
        //    }
        //}
        //private void printGraph2()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < _iLength2; i++)
        //    {
        //        StringBuilder sbline = new StringBuilder();
        //        for (int j = 0; j < _jLength2; j++)
        //        {
        //            if (string.IsNullOrEmpty(_strtab2[i, j]))
        //            {
        //                _strtab[i, j] = ".";
        //            }
        //            sbline.Append(_strtab2[i, j]);
        //        }
        //        sb.AppendLine(sbline.ToString());
        //    }
        //    string str = sb.ToString();
        //    Debug.LogFormat("print");
        //}




        private float Solve (Queue <(long,long)> queue, long boundaries)
        {
            float A = ShoelaceFormula(queue);
            var i = A - boundaries / 2 + 1;
            Debug.Log($"A : {(long)A}; i:{(long)i}, b : {boundaries} ");
            return i + boundaries;
        }




        //private void ComputeCharTab(long imax, long imin, long jmax, long jmin, Queue<Cell> queue)
        //{
        //    _iLength = imax + Mathf.Abs(imin) + 1L;
        //    _jLength = jmax + Mathf.Abs(jmin) + 1;
        //    _strtab = new string[_iLength, _jLength];
        //    int i = Mathf.Abs(imin);
        //    int j = Mathf.Abs(jmin);
        //    while (queue.Count > 0)
        //    {
        //        Cell c = queue.Dequeue();
        //        if (c.Direction == Direction.North)
        //        {
        //            for (int k = 1;k < c.Steps+1;k++)
        //            {
        //                _strtab[i, j] = c.ToString();
        //                i--;
        //            }
        //        }
        //        else if (c.Direction == Direction.South)
        //        {
        //            for (int k = 1; k < c.Steps + 1; k++)
        //            {
        //                _strtab[i, j] = c.ToString();
        //                i++;
        //            }
        //        }
        //        if (c.Direction == Direction.East)
        //        {
        //            for (int k = 1; k < c.Steps + 1; k++)
        //            {
        //                _strtab[i, j] = c.ToString();
        //               j++;
        //            }
        //        }
        //        if (c.Direction == Direction.West)
        //        {
        //            for (int k = 1; k < c.Steps + 1; k++)
        //            {
        //                _strtab[i, j] = c.ToString();
        //                j--;
        //            }
        //        }
        //    }
        //}
        //private void printGraph()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < _iLength; i++)
        //    {
        //        StringBuilder sbline = new StringBuilder();
        //        for (int j = 0; j < _jLength; j++)
        //        {
        //            if (string.IsNullOrEmpty(_strtab[i, j]))
        //            {
        //                _strtab[i, j] = ".";
        //            }
        //            sbline.Append(_strtab[i, j]);
        //        }
        //        sb.AppendLine(sbline.ToString());
        //    }
        //    string str = sb.ToString();
        //    Debug.LogFormat(sb.ToString());
        //}

        private void SolvePart1(bool verbose)
        {
       //     bool isFound = false;
       //         int i = 1;
       //     int ffi = 0, ffj = 0;
       //     while (!isFound)
       //     {
       //         for (int j =1;!isFound  && j < _jLength; j++)
       //         {
       //             if (_strtab[i, j] == "#")
       //             {
       //                 if (j+1 < _jLength && _strtab[i, j + 1] == ".")
       //                 {
       //                     ffi = i;
       //                     ffj = j+1;
       //                     isFound = true;
       //                 }
       //                 else if (i + 1 < _iLength && _strtab[i + 1, j] == ".")
       //                 {
       //                     ffi = i+1;
       //                     ffj = j;
       //                     isFound = true;
       //                 }
       //             }

       //         }
       //         i++;
       //     }
       ////     Algorithms.GenericAlgo.FloodFill(ffi, ffj, ref _strtab, _iLength, _jLength);
       //     long solution = 0;
       //     for (i=0; i< _iLength; i++)
       //     {
       //         for (int j = 0; j < _jLength; j++)
       //         {
       //             if (_strtab[i,j] == "#" || _strtab[i,j] == "@")
       //             {
       //                 solution++;
       //             }
       //         }
       //     }

          //  Debug.Log($"Solution Part1 : {solution}");
            Queue<(long, long)> test = new Queue<(long, long)>();
            test.Enqueue((2,0));
            test.Enqueue((1, 2));
            test.Enqueue((0, 4));
            test.Enqueue((1, 4));
            test.Enqueue((2, 4));
            test.Enqueue((3, 3));
            test.Enqueue((4, 2));
            test.Enqueue((5, 1));
          //  var testpick = Solve(test, 8);
            var part1 = Solve(_queueShoelace1, _boundaries);
            Debug.Log($"Solution Shoelace Part1 : {(long)part1}");
        }
        private void SolvePart2(bool verbose)
        {
            Stack<(long, long)> s = new Stack<(long, long)>();
            Queue<(long, long)> q = new Queue<(long, long)>();
            foreach(var point in _queueShoelace2)
            {
                s.Push(point);
            }
            foreach (var point in s)
            {
                q.Enqueue(point);
            }
            var part2 = Solve(q, _boundaries2);
            //var part2 = Solve(_queueShoelace2, _boundaries2);

            Debug.Log($"Solution Shoelace Part2 : {(long)part2}");
        }
    }
}