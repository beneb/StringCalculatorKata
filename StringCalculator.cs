using System;
using System.Collections.Generic;
using System.Linq;

namespace StringCalculatorSolution{
    public class StringCalculator{
        public static int Add(string input){
            var strategies = new List<StringCalculatorStrategy>{
                                                                   new NullOrEmpty(input),
                                                                   new OneNumberOnly(input),
                                                                   new MultipleDelimiters(input),
                                                                   new DifferentDelimiterAsString(input),
                                                                   new DifferentDelimiterAsChar(input),                                                                   new NumbersWithCommasAndBlanks(input),
                                                                   new NumbersWithCommas(input),
                                                                   new Default(input)
                                                               };
            int[] result = (from c in strategies where c.CanHandle() select c.AsIntArray()).FirstOrDefault().ToArray();
            AssertForNegativeNumbers(result);
            Func<int, bool> FilterForNumbersGreaterThen1000 = c => c <= 1000;
            return result.Where(FilterForNumbersGreaterThen1000).Sum();
        }

        private static void AssertForNegativeNumbers(int[] result){
            // TODO: diesen IF muss ich natürlich auch wegoptimieren, aber wie?
            if (result.Any(c => c < 0)){
                int[] negatives = result.Where(c => c < 0).ToArray();
                string msg = negatives.First().ToString();
                string msgWhole = negatives.Skip(1).Aggregate(msg,
                                                              (current, number) =>
                                                              current + string.Format(", {0}", number));
                throw new NegativeNumberException(string.Format("negative numbers are not allowed: {0}", msgWhole));
            }
        }
    }

    public class Default : StringCalculatorStrategy{
        public Default(string input) : base(input){}

        public override bool CanHandle(){
            return true;
        }

        public override IEnumerable<int> AsIntArray(){
            return new[]{0};
        }
    }


    public class DifferentDelimiterAsChar : StringCalculatorStrategy{
        public DifferentDelimiterAsChar(string input) : base(input){}

        public override bool CanHandle(){
            return _input.StartsWith("//");
        }

        public override IEnumerable<int> AsIntArray(){
            char delimiter = _input[2];
            return _input.Remove(0, 4).Split(delimiter).Select(int.Parse);
        }
    }

    public class DifferentDelimiterAsString : StringCalculatorStrategy{
        public DifferentDelimiterAsString(string input) : base(input){}

        public override bool CanHandle(){
            return _input.StartsWith("//[");
        }

        public override IEnumerable<int> AsIntArray(){
            string delimiter = _input.Substring(_input.IndexOf('[') + 1, _input.IndexOf('\n') - 4);
            return _input.Remove(0, _input.IndexOf(']') + 1)
                .Split(new[]{delimiter}, StringSplitOptions.None)
                .Select(int.Parse);
        }
    }

    public class MultipleDelimiters : StringCalculatorStrategy{
        public MultipleDelimiters(string input) : base(input){}

        public override bool CanHandle(){
            return _input.Select(c => c).Count(c => c == '[') > 1;
        }

        public override IEnumerable<int> AsIntArray(){
            string removeFront = _input.Remove(0, 3);
            string removeEnd = removeFront.Substring(0, removeFront.IndexOf('\n') - 1);
            string[] splitedDelimiters = removeEnd.Split(new[]{"]["}, StringSplitOptions.None);
            return _input.Remove(0, _input.IndexOf('\n') + 1)
                .Split(splitedDelimiters, StringSplitOptions.None)
                .Select(int.Parse);
        }
    }

    public class NumbersWithCommasAndBlanks : StringCalculatorStrategy{
        public NumbersWithCommasAndBlanks(string input) : base(input){}

        public override bool CanHandle(){
            return _input.Contains(',') && _input.Contains('\n');
        }

        public override IEnumerable<int> AsIntArray(){
            return _input.Replace('\n', ',').Split(',').Select(int.Parse);
        }
    }

    public class NumbersWithCommas : StringCalculatorStrategy{
        public NumbersWithCommas(string input) : base(input){}

        public override bool CanHandle(){
            return _input.Contains(',');
        }

        public override IEnumerable<int> AsIntArray(){
            return _input.Split(',').Select(int.Parse);
        }
    }

    public class OneNumberOnly : StringCalculatorStrategy{
        private int _result;
        public OneNumberOnly(string input) : base(input){}

        public override bool CanHandle(){
            return int.TryParse(_input, out _result);
        }

        public override IEnumerable<int> AsIntArray(){
            return new[]{_result};
        }
    }

    public class NullOrEmpty : StringCalculatorStrategy{
        public NullOrEmpty(string input) : base(input){}

        public override bool CanHandle(){
            return string.IsNullOrEmpty(_input);
        }

        public override IEnumerable<int> AsIntArray(){
            return new[]{0};
        }
    }

    public abstract class StringCalculatorStrategy{
        protected readonly string _input;

        protected StringCalculatorStrategy(string input){
            _input = input;
        }

        public abstract bool CanHandle();
        public abstract IEnumerable<int> AsIntArray();
    }
}