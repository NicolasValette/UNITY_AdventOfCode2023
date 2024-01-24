using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day11
{
    public class Galaxy
    {
        public int ID { get; }
        public int ICoord { get; }
        public int JCoord { get; }
        public Galaxy(int id, int i, int j)
        {
            ID = id; ICoord = i; JCoord = j;
        }
        public override string ToString()
        {
            return $"Galaxy #{ID} - ({ICoord}, {JCoord}).";
        }
    }
    public class SolverDay11 : Solver
    {
        [SerializeField]
        private long _multiplier;
        private List<Galaxy> _galaxies = new List<Galaxy>();
        private bool[] _colToExpand;
        private bool[] _lineToExpand;
        private string _stringGalaxies;
        // Start is called before the first frame update
        void Start()
        {
            //ReadData(_verbose);

            ReadSmallData(_verbose);

            if (_part1) Solve(_multiplier, _verbose);
            // if (_part1) SolvePart1(_verbose);
            if (_part2) SolvePart2(_verbose);
        }

        private void ReadSmallData(bool verbose)
        {
            string[] lines = _input.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
            int idGalaxy = 1;
            bool[] galInColumn = new bool[lines.Length];
            _lineToExpand = new bool[lines.Length];
            _colToExpand = new bool[lines[0].Trim().Length];
            for (int i = 0; i < lines.Length; i++)
            {
                _lineToExpand[i] = true;
            }
            for (int i = 0; i < lines[0].Trim().Length; i++)
            {
                _colToExpand[i] = true;
            }
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Trim().Length; j++)
                {

                    if (lines[i][j] == '#')
                    {
                        _colToExpand[j] = false;
                        _lineToExpand[i] = false;
                        _galaxies.Add(new Galaxy(idGalaxy, i, j));
                        idGalaxy++;
                    }
                }
            }
            Debug.Log("Data readed !");
        }




        private void ReadData(bool verbose)
        {
            StringReader reader = new StringReader(_input);
            string strread = reader.ReadLine();
            int id = 1;
            bool[] galInColumn = new bool[strread.Length];
            bool galInLine = false;
            List<List<char>> newInput = new List<List<char>>();
            int actualLine = 0;
            while (strread != null && strread != string.Empty)
            {
                newInput.Add(new List<char>());
                for (int i = 0; i < strread.Length; i++)
                {
                    newInput[actualLine].Add(strread[i]);
                    if (strread[i] == '#') // we found a galaxy
                    {
                        galInLine = true;
                        galInColumn[i] = true;
                    }

                }
                if (!galInLine)
                {
                    newInput.Add(new List<char>(newInput[actualLine]));
                    actualLine++;
                }
                actualLine++;
                galInLine = false;
                strread = reader.ReadLine();
            }
            int offset = 0;
            for (int i = 0; i < galInColumn.Length; ++i)
            {
                if (!galInColumn[i])
                {
                    for (int j = 0; j < newInput.Count; ++j)
                    {
                        newInput[j].Insert(i + 1 + offset, '.');

                    }
                    offset++;
                }
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < newInput.Count; i++)
            {
                sb.AppendLine(new string(newInput[i].ToArray()));
            }
            _stringGalaxies = sb.ToString();
            if (verbose) Debug.Log($"Galaxy : \n{_stringGalaxies}");


            StringReader readerGal = new StringReader(_stringGalaxies);
            string sr = readerGal.ReadLine();
            int line = 0;
            while (sr != null && sr != string.Empty)
            {
                for (int j = 0; j < sr.Length; j++)
                {
                    if (sr[j] == '#')
                    {
                        if (verbose) Debug.Log($"Galaxy found at ({line},{j}).");
                        _galaxies.Add(new Galaxy(id, line, j));
                        id++;
                    }
                }
                line++;
                sr = readerGal.ReadLine();
            }

            if (verbose)
            {
                StringBuilder stb = new StringBuilder();
                stb.AppendLine($"Universe contains {_galaxies.Count} galaxies : ");
                for (int i = 0; i < _galaxies.Count; i++)
                {
                    stb.AppendLine(_galaxies[i].ToString());
                }
                Debug.Log(stb.ToString());
            }
            Debug.Log("Data readed");
        }
        public long getNumbToExpendBetween(int i, int j, bool isColumn)
        {
            long n = 0;
            if (i > j)
            {
                int t = j;
                j = i;
                i = t;
            }
            if (isColumn)
            {
                for (int k = i; k < j; k++)
                {
                    if (_colToExpand[k]) n++;
                }
            }
            else
            {
                for (int k = i; k < j; k++)
                {
                    if (_lineToExpand[k]) n++;
                }
            }
            return n;
        }
        private void Solve(long multiplier, bool verbose)
        {
            long solution = 0;
            List<(Galaxy, Galaxy)> galPair = new List<(Galaxy, Galaxy)>();
            for (int i = 0; i < _galaxies.Count; i++)
            {
                for (int j = i + 1; j < _galaxies.Count; j++)
                {
                    galPair.Add((_galaxies[i], _galaxies[j]));
                }
            }
            for (int i = 0; i < galPair.Count; i++)
            {
                long iDiff = (Mathf.Abs((galPair[i].Item2.ICoord - galPair[i].Item1.ICoord)));
                long iAdd = getNumbToExpendBetween(galPair[i].Item1.ICoord, galPair[i].Item2.ICoord, false) * (multiplier-1);

                long jDiff = (Mathf.Abs((galPair[i].Item2.JCoord - galPair[i].Item1.JCoord)));
                long jAdd = getNumbToExpendBetween(galPair[i].Item1.JCoord, galPair[i].Item2.JCoord, true) * (multiplier-1);
                long shortestPath = (iDiff + iAdd) + (jDiff + jAdd);
                solution += shortestPath;
                if (verbose) Debug.Log($"Shortest Path between #{galPair[i].Item1.ID} and {galPair[i].Item2.ID} : {shortestPath}");
            }

            Debug.Log($"Solution : {solution}");
        }
        private void SolvePart1(bool verbose)
        {
            long solution = 0;
            List<(Galaxy, Galaxy)> galPair = new List<(Galaxy, Galaxy)>();
            for (int i = 0; i < _galaxies.Count; i++)
            {
                for (int j = i + 1; j < _galaxies.Count; j++)
                {
                    galPair.Add((_galaxies[i], _galaxies[j]));
                }
            }

            for (int i = 0; i < galPair.Count; i++)
            {
                int shortestPath = Mathf.Abs((galPair[i].Item2.ICoord - galPair[i].Item1.ICoord)) + Mathf.Abs((galPair[i].Item2.JCoord - galPair[i].Item1.JCoord));
                solution += shortestPath;
                if (verbose) Debug.Log($"Shortest Path between #{galPair[i].Item1.ID} and {galPair[i].Item2.ID} : {shortestPath}");
            }
            Debug.Log($"Solution : {solution}");
        }
        private void SolvePart2(bool verbose)
        {

        }


    }
}