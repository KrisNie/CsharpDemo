using System;

namespace Services.Utilities
{
    public static class Pi
    {
        /// <summary>
        /// Given that Pi can be estimated using the function 4 * (1 – 1/3 + 1/5 – 1/7 + …)
        /// with more terms giving greater accuracy,
        /// write a function that calculates Pi to an accuracy of 5 decimal places.
        /// https://programmers.blogoverflow.com/2012/08/20-controversial-programming-opinions/
        /// </summary>
        public static void GetPi(int accuracy)
        {
            var i = 1;
            double pi = 0, fraction = 1;

            while (Math.Abs(Math.Round(pi, accuracy, MidpointRounding.ToZero) - 3.14159) > 0)
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