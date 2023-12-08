using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

namespace AdventOfCode.Utility
{
    public static class MathUt
    {
        public static List<long> Primes (long max)
        {
                List<long> primes = new();
                for (int i = 2; i <= max; i++)
                    if (!primes.Any(p => i % p == 0))
                        primes.Add(i);
                return primes;  
        }

        public static long PPCM (List<long> numbers)
        {
            List<(long, long)> factors = new List<(long, long)> ();
            for (int i=0; i < numbers.Count;i++)
            {
                List<(long, long)> factor = Factor(numbers[i]);
                for (int j=0;j<factor.Count;j++)
                {
                    if (factors.Exists(x => x.Item1 == factor[j].Item1))
                    {
                        int ind = factors.FindIndex(x => x.Item1 == factor[j].Item1);
                        if (factors[ind].Item2 < factor[j].Item2)
                        {
                            factors[ind] = factor[j];
                        }
                    }
                    else
                    {
                        factors.Add(factor[j]);
                    }
                }

            }
            long ppcm = 1;
            for (int i=0; i<factors.Count;i++)
            {
                ppcm *= (long)Mathf.Pow(factors[i].Item1, factors[i].Item2); ;
            }
            return ppcm;
        }
        public static List<(long, long)> Factor(long number)
        {
            List<(long, long)> factor = new List<(long, long)>();
            List<long> primes = Primes(number);
            long actual = number;
            for (int i = 0; i< primes.Count; i++)
            {
                if (actual%primes[i]==0)
                {
                    int exp = 1;
                    actual = actual / primes[i];
                    while (actual % primes[i] == 0)
                    {
                        exp++;
                        actual = actual / primes[i];
                    }
                    factor.Add((primes[i], exp));
                }
                if (actual <= 1)
                {
                    break;
                }
            }
            return factor;
        }
    }
}