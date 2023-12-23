using AdventOfCode.Datas;
using AdventOfCode.Datas.Day22;
using AdventOfCode.Solver.Day19;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace AdventOfCode.Solver.Day22
{

    public class SolverDay22 : Solver
    {
        [SerializeField]
        private List<GameObject> _brickPartsPrefab;
        private List<Brick> _bricks;
        int color = 0;
        private bool[][][] _staticpos;
        private BrickGraph<int>_bricksGraph;
        private Dictionary<int, int> BrickFallCounts = new();
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
            StreamReader stream = new StreamReader(path);
            _bricksGraph = new BrickGraph<int>();
            _bricks = new List<Brick>();
            int maxX = 0, maxY = 0, maxZ = 0;
            int id = 0;
            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine().Split('~').Select(x => x.Split(',').Select(x => int.Parse(x)).ToList()).ToList();
                
                Brick b = new Brick(++id, new Coords3D(line[0][0], line[0][1], line[0][2]), new Coords3D(line[1][0], line[1][1], line[1][2]));
                if (line[0][0] > maxX) maxX = line[0][0];
                if (line[1][0] > maxX) maxX = line[1][0];
                if (line[0][1] > maxY) maxY = line[0][1];
                if (line[1][1] > maxY) maxY = line[1][1];
                if (line[0][2] > maxZ) maxZ = line[0][2];
                if (line[1][2] > maxZ) maxZ = line[1][2];
                _bricks.Add(b);
            }

            _bricks.Sort((a, b) => (a.Start.Z.CompareTo(b.Start.Z)));
            _staticpos = new bool[maxX+1][][];
            for (int i = 0; i< maxX+1;i++)
            {
                _staticpos[i] = new bool[maxY+1][];
                for (int j = 0;j< maxY+1;j++)
                {
                    _staticpos[i][j] = new bool[maxZ+1];
                
                }
            }
            if (verbose) Debug.Log("Reading finished");
        }

        private void CreateBrick (Brick brick)
        {
            if (brick.Start.X != brick.End.X)
            {
                for (int i = 0; brick.Start.X + i <= brick.End.X; i++)
                {
                    Instantiate(_brickPartsPrefab[color], new Vector3(brick.Start.X + i, brick.Start.Z, brick.Start.Y), Quaternion.identity);
                }
            }
            if (brick.Start.Y != brick.End.Y)
            {
                for (int i = 0; brick.Start.Y + i <= brick.End.Y; i++)
                {
                    Instantiate(_brickPartsPrefab[color], new Vector3(brick.Start.X, brick.Start.Z, brick.Start.Y + i), Quaternion.identity);
                }
            }
            if (brick.Start.Z != brick.End.Z)
            {
                for (int i = 0; brick.Start.Z + i <= brick.End.Z; i++)
                {
                    Instantiate(_brickPartsPrefab[color], new Vector3(brick.Start.X, brick.Start.Z + i, brick.Start.Y), Quaternion.identity);
                }
            }
            color = (color + 1) % _brickPartsPrefab.Count;
        }
        private void CreateAllBricks()
        {
            for (int i = 0; i< _bricks.Count; i++)
            {
                CreateBrick(_bricks[i]);
            }
        }

        private void Fall2()
        {
            
            foreach(Brick brick in _bricks)
            {
                int fallingHeight = 0;
                for (int i = 0; brick.Start.Z - i >= 0 && fallingHeight == 0;i++)
                {
                   //if (!brick.GetPoints().All(p=> _staticpos[p.X][p.Y][p.Z -1] == false))
                   // {
                   //     fallingHeight = i;
                   //     break;
                   // }
                   foreach(var point in brick.GetPoints())
                    {
                        if (_staticpos[point.X][point.Y][point.Z - i] == true)
                        {
                            fallingHeight = i-1;
                            break;
                        }
                    }
                }
               
                brick.Fall(fallingHeight);
                foreach (var x in brick.GetPoints())
                {
                    _staticpos[x.X][x.Y][x.Z] = true;
                }

                //brick.GetPoints().Select(x => _staticpos[x.X][x.Y][x.Z] = true) ;
            }
        }

        private bool Fall()
        {
            bool falling = false;
            int movingBricks;
            do
            {
                movingBricks = 0;
                List<Coords3D> points = GetPointList();
                foreach (var brick in _bricks)
                {
                    List<Coords3D> brickPoints = brick.GetPoints();
                    if (brickPoints.Any(b => b.Z == 1 || (points.Contains(b - new Coords3D(0, 0, 1)) && !brickPoints.Contains(b - new Coords3D(0, 0, 1)))))
                    {
                        continue;
                    }
                    movingBricks++;
                    brick.Fall(1);
                    falling = true;
                }
            } while (movingBricks != 0);
            return falling;
        }
        private List<Coords3D> GetPointList()
        {
            List<Coords3D> points = new List<Coords3D>();
            foreach (var b in _bricks)
            {
                points.AddRange(b.GetPoints());

            }
            return points;
        }
        private void SetupSupportedAndSupportingBricks()
        {
            foreach (var brick in _bricks)
            {
                foreach (var aboveBrick in _bricks.Where(b => b.Start.Z == brick.End.Z + 1))
                {
                    foreach (var point in brick.GetPoints())
                    {
                        if (aboveBrick.GetPoints().Any(p => p.X == point.X && p.Y == point.Y && p.Z - 1 == point.Z))
                        {
                            brick.AddSupportingBrick(aboveBrick);
                            aboveBrick.AddSupportedByBrick(brick);
                            _bricksGraph.AddEdge(brick.ID, aboveBrick.ID);
                        }
                    }
                }
            }
        }

        private List<Brick> GetListOfFallingBrick(Brick removeBrick)
        {
            List<Brick> fallingBricks = new List<Brick>();
            foreach (Brick b in removeBrick.SupportingBricks) 
            {
                fallingBricks.Add(b);
            }
            return fallingBricks;
        }

        //****
        private void BrickFallCount(Brick b, List<int> toIgnore, bool updateDict = false)
        {
            toIgnore.Add(b.ID);
            if (b.SupportingBricks.Count == 0)
            {
               if (updateDict) BrickFallCounts[b.ID] = 0;
            }

            //Collect all that would fall with the removal of that support
            var bricksThatFall = b.SupportingBricks.Where(p => p.SupportedByBricks.Count(a => !toIgnore.Contains(a.ID)) == 0).ToList();
            foreach (var p in bricksThatFall)
            {
                toIgnore.Add(p.ID);
            }

            foreach (var p in bricksThatFall)
            {
                BrickFallCount(p, toIgnore);
            }

           if (updateDict) BrickFallCounts[b.ID] = toIgnore.Distinct().Count() - 1;
        }

        private bool CanBrickBeRemove(Brick brick)
        {
            if (brick.SupportingBricks.Count ==0)
            {
                return true;
            }
            if (brick.SupportingBricks.Count > 0)
            {

                foreach (Brick b in brick.SupportingBricks)
                {
                    if (b.SupportedByBricks.Count < 2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void SolvePart1(bool verbose)
        {
           
            Fall2();
            SetupSupportedAndSupportingBricks();
            CreateAllBricks();
            //foreach (Brick brick in _bricks)
            //{
            //    if (CanBrickBeRemove(brick))
            //    {
            //        if (verbose) Debug.Log($"Brick {brick.ID} can be Removed (supporting {brick.SupportingBricks.Count}, supported by {brick.SupportedByBricks.Count}");
            //        solution++;
            //    }
            //    else
            //    {
            //        if (verbose) Debug.Log($"Brick {brick.ID} CANNOT be Removed (supporting {brick.SupportingBricks.Count}, supported by {brick.SupportedByBricks.Count}");
            //    }
            //}


            //foreach (var b in _bricks) BrickFallCount(b, new List<int>(), true);
            //Debug.Log($"Solution Part 1 : {BrickFallCounts.Count(kvp => kvp.Value == 0)}");

            // Debug.Log($"Solution Part 1 : {solution}");
            int sol = _bricks.Count(brick => _bricksGraph.GetSupporting(brick.ID).All(supported => _bricksGraph.GetSupported(supported).Count > 1));
            Debug.Log($"Solution Part 1 : {sol}");
        }
        private void SolvePart2(bool verbose)
        {

        }
    }
}