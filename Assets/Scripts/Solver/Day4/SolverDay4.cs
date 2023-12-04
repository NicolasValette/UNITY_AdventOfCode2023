using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;


public class GameCard
{
    private int _numberOfCopy;
    public int Id { get; private set; }
    public List<int> WinningNumber { get; private set; }
    public List<int> PlayedNumber { get; private set; }

    public GameCard(int gameId)
    {
        Id = gameId;
        _numberOfCopy = 1;
        WinningNumber = new List<int>();
        PlayedNumber = new List<int>();
    }
    public void AddWinningNumber(int numb)
    {
        WinningNumber.Add(numb);
    }
    public void AddPlayedNumber(int numb)
    {
        PlayedNumber.Add(numb);
    }
    public void AddCopy(int n)
    {
        _numberOfCopy += n;
    }
    public int GetNumbOfCopy()
    {
        return _numberOfCopy;
    }

    public float GetWorthyPoint ()
    {
        int point = -1;
        for (int i=0;i<PlayedNumber.Count;i++)
        {
            if (WinningNumber.Contains(PlayedNumber[i]))
            {
                point++;
            }
        }
        return (point >=0)?Mathf.Pow(2, point):0;
    }
    public int getNumberOfMatchingNumber()
    {
        int matching = 0;
        for (int i = 0; i < PlayedNumber.Count; i++)
        {
            if (WinningNumber.Contains(PlayedNumber[i]))
            {
                matching++;
            }
        }
        return matching;
    }

}
public class SolverDay4 : MonoBehaviour
{

    [TextArea(17, 1000)]
    [SerializeField]
    private string _input;

    private List<GameCard> cards = new List<GameCard>();
    // Start is called before the first frame update
    void Start()
    {
        ReadData();
        //SolvePart1();
        SolvePart2();
    }

    private void ReadData()
    {
        string[] substr = _input.Split('\n');
        for (int i = 0; i < substr.Length; i++)
        {
            string card = substr[i].Split(":")[1];
            string[] numbers = card.Split("|");

            string[] winningNumbers = numbers[0].Trim().Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
            string[] PlayedNumbers = numbers[1].Trim().Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
            GameCard tempCard = new GameCard(i + 1);
            for (int j=0; j < winningNumbers.Length; j++)
            {
                tempCard.AddWinningNumber(int.Parse(winningNumbers[j].Trim()));
            }
            for (int j = 0; j < PlayedNumbers.Length; j++)
            {
                tempCard.AddPlayedNumber(int.Parse(PlayedNumbers[j].Trim()));
            }
            cards.Add(tempCard);
        }
        Debug.Log("Number of cards : " + cards.Count);
    }

    private void SolvePart1()
    {
        float solution = 0;
        for (int i=0; i < cards.Count; i++)
        {
            float points = cards[i].GetWorthyPoint();
            Debug.Log("Game " + cards[i].Id + " is worthy of " + points + " points !");
            solution += points;
        }
        Debug.Log("Solution : " + solution);
    }
    private void SolvePart2()
    {
        int solution = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            Debug.Log("************");
            int matchings = cards[i].getNumberOfMatchingNumber();
            solution += cards[i].GetNumbOfCopy();
            Debug.Log("Game " + cards[i].Id + " has " + cards[i].GetNumbOfCopy() + " copies");      
            Debug.Log("Game " + cards[i].Id + " add " + cards[i].GetNumbOfCopy() + " cards of the " + matchings + " following cards");
            for (int j = 1; j<= matchings; j++)
            {
                if (i+j < cards.Count)
                {
                    cards[i + j].AddCopy(cards[i].GetNumbOfCopy());
                }
            }
            Debug.Log("************");
        }
        Debug.Log("Solution : " + solution);
    }
}
