namespace Services.Utilities
{
    public class Pi
    {
        private static void GetPi()
        {
            double pi = 0;
            var i = 1;

            while (Math.Abs(Math.Round(pi, 5, MidpointRounding.ToZero) - 3.14159) > 0)
            {
                pi = 4 * Accuracy(i);
                i++;
            }

            Console.WriteLine(i);
            Console.WriteLine(pi);
        }

        private static double Accuracy(int n)
        {
            double d;
            if (n == 0)
            {
                return 1;
            }

            if (n % 2 != 0)
            {
                d = (double)1 / -(2 * n + 1) + Accuracy(n - 1);
            }
            else
            {
                d = (double)1 / (2 * n + 1) + Accuracy(n - 1);
            }

            return d;
        }
    }
}