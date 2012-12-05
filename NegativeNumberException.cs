using System;

namespace StringCalculatorSolution{
    public class NegativeNumberException : Exception{
        public NegativeNumberException(string msg) : base(msg){}
    }
}