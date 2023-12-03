using AdventOfCode.Solver.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Solver.Day2
{
    public class GameInfos
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public GameInfos()
        {

        }
        public int Power ()
        {
            return Red * Green * Blue;
        }
    }
    public class SolverDay2 : MonoBehaviour, IDisplaySolution
    {
        [SerializeField]
        private int _redLoaded;
        [SerializeField]
        private int _greenLoaded;
        [SerializeField]
        private int _blueLoaded;
        [TextArea(17, 1000)]
        [SerializeField]
        private string _input;

        private List<GameInfos> _games = new List<GameInfos>();
        private int _solution;

        private void Start()
        {
            ReadGamesData();
            Solve();

        }
        private void ReadGamesData()
        {
            string[] substr = _input.Split('\n');

            for (int i = 0; i < substr.Length; i++)
            {
                int r = 0, g = 0, b = 0;
                string[] strGame = substr[i].Split(':')[1].Split(';');
                Debug.Log("yo");
                // Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
                for (int j = 0; j < strGame.Length; j++)
                {
                    string[] strPick = strGame[j].Split(',');
                    for (int k = 0; k < strPick.Length; k++)
                    {
                        string[] strInfo = strPick[k].Trim().Split(' ');
                        if (strInfo[1].Equals("red"))
                        {
                            int temporaryRed = int.Parse(strInfo[0]);
                            if (temporaryRed > r)
                            {
                                r = temporaryRed;
                            }

                        }
                        else if (strInfo[1].Equals("green"))
                        {
                            int temporaryGreen = int.Parse(strInfo[0]);
                            if (temporaryGreen > g)
                            {
                                g = temporaryGreen;
                            }
                        }
                        else if (strInfo[1].Equals("blue"))
                        {
                            int temporaryBlue = int.Parse(strInfo[0]);
                            if (temporaryBlue > b)
                            {
                                b = temporaryBlue;
                            }
                        }
                    }
                }
                GameInfos gi = new()
                {
                    Red = r,
                    Blue = b,
                    Green = g
                };
                _games.Add(gi);

            }
            Debug.Log ("Nombre de parties : " + _games.Count);
        }

        private void Solve()
        {
            int solPart1 = 0;
            int solPart2 = 0;
            
            for (int i = 0; i < _games.Count; i++)
            {
                Debug.Log("Power of game " + (i+1) + " : " + _games[i].Power());
                if (_games[i].Red <= _redLoaded &&
                    _games[i].Green <= _greenLoaded &&
                    _games[i].Blue <= _blueLoaded)
                {
                    Debug.Log("Game " + (i+1) + " is possible");
                    solPart1 += (i+1);
                    
                }
                solPart2 += _games[i].Power();
            }
            _solution = solPart1;
            Debug.Log("Solution Partie 1: " + _solution
                + ", Partie 2 : " + solPart2);
        }
        public int GetSolution()
        {
            return _solution;
        }
    }
}