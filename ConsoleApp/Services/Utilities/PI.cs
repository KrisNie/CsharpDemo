using System;

namespace Services.Utilities
{
    public static class PI
    {
        /// <summary>
        /// Given that Pi can be estimated using the function 4 * (1 – 1/3 + 1/5 – 1/7 + …)
        /// with more terms giving greater accuracy,
        /// write a function that calculates Pi to an accuracy of 5 decimal places.
        /// </summary>
        public static void Pi()
        {
            double pi = 0;
            var i = 1;
            double fraction = 1;

            while (Math.Abs(Math.Round(pi, 5, MidpointRounding.ToZero) - 3.14159) > 0)
            {
                fraction += Math.Pow(-1, i) / (2 * i + 1);
                pi = 4 * fraction;
                i++;
            }

            Console.WriteLine(i);
            Console.WriteLine(pi);
        }
    }
}