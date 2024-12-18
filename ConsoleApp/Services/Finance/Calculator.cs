using System;

namespace Services.Finance
{
    public class Calculator : ICalculator
    {
        public Calculator()
        {
        }

        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }

        public int Add()
        {
            return FirstNumber + SecondNumber;
            throw new NotImplementedException();
        }
    }
}