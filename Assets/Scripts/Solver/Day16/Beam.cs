using AdventOfCode.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Solver.Day16
{
    public class Beam
    {
        public static List<Beam> BeamList;
        public int Ipos { get; private set; }
        public int Jpos { get; private set; }
        public Direction Direction { get; private set; }

        public Beam(int ipos, int jpos, Direction direction)
        {
            Ipos = ipos;
            Jpos = jpos;
            Direction = direction;
            if (BeamList == null)
            {
                BeamList = new List<Beam>();
            }
            BeamList.Add(this);
        }
        public static void Init()
        {
            if (BeamList!= null)
            {
                BeamList.Clear();
            }
        }
        public void AlreadyVisited()
        {
            BeamList.Remove(this);
        }
        public void Move(ref string[] contraption)
        {
            if (contraption[Ipos][Jpos] == '.')
            {
                //we move to one step
                if (Direction == Direction.North)
                {
                    if (Ipos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Ipos--;
                    }
                }
                else if (Direction == Direction.South)
                {
                    if (Ipos + 1 >= contraption.Length)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Ipos++;
                    }
                }
                else if (Direction == Direction.West)
                {
                    if (Jpos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Jpos--;
                    }
                }
                else if (Direction == Direction.East)
                {
                    if (Jpos + 1 >= contraption[Ipos].Trim().Length)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Jpos++;
                    }
                }
            }
            else if (contraption[Ipos][Jpos] == '/')
            {
                if (Direction == Direction.North)
                {
                    if (Jpos + 1 >= contraption[Ipos].Trim().Length)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Jpos++;
                        Direction = Direction.East;
                    }
                }
                else if (Direction == Direction.South)
                {
                    if (Jpos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Jpos--;
                        Direction = Direction.West;
                    }
                }
                else if (Direction == Direction.West)
                {
                    if (Ipos + 1 >= contraption.Length )
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Ipos++;
                        Direction = Direction.South;
                    }
                }
                else if (Direction == Direction.East)
                {
                    if (Ipos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Ipos--;
                        Direction = Direction.North;
                    }
                }
            }
            else if (contraption[Ipos][Jpos] == '\\')
            {
                if (Direction == Direction.North)
                {
                    if (Jpos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Jpos--;
                        Direction = Direction.West;
                    }
                }
                else if (Direction == Direction.South)
                {
                    if (Jpos + 1 >= contraption[Ipos].Trim().Length)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Jpos++;
                        Direction = Direction.East;
                    }
                }
                else if (Direction == Direction.West)
                {
                    if (Ipos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Ipos--;
                        Direction = Direction.North;
                    }
                }
                else if (Direction == Direction.East)
                {
                    if (Ipos +1 >= contraption.Length)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Ipos++;
                        Direction = Direction.South;
                    }
                }
            }
            else if (contraption[Ipos][Jpos] == '-')
            {
                if (Direction == Direction.North || Direction == Direction.South)
                {
                    //we split the beam in two
                    if (Jpos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Jpos--;
                        Direction = Direction.West;
                    }

                    //we create a new beam
                    if (Jpos + 1 < contraption[Ipos].Trim().Length)
                    {
                        Beam b = new Beam(Ipos, Jpos + 1, Direction.East);
                    }
                }
                else if (Direction == Direction.West)
                {
                    if (Jpos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Jpos--;
                    }
                }
                else if (Direction == Direction.East)
                {
                    if (Jpos + 1 >= contraption[Ipos].Trim().Length)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Jpos++;
                    }
                }
            }
            else if (contraption[Ipos][Jpos] == '|')
            {
                if (Direction == Direction.East || Direction == Direction.West)
                {
                    //we split the beam in two
                    if (Ipos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Ipos--;
                        Direction = Direction.North;
                    }

                    //we create a new beam
                    if (Ipos + 1 < contraption.Length)
                    {
                        Beam b = new Beam(Ipos +1, Jpos, Direction.South);
                    }
                }
                else if (Direction == Direction.North)
                {
                    if (Ipos - 1 < 0)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Ipos--;
                    }
                }
                else if (Direction == Direction.South)
                {
                    if (Ipos + 1 >= contraption.Length)
                    {
                        BeamList.Remove(this);
                    }
                    else
                    {
                        Ipos++;
                    }
                }
            }

        }
    }
}