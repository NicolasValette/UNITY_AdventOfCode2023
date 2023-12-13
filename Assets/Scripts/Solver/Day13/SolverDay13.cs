using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdventOfCode.Solver
{
    public class SolverDay13 : Solver
    {
        private string[] _patterns;
        private int _currentPattern = -1;
        private char[,] _pattern;
        private string[] _patternLines;
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
            StreamReader _stream = new StreamReader(path);
            _input = _stream.ReadToEnd();
            _patterns = _input.Split("\r\n\r\n");

        }
        private void ReadPattern()
        {
            if (_currentPattern + 1 < _patterns.Length)
            {
                _currentPattern++;
                _patternLines = _patterns[_currentPattern].Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
                _pattern = new char[_patternLines.Length, _patternLines[0].Trim().Length];
                for (int i = 0; i < _patternLines.Length; i++)
                {
                    for (int j = 0; j < _patternLines[i].Trim().Length; j++)
                    {
                        _pattern[i, j] = _patternLines[i][j];
                    }
                }
            }
            else
            {
                _pattern = null;
            }
        }
        private int DiffInLineReflexion(int downsideline)
        {
            int nbDiff = 0;
            for (int j = 0; j < _patternLines[downsideline].Trim().Length; j++)
            {
                int line = downsideline;
                line--;
                for (int i = line + 1; (i < _patternLines.Length) && line >= 0; i++)
                {
                    if (_pattern[line, j] != _pattern[i, j]) nbDiff++;
                    line--;
                }
            }
            return nbDiff;

        }
        private int DiffInCollumnReflexion(int rightsidecollumn)
        {
            int nbDiff = 0;
            for (int i = 0; i < _patternLines.Length; i++)
            {
                int collumn = rightsidecollumn;
                collumn--;
                for (int j = collumn +1 ; (j < _patternLines[i].Trim().Length) && collumn >= 0; j++)
                {
                    if (_pattern[i, collumn] != _pattern[i, j]) nbDiff++;
                    collumn--;
                }
            }
            return nbDiff;
        }
        private void SolvePart1(bool verbose)
        {
            long solution = 0L;
            long solution2 = 0L;
            int mirrorColumn = -1, mirrorLine = -1;
            int mirrorColumn2 = -1, mirrorLine2 = -1;

            ReadPattern();
            int id = 0;
            while (_pattern != null)
            {
                int reflexionline = 0;
                mirrorColumn = -1;
                mirrorLine = -1;
                mirrorColumn2 = -1;
                mirrorLine2 = -1;
                for (int i = 1; i < _patternLines.Length; i++)
                {
                    if (DiffInLineReflexion(i) == 0)
                    {
                        mirrorLine = i;
                    }
                    if (DiffInLineReflexion(i) == 1)
                    {
                        mirrorLine2 = i;
                    }
                }
                for (int j = 1; j < _patternLines[0].Trim().Length; j++)
                {
                    if (DiffInCollumnReflexion(j) == 0)
                    {
                        mirrorColumn = j;
                    }
                    if (DiffInCollumnReflexion(j) == 1)
                    {
                        mirrorColumn2 = j;
                    }
                }
                StringBuilder sb = new StringBuilder();
                
                sb.AppendLine("***************");
                sb.AppendLine($"Pattern #{id} : ");
                for (int i = 0; i < _patternLines.Length; i++)
                {
                    sb.AppendLine(_patternLines[i]);
                    
                }
                sb.AppendLine("*****");
                sb.AppendLine($"* Mirror line : {mirrorLine} and Mirror collumn : {mirrorColumn} ");
                sb.AppendLine($"* PART 2 Mirror line : {mirrorLine2} and Mirror collumn : {mirrorColumn2} ");
                sb.AppendLine("***************");
                sb.AppendLine("##########");
                Debug.Log(sb.ToString());
                id++;

                if (mirrorColumn > 0) 
                {
                    solution += mirrorColumn;
                }
                else if (mirrorLine > 0)
                {
                    solution += (100 * mirrorLine);
                }

                if (mirrorColumn2 > 0)
                {
                    solution2 += mirrorColumn2;
                }
                else if (mirrorLine2 > 0)
                {
                    solution2 += (100 * mirrorLine2);
                }
                ReadPattern();
            }
            Debug.Log($"Solution Part 1 : {solution}");
            Debug.Log($"Solution Part 2 : {solution2}");
        }
        private void SolvePart2(bool verbose)
        {
        }
    }
}