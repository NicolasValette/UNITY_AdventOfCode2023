using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day7
{
    public enum Card
    {
        none,
        two = 2,
        three = 3,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        T,
        J,
        Q,
        K,
        A
    }
    public enum HandValue
    {
        High,
        OnePair,
        TwoPair,
        Three,
        Full,
        Four,
        Five
    }
    public class Hand : IComparable<Hand>
    {
        private Card[] _cards = new Card[5];
        private HandValue _handValue;
        public HandValue Value { get { return _handValue; } }
        private HandValue _handValuePart2;
        public HandValue ValuePart2 { get { return _handValuePart2; } }
        public long Bid { get; set; }
        public bool Part2 { get; set; } = false;
        public Card this[int i]
        {
            get { return _cards[i]; }
            set { _cards[i] = value; }
        }
        public Hand(Card[] cards, long bid)
        {
            _cards = cards;
            Bid = bid;
        }
        public Hand(string cards, long bid)
        {
            Dictionary<Card, int> dic = new Dictionary<Card, int>();
            int i = 0;
            foreach (char c in cards)
            {
                if (Enum.TryParse<Card>(c.ToString(), out Card card))
                {
                    if (!dic.ContainsKey(card))
                    {
                        dic.Add(card, 1);
                    }
                    else
                    {
                        dic[card]++;
                    }
                    
                    _cards[i] = card;
                    i++;
                }
            }
            HandValue[] val = GetValue(dic);
            _handValue = val[0];
            _handValuePart2 = val[1];
            Bid = bid;
        }
        public Card GetHighest()
        {
            Card c = Card.none;
            for (int i=0; i<_cards.Length;i++)
            {
                if (c < _cards[i])
                {
                    c = _cards[i];
                }
            }
            return c;
        }
    
        private HandValue[] GetValue(Dictionary<Card, int> dic)
        {
            HandValue[] values = new HandValue[2];
            int numberOfJ = 0;
            if (dic.Count == 5) //All card are different
            {
                values[0] = HandValue.High;
            }
            else if (dic.Count== 4) //All card are different except one
            {
                values[0] = HandValue.OnePair;
            }
            else if (dic.Count == 3) //two pair or three of a kind
            {
                bool three = false;
                foreach (KeyValuePair<Card, int> val in dic)
                {
                    if (val.Value == 3)
                    {
                        three = true;
                    }
                }
                values[0] = (three) ? HandValue.Three : HandValue.TwoPair;
            }
            else if (dic.Count== 2) //full house or four
            {
                bool four = false;
                foreach (KeyValuePair<Card, int> val in dic)
                {
                    if (val.Value == 4)
                    {
                        four = true;
                    }
                }
                values[0] = (four) ? HandValue.Four : HandValue.Full;
            }
            else
            {
                values[0] = HandValue.Five;
            }

            dic.TryGetValue(Card.J, out numberOfJ);
            int j = 0;
            if (numberOfJ>0)
            {
                j++;
            }
            if (dic.Count - j== 5) //All card are different
            {
                values[1] = HandValue.High;
            }
            else if (dic.Count - j == 4) //All card are different except one
            {
                values[1] = HandValue.OnePair;
            }
            else if (dic.Count - j == 3) //two pair or three of a kind
            {
                bool three = false;
                foreach (KeyValuePair<Card, int> val in dic)
                {
                    if (val.Value + numberOfJ == 3)
                    {
                        three = true;
                    }
                }
                values[1] = (three) ? HandValue.Three : HandValue.TwoPair;
            }
            else if (dic.Count - j == 2) //full house or four
            {
                bool four = false;
                foreach (KeyValuePair<Card, int> val in dic)
                {
                    if (val.Value + numberOfJ == 4)
                    {
                        four = true;
                    }
                }
                values[1] = (four) ? HandValue.Four : HandValue.Full;
            }
            else
            {
                values[1] = HandValue.Five;
            }
            return values;
        }
        public void Sort()
        {
            Array.Sort(_cards);
            
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Hand is [");
            for (int i = 0; i <5; i++)
            {
                stringBuilder.Append(((int)_cards[i] <= 9) ? ((int)_cards[i]) : _cards[i].ToString());
            }
            HandValue val = (!Part2) ? Value : ValuePart2;
            stringBuilder.Append($"] ({val}) with bid : {Bid}");
            return stringBuilder.ToString();
        }

        public int CompareTo(Hand other)
        {
            if (!Part2)
            {
                if (other == null)
                {
                    return 1;
                }
                if (Value > other.Value)
                {
                    return 1;
                }
                else if (Value < other.Value)
                {
                    return -1;
                }
                else if (Value == other.Value)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int valuetmp = (int)_cards[i];
                        int othervaluetmp = (int)other._cards[i];
                        if (Part2)
                        {
                            if (_cards[i] == Card.J)
                                valuetmp = 0;
                            if (other._cards[i] == Card.J)
                                othervaluetmp = 0;
                        }
                        if (valuetmp < othervaluetmp)
                        {
                            return -1;
                        }
                        else if (valuetmp > othervaluetmp)
                        {
                            return 1;
                        }
                    }
                }
                return 0;
            }
            else
            {
                if (other == null)
                {
                    return 1;
                }
                if (ValuePart2 > other.ValuePart2)
                {
                    return 1;
                }
                else if (ValuePart2 < other.ValuePart2)
                {
                    return -1;
                }
                else if (ValuePart2 == other.ValuePart2)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int valuetmp = (int)_cards[i];
                        int othervaluetmp = (int)other._cards[i];
                        if (Part2)
                        {
                            if (_cards[i] == Card.J)
                                valuetmp = 0;
                            if (other._cards[i] == Card.J)
                                othervaluetmp = 0;
                        }
                        if (valuetmp < othervaluetmp)
                        {
                            return -1;
                        }
                        else if (valuetmp > othervaluetmp)
                        {
                            return 1;
                        }
                    }
                }
                return 0;
            }
        }
    }
}
