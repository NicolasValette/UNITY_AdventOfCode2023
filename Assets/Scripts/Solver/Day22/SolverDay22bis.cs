using AdventOfCode.Datas;
using AdventOfCode.Datas.Day22;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;



namespace AdventOfCode.Solver.Day22
{
    public class SolverDay22bis : Solver
    {
        List<Brick2> AllBricks = new();
        Dictionary<char, int> BrickFallCounts = new();
       void Start()
        {
          
                string path = Path.Combine(Application.streamingAssetsPath, _filename);
                if (_verbose) Debug.Log($"Reading file : {path}");
                StreamReader stream = new StreamReader(path);
           
               
            char i = 'A';
            while (!stream.EndOfStream)
            {
                var n = stream.ReadLine().Split('~').Select(x => x.Split(',').Select(x => int.Parse(x)).ToList()).ToList();
                Brick2 tmp = new(i, new Coords3D(n[0][0], n[0][1], n[0][2]), new Coords3D(n[1][0], n[1][1], n[1][2]));
                AllBricks.Add(tmp);
                i++;
            }


            //Shift bricks that can move down until everyone is settled
            int numBricksShifted = 0;
            do
            {
                numBricksShifted = 0;

                HashSet<Coords3D> AllBrickPoints = new();

                foreach (var b in AllBricks) foreach (var p in b.Members()) AllBrickPoints.Add(p);

                foreach (var b in AllBricks)
                {
                    if (b.Members().Any(m => m.Z == 1 || (AllBrickPoints.Contains(m - new Coords3D(0, 0, 1)) && !b.Members().Contains(m - new Coords3D(0, 0, 1))))) continue;

                    b.Start = b.Start - new Coords3D(0, 0, 1);
                    b.End = b.End - new Coords3D(0, 0, 1);
                    numBricksShifted++;
                }


            } while (numBricksShifted != 0);

            AllBricks.Sort((a, b) => a.Start.Z.CompareTo(b.Start.Z));

            //Get everyone's interactions with one another.
            foreach (var b in AllBricks)
            {
                foreach (var p in AllBricks.Where(a => a.Start.Z == b.End.Z + 1))
                {
                    foreach (var m in b.Members())
                    {
                        if (p.Members().Any(a => a.X == m.X && a.Y == m.Y && a.Z - 1 == m.Z))
                        {
                            b.Supports.Add(p);
                            p.SupportedBy.Add(b);
                            break;
                        }
                    }
                }
            }

           

            foreach (var b in AllBricks) BrickFallCount(b, new(), true);

            Debug.Log($"Sol 1 : {SolvePartOne()}");
        }

        protected object SolvePartOne()
        {
            return BrickFallCounts.Count(kvp => kvp.Value == 0);
        }

        protected object SolvePartTwo()
        {
            return BrickFallCounts.Sum(k => k.Value);
        }

        private void BrickFallCount(Brick2 b, List<char> toIgnore, bool updateDict = false)
        {
            toIgnore.Add(b.id);
            if (b.Supports.Count == 0)
            {
                if (updateDict) BrickFallCounts[b.id] = 0;
            }

            //Collect all that would fall with the removal of that support
            var bricksThatFall = b.Supports.Where(p => p.SupportedBy.Count(a => !toIgnore.Contains(a.id)) == 0).ToList();
            foreach (var p in bricksThatFall)
            {
                toIgnore.Add(p.id);
            }

            foreach (var p in bricksThatFall)
            {
                BrickFallCount(p, toIgnore);
            }

            if (updateDict) BrickFallCounts[b.id] = toIgnore.Distinct().Count() - 1;
        }

        private class Brick2
        {
            public char id;
            public Coords3D Start;
            public Coords3D End;

            public List<Brick2> Supports = new();
            public List<Brick2> SupportedBy = new();

          // public int Length => Start.ManhattanDistance(End) + 1;

            public IEnumerable<Coords3D> Members()
            {
                yield return Start;
                var curLoc = Start;
                while (curLoc != End)
                {
                    if (Start.X == End.X && Start.Y == End.Y) curLoc = curLoc + new Coords3D(0, 0, 1); //Vertically Oriented Brick 
                    else if (Start.X == End.X && Start.Z == End.Z) curLoc = curLoc + new Coords3D(0, 1, 0); //Oriented Along Y- Axis
                    else if (Start.Y == End.Y && Start.Z == End.Z) curLoc = curLoc + new Coords3D(1, 0, 0); //Oriented Along x- Axis
                    yield return curLoc;
                }
            }

            public Brick2(char id, Coords3D p1, Coords3D p2)
            {
                this.id = id;
                if (p1.X < p2.X || p1.Y < p2.Y || p1.Z < p2.Z)
                {
                    Start = p1;
                    End = p2;
                }
                else
                {
                    Start = p2;
                    End = p2;
                }
            }

            public override string ToString()
            {
                return id.ToString();
            }
        }
    }
}
