using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;


namespace AdventOfCode.Solver.Day2
{
    public class SolverDay3 : MonoBehaviour
    {
        [TextArea(17, 1000)]
        [SerializeField]
        private string _input;
        private char[,] _gondollaEngine;
        private int iEngineLeght = 0;
        private int jEngineLeght = 0;
        // Start is called before the first frame update
        void Start()
        {
            LoadGondollaEngine();
            Solve();
        }

        private long Gear(int posi, int posj)
        {
            int nbOfPart = 0;
            long ratio = 1;

            bool[] posTab = new bool[8];
            if (_gondollaEngine[posi, posj] == '*')
            {
                //special case 5.7
                if (posi - 1 >= 0)
                {
                    if (posj - 1 >= 0 && posj +1 < jEngineLeght)
                    {
                        if ((char.IsDigit(_gondollaEngine[posi - 1, posj - 1])) &&
                                (Regex.IsMatch(_gondollaEngine[posi - 1, posj].ToString(), "[^0-9]")) &&
                                (char.IsDigit(_gondollaEngine[posi - 1, posj + 1])))
                        {
                            long n = int.Parse(ExtractNumberPart(posi - 1, posj - 1));
                            long m = int.Parse(ExtractNumberPart(posi - 1, posj + 1));
                            if ((char.IsDigit(_gondollaEngine[posi, posj - 1])) ||
                                ((char.IsDigit(_gondollaEngine[posi, posj + 1])))||
                                ((char.IsDigit(_gondollaEngine[posi+1, posj + 1]))) ||
                                ((char.IsDigit(_gondollaEngine[posi+1, posj + 1]))))
                                {
                                return 0;
                            }
                            return n * m;
                        }
                    }
                    if (posi + 1 < iEngineLeght)
                    {
                        if (posj - 1 >= 0 && posj + 1 < jEngineLeght)
                        {
                            if ((char.IsDigit(_gondollaEngine[posi + 1, posj - 1])) &&
                                    (Regex.IsMatch(_gondollaEngine[posi + 1, posj].ToString(), "[^0-9]")) &&
                                    (char.IsDigit(_gondollaEngine[posi + 1, posj + 1])))
                            {
                                long n = int.Parse(ExtractNumberPart(posi + 1, posj - 1));
                                long m = int.Parse(ExtractNumberPart(posi + 1, posj + 1));
                                if ((char.IsDigit(_gondollaEngine[posi-1, posj - 1])) ||
                                    ((char.IsDigit(_gondollaEngine[posi-1, posj + 1]))) ||
                                    ((char.IsDigit(_gondollaEngine[posi , posj + 1]))) ||
                                    ((char.IsDigit(_gondollaEngine[posi , posj + 1]))))
                                {
                                    return 0;
                                }
                                return n * m;
                            }
                        }
                    }
                }

                if (posi - 1 >= 0 && posj - 1 >= 0)
                {
                    if (char.IsDigit(_gondollaEngine[posi - 1, posj - 1]))
                    {
                        posTab[0] = true;
                        if (char.IsDigit(_gondollaEngine[posi - 1, posj]))
                        {
                            posTab[1] = true;
                            if (posj + 1 < jEngineLeght && char.IsDigit(_gondollaEngine[posi - 1, posj + 1]))
                            {
                                posTab[2] = true;
                            }
                        }

                        ratio *= int.Parse(ExtractNumberPart(posi - 1, posj - 1));
                        nbOfPart++;
                    }
                }
                if (posTab[1] == false && posi - 1 >= 0)
                {
                    if (char.IsDigit(_gondollaEngine[posi - 1, posj]))
                    {
                        if (posj + 1 < jEngineLeght && char.IsDigit(_gondollaEngine[posi - 1, posj + 1]))
                        {
                            posTab[2] = true;
                        }
                        nbOfPart++;
                        ratio *= int.Parse(ExtractNumberPart(posi - 1, posj));
                    }
                }
                if (posTab[2] == false && posi - 1 >= 0 && posj + 1 < jEngineLeght)
                {
                    if (char.IsDigit(_gondollaEngine[posi - 1, posj + 1]))
                    {
                        ratio *= int.Parse(ExtractNumberPart(posi - 1, posj + 1));
                        nbOfPart++;
                    }
                }


                if (posj - 1 >= 0)
                {
                    if (char.IsDigit(_gondollaEngine[posi, posj - 1]))
                    {
                        ratio *= int.Parse(ExtractNumberPart(posi, posj - 1));
                        nbOfPart++;
                    }
                }

                if (posj + 1 < jEngineLeght)
                {
                    if (char.IsDigit(_gondollaEngine[posi, posj + 1]))
                    {
                        ratio *= int.Parse(ExtractNumberPart(posi, posj + 1));
                        nbOfPart++;
                    }
                }

                if (posi + 1 < iEngineLeght && posj - 1 >= 0)
                {
                    if (char.IsDigit(_gondollaEngine[posi + 1, posj - 1]))
                    {
                        posTab[5] = true;
                        if (char.IsDigit(_gondollaEngine[posi + 1, posj]))
                        {
                            posTab[6] = true;
                            if (posj + 1 < jEngineLeght && char.IsDigit(_gondollaEngine[posi + 1, posj + 1]))
                            {
                                posTab[7] = true;
                            }
                        }

                        ratio *= int.Parse(ExtractNumberPart(posi + 1, posj - 1));
                        nbOfPart++;
                    }
                }
                if (posTab[6] == false && posi + 1 >= 0)
                {
                    if (char.IsDigit(_gondollaEngine[posi + 1, posj]))
                    {
                        if (posj + 1 < jEngineLeght && char.IsDigit(_gondollaEngine[posi + 1, posj + 1]))
                        {
                            posTab[7] = true;
                        }

                        ratio *= int.Parse(ExtractNumberPart(posi + 1, posj));
                        nbOfPart++;
                    }
                }
                if (posTab[7] == false && posi + 1 >= 0 && posj + 1 < jEngineLeght)
                {
                    if (char.IsDigit(_gondollaEngine[posi + 1, posj + 1]))
                    {
                        ratio *= int.Parse(ExtractNumberPart(posi + 1, posj + 1));
                        nbOfPart++;
                    }
                }
            }
            if (nbOfPart == 2)
            {
                return ratio;
            }
            return 0;
        }

        private bool IsSymbol(char c)
        {
            return Regex.IsMatch(c.ToString(), "[^0 - 9a - zA - Z.]");
        }

        private bool IsPart(int posi, int posj)     // we get the poisition of the first digit
        {
            int offset = 0;

            //first digit
            if (posj - 1 >= 0)
            {
                if (posi - 1 >= 0)        //we can check this position
                {
                    if (IsSymbol(_gondollaEngine[posi - 1, posj - 1]))
                    {
                        return true;
                    }
                }
                if (IsSymbol(_gondollaEngine[posi, posj - 1]))
                {
                    return true;
                }
                if (posi + 1 < iEngineLeght)
                {
                    if (IsSymbol(_gondollaEngine[posi + 1, posj - 1]))
                    {
                        return true;
                    }
                }
            }
            if (posi - 1 >= 0)        //we can check this position
            {
                if (IsSymbol(_gondollaEngine[posi - 1, posj]))
                {
                    return true;
                }
            }
            if (posi + 1 < iEngineLeght)
            {
                if (IsSymbol(_gondollaEngine[posi + 1, posj]))
                {
                    return true;
                }
            }

            //midle
            for (int n = posj + 1; n < jEngineLeght && char.IsDigit(_gondollaEngine[posi, n]); n++)
            {
                if (posi - 1 >= 0)        //we can check this position
                {
                    if (IsSymbol(_gondollaEngine[posi - 1, n]))
                    {
                        return true;
                    }
                }
                if (posi + 1 < iEngineLeght)
                {
                    if (IsSymbol(_gondollaEngine[posi + 1, n]))
                    {
                        return true;
                    }
                }
                offset++;
            }

            int lastPosDigit = posj + offset + 1;
            //Last digit
            if (lastPosDigit < jEngineLeght)
            {
                if (posi - 1 >= 0)
                {
                    if (IsSymbol(_gondollaEngine[posi - 1, lastPosDigit]))
                    {
                        return true;
                    }
                }
                if (IsSymbol(_gondollaEngine[posi, lastPosDigit]))
                {
                    return true;
                }
                if (posi + 1 < iEngineLeght)
                {
                    if (IsSymbol(_gondollaEngine[posi + 1, lastPosDigit]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string ExtractNumberPart(int posi, int posj)
        {
            StringBuilder strbld = new StringBuilder();
            int j = posj;
            while (j - 1 >= 0 && char.IsDigit(_gondollaEngine[posi, j - 1]))
            {
                j--;
            }

            for (int n = j; n < jEngineLeght && char.IsDigit(_gondollaEngine[posi, n]); n++)
            {
                strbld.Append(_gondollaEngine[posi, n]);
            }
            return strbld.ToString();
        }
        private void Solve()
        {
            int solution = 0;
            long solution2 = 0;
            for (int i = 0; i < iEngineLeght; i++)
            {
                for (int j = 0; j < jEngineLeght; j++)
                {
                   
                    if (char.IsDigit(_gondollaEngine[i, j]))
                    {
                        string numb = ExtractNumberPart(i, j);
                        string strlog = "number " + numb;
                        if (IsPart(i, j))
                        {
                            strlog += " is a part !";
                            solution += int.Parse(numb);
                        }
                        else
                        {
                            strlog += " in NOT a part !";
                        }
                        //Debug.Log(strlog);
                        j = j + numb.Length;
                    }
                    

                }
            }

            for (int i = 0; i < iEngineLeght; i++)
            {
                for (int j = 0; j < jEngineLeght; j++)
                {

                    if (_gondollaEngine[i, j] == '*')
                    {
                        long ratio = Gear(i, j);
                        if (ratio != 0)
                        {
                         //   Debug.Log("Gear found, ratio : " + ratio);
                            solution2 += ratio;
                        }
                        else
                        {
                            Debug.Log("NonGear (" + i + ", " + j + "j");
                        }
                    }


                }
            }

           
            Debug.Log("Solution : " + solution);
            Debug.Log("Solution2 : " + solution2);
        }

        private void LoadGondollaEngine()
        {
            string[] substr = _input.Split('\n');
            iEngineLeght = substr.Length;
            jEngineLeght = substr[0].Trim().Length;
            _gondollaEngine = new char[iEngineLeght, jEngineLeght];
            for (int i = 0; i < substr.Length; i++)
            {
                for (int j = 0; j < substr[i].Trim().Length; j++)
                {
                    _gondollaEngine[i, j] = substr[i].Trim()[j];
                }
            }
            Debug.Log("Engine loaded");
        }
    }
}