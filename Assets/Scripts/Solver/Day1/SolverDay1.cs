using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SolverDay1 : MonoBehaviour
{
    private enum Digit
    {
        zero,
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine
    }
    [TextArea(17, 1000)]
    [SerializeField]
    private string _input;

    private bool _isSolved = false;
    private int _solution = 0;
    public int Solution
    {
        get
        {
            if (!_isSolved)
            {
                Solve();
            }
            return _solution;    
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int ExtractDigit (string str)
    {
        if (str.Equals(Digit.zero.ToString()))
        {
            return (int)Digit.zero;
        }
        else if (str.Equals(Digit.one.ToString()))
        {
            return (int)Digit.one;
        }
        else if (str.Equals(Digit.two.ToString()))
        {
            return (int)Digit.two;
        }
        else if (str.Equals(Digit.three.ToString()))
        {
            return (int)Digit.three;
        }
        else if (str.Equals(Digit.four.ToString()))
        {
            return (int)Digit.four;
        }
        else if (str.Equals(Digit.five.ToString()))
        {
            return (int)Digit.five;
        }
        else if (str.Equals(Digit.six.ToString()))
        {
            return (int)Digit.six;
        }
        else if (str.Equals(Digit.seven.ToString()))
        {
            return (int)Digit.seven;
        }
        else if (str.Equals(Digit.eight.ToString()))
        {
            return (int)Digit.eight;
        }
        else if (str.Equals(Digit.nine.ToString()))
        {
            return (int)Digit.nine;
        }
        else
        {
            return int.Parse(str);
        }
    }
    private void Solve ()
    {
        string[] substr = _input.Split('\n');
       
        for (int i = 0; i < substr.Length; i++)
        {
            Match match = Regex.Match(substr[i], "one|two|three|four|five|six|seven|eight|nine|zero|[0-9]");
            Match matchInv = Regex.Match(substr[i], "one|two|three|four|five|six|seven|eight|nine|zero|[0-9]", RegexOptions.RightToLeft);
            if (match.Success && matchInv.Success)
            {
                int temporaryNumber = ExtractDigit(match.Value) *10 + ExtractDigit(matchInv.Value);
                Debug.Log(temporaryNumber);
                _solution += temporaryNumber;
            }
        }
        Debug.Log("Nombre de ligne : " + substr.Length +"\nSolution : " + _solution);
        _isSolved = true;
        
    }
}
