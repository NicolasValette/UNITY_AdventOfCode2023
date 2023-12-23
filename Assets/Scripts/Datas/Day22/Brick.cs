using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Datas.Day22
{
    public class Brick
    {
        public int ID { get; private set; }
        public Coords3D Start { get; private set; }
        public Coords3D End { get; private set; }
        public List<Brick> SupportingBricks { get; private set; }
        public List<Brick> SupportedByBricks { get; private set; }
        public Brick(int id, Coords3D start, Coords3D end)
        {
            ID = id;
            Start = start;
            End = end;
            SupportingBricks = new List<Brick>();
            SupportedByBricks = new List<Brick>();
        }

        public List<Coords3D> GetPoints()
        {
            List<Coords3D> points = new List<Coords3D>();
            Coords3D s = new Coords3D(Start.X, Start.Y, Start.Z);
            while (s != End)
            {
                points.Add(s);
                if (s.X != End.X)
                {
                    s = s + new Coords3D(1, 0, 0);
                }
                else if (s.Y != End.Y)
                {
                    s = s + new Coords3D(0, 1, 0);
                }
                else if (s.Z != End.Z)
                {
                    s = s + new Coords3D(0, 0, 1);
                }
                else
                {
                    break;
                }
            }
            points.Add(End);
            return points;
        }
        public void AddSupportingBrick (Brick brick)
        {
            SupportingBricks.Add(brick);
        }
        public void AddSupportedByBrick(Brick brick)
        {
            SupportedByBricks.Add(brick);
        }
        public void Fall(int step)
        {
            Start.Z -= step;
            End.Z -= step;
        }

        
        public override string ToString()
        {
            return $"Brick #{ID} - ({Start},{End})";
        }
    }
}