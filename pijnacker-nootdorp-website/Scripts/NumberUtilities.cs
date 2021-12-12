using System;
using System.Collections.Generic;

public static class NumberUtilities
{
    public static List<int> GetDigits(int source)
    {
        int individualFactor = 0;
        int tennerFactor = Convert.ToInt32(Math.Pow(10, source.ToString().Length));

        List<int> digits = new List<int>();
        do
        {
            source -= tennerFactor * individualFactor;
            tennerFactor /= 10;
            individualFactor = source / tennerFactor;

            digits.Add(individualFactor);
        } while (tennerFactor > 1);

        return digits;
    }
}