using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace AdventOfCode.Solver.Day24
{
    public class SolverDay24 : Solver
    {
        [SerializeField]
        private float _min;
        [SerializeField]
        private float _max;
        private List<(Vector3 pos, Vector3 vel)> _hailstones;
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
            _hailstones = new List<(Vector3 pos, Vector3 vel)>();
            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine().Split('@').Select(x => x.Split(',').Select(x => float.Parse(x.Trim())).ToList()).ToList();
                Vector3 pos = new Vector3(line[0][0], line[0][1], line[0][2]);
                Vector3 vel = new Vector3(line[1][0], line[1][1], line[1][2]);
            _hailstones.Add((pos, vel));
            }
        }

        private bool IsInRange (float x, float min, float max)
        {
            return ((x <= max) && (x >= min));
        }

        private bool IsIntersectinRange ((Vector3 pos, Vector3 vel) hailstoneA, float timeA, (Vector3 pos, Vector3 vel) hailstoneB, float timeB, float min, float max)
        {
            float xa = hailstoneA.pos.x + timeA * hailstoneA.vel.x;
            float ya = hailstoneA.pos.y + timeA * hailstoneA.vel.y;

            float xb = hailstoneB.pos.x + timeB * hailstoneB.vel.x;
            float yb = hailstoneB.pos.y + timeB * hailstoneB.vel.y;

            return ((Mathf.Abs(xa - xb)  < 0.00001f) && (Mathf.Abs(ya - yb) < 0.00001f)
                && (IsInRange(xa, min, max))
                && (IsInRange(ya, min, max))
                && (IsInRange(xb, min, max))
                && (IsInRange(yb, min, max)));

        }
        //Multiply a 2x2 matrix with a 1x2 matrix
        private (float X, float Y) MultiplyMatrix (Matrix3x2 m1, Matrix3x2 m2)
        {
            (float X, float Y) result;
            result.X = m1.M11 * m2.M11 + m1.M21 * m2.M21;
            result.Y = m1.M21 * m2.M11 + m1.M22 * m2.M21;
            return result;
        }
        private bool SolveItersection ((Vector3 pos, Vector3 vel) hailstoneA, (Vector3 pos, Vector3 vel) hailstoneB, float min, float max, out (float X, float Y) result)
        {
            result = (0F, 0F);
       
            Matrix3x2 matrix1 = new Matrix3x2();
            Matrix3x2 matrix2 = new Matrix3x2();
            matrix1.M11 = hailstoneA.vel.x;
            matrix1.M12 = -hailstoneB.vel.x;
            matrix1.M21 = hailstoneA.vel.y;
            matrix1.M22 = -hailstoneB.vel.y;

            matrix2.M11 = hailstoneB.pos.x - hailstoneA.pos.x;
            matrix2.M21 = hailstoneB.pos.y - hailstoneA.pos.y;


            if (Matrix3x2.Invert(matrix1, out Matrix3x2 inv))
            {
                var matrixResult = Matrix3x2.Multiply(inv, matrix2);
                result = (matrixResult.M11, matrixResult.M21);
                
                return IsIntersectinRange(hailstoneA, matrixResult.M11, hailstoneB, matrixResult.M21, min, max);
            }
            return false;
        }
        private void SolvePart1(bool verbose)
        {
            long solution = 0L;
            Queue<(Vector3 pos, Vector3 vel)> q = new Queue<(Vector3 pos, Vector3 vel)>(_hailstones);
            while(q.TryDequeue(out (Vector3 pos, Vector3 vel) hailstoneA))
            {
                string strhailstoneA = $"HailstoneA : ({hailstoneA.pos.x},{hailstoneA.pos.y},{hailstoneA.pos.z} @ {hailstoneA.vel.x},{hailstoneA.vel.y},{hailstoneA.vel.z})";
                foreach (var hailstoneB in q.ToList())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(strhailstoneA);
                    string strhailstoneB = $"HailstoneB : ({hailstoneB.pos.x},{hailstoneB.pos.y},{hailstoneB.pos.z} @ {hailstoneB.vel.x},{hailstoneB.vel.y},{hailstoneB.vel.z})";
                    sb.AppendLine(strhailstoneB);
                    if (SolveItersection(hailstoneA, hailstoneB, _min, _max, out (float X, float Y) result))
                    {
                        double xa = Math.Round((hailstoneA.pos.x + result.X * hailstoneA.vel.x), 5);
                        double ya = Math.Round((hailstoneA.pos.y + result.X * hailstoneA.vel.y), 5);
                        if (result.X >= 0 && result.Y >= 0)
                        {
                            sb.AppendLine($"Hailstones' paths will cross inside the test area at ({xa},{ya}) time ({result.X},{result.Y}) ");
                            solution++;
                        }
                        else
                        {
                            sb.AppendLine("Hailstones' paths crossed in the past");
                        }

                    }
                    else
                    {
                        sb.AppendLine("Hailstones' paths will not cross");
                    }
                    if (verbose) Debug.Log(sb.ToString());
                    
                }
            }
            Debug.Log($"Solution Part1 :  {solution}");
        }

        private void SolvePart2(bool verbose)
        {
        }
    }
}