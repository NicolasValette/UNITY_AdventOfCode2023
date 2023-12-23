using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Datas
{
    public class Coords
    {
        public long X { get; }
        public long Y { get; }

        public Coords (long x, long y)
        {
            X = x;
            Y = y;
        }
    }

    public class IntCoords
    {
        public int X { get; }
        public int Y { get; }

        public IntCoords(int x, int y)
        {
            X = x;
            Y = y;
        }
        public static bool operator ==(IntCoords a, IntCoords b)
        {
            return ((a.X == b.X) && (a.Y == b.Y));
        }
        public static bool operator !=(IntCoords a, IntCoords b)
        {
            return ((a.X != b.X) || (a.Y != b.Y));
        }
        public override bool Equals(object obj)
        {
            return this == (obj as IntCoords);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }

    public class Coords3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Coords3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static bool operator == (Coords3D a, Coords3D b)
        {
            return ((a.X == b.X)&&(a.Y == b.Y)&&(a.Z==b.Z));
        }
        public static bool operator!=(Coords3D a, Coords3D b)
        {
            return ((a.X != b.X)||(a.Y != b.Y) ||(a.Z != b.Z));
        }
        public static Coords3D operator +(Coords3D a, Coords3D b)
        {
            return new Coords3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Coords3D operator -(Coords3D a, Coords3D b)
        {
            return new Coords3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }

        public override bool Equals(object obj)
        {
           return this == (obj as Coords3D);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
        

}
