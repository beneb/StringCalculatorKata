using System;
using NUnit.Framework;

namespace StringCalculatorSolution{
    [TestFixture]
    public class StringCalculatorTests{
        // [TestCase("//[,][;][\n]\n1,2;3\n4", Result = 10)] -- questionable input
        
        
        [TestCase(null, Result = 0)]
        [TestCase("", Result = 0)]
        [TestCase("1", Result = 1)]
        [TestCase("42", Result = 42)]
        [TestCase("1,2", Result = 3)]
        [TestCase("1,2, 1001, 2000", Result = 3)]
        [TestCase("//;\n1;2", Result = 3)]
        [TestCase("//[***]\n2***3", Result = 5)]
        [TestCase("//[*][%]\n1*4%3", Result = 8)]
        [TestCase("//[**][%%%]\n1**2%%%3", Result = 6)]
        [TestCase("1\n2,3", Result = 6)]
        [TestCase("mrzerzyno", Result = 0)]
        public int InputTest(string input) {
            return StringCalculator.Add(input);
        }

        [Test]
        public void NegativeNumbersExceptionExpected(){
            try{
                StringCalculator.Add("2,3,-3,-5,4");
            }
            catch (NegativeNumberException e){
                Assert.AreEqual("negative numbers are not allowed: -3, -5", e.Message);
                return;
            }
            Assert.Fail("NegativeNumberException has not been thrown");
        }
    }
}