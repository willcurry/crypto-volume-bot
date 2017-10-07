using System;

namespace WillCrypto
{
    internal class Library
    {
        internal static string DoubleToPercentageString(double d)
        {
            return Math.Round(d, 2).ToString() + "%";
        }

        internal static double CalculateChange(double previous, double current)
        {
            var change = current - previous;
            return  change / previous * 100;
        }

    }
}
