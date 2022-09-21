namespace Xulu.Services
{
    public class CalculatorService : ICalculatorService
    {
        const string addition = "abcd";
        const string subtraction = "bcde";
        const string multiplication = "dede";
        private readonly ICalculatorValidationService _calculatorValidationService;

        public CalculatorService(ICalculatorValidationService calculatorValidationService)
        {
            _calculatorValidationService = calculatorValidationService;
        }


        public double Calculate(string operations)
        {
            _calculatorValidationService.IsValidOperations(operations);

            if (IsCalculateOneWord(operations))
            {
                return CalculateOneWord(operations);
            }
            else if (IsCalculateWordsWithOperators(operations))
            {
                return CalculateWordsWithOperators(operations);
            }
            else
            {
                return CalculateWordsWithoutOperator(operations);
            }
        }


        private bool IsCalculateOneWord(string operations)
        {
            return operations.Split(" ").Length == 1;
        }


        private bool IsCalculateWordsWithOperators(string operations)
        {
            return
                operations.Split(" ").Contains(addition) ||
                operations.Split(" ").Contains(subtraction) ||
                operations.Split(" ").Contains(multiplication);
        }


        private double CalculateOneWord(string operations)
        {
            var groupedChars = GetGroupedCharsFromOperationPhrase(operations);

            var sum = CalculateGroupedChars(groupedChars);

            return sum;
        }


        private double CalculateWordsWithoutOperator(string operations)
        {
            double sum = 0;

            foreach (var item in operations.Split(" "))
            {
                sum += CalculateOneWord(item);
            }

            return sum;
        }


        private double CalculateWordsWithOperators(string operations)
        {
            List<string> operationList = new List<string>();
            List<string> operationMembers = new List<string>();

            foreach (var item in operations.Split(" ").Reverse())
            {
                operationMembers.Add(item);

                if (item == addition || item == subtraction || item == multiplication)
                {
                    if (operationMembers.Count == 1)
                    {
                        operationList.Add(operationMembers.First());
                    }
                    else
                    {
                        operationList.Add(GetSumOfPhrases(operationMembers.ToArray().Reverse().ToList()).ToString());
                    }

                    operationMembers = new List<string>();
                }
            }

            if (operationList.Count == 1)
            {
                return Convert.ToDouble(operationList.First());
            }

            return ComputationalExpressionHasTwoConsecutiveOperators(operationList);
        }


        private double ComputationalExpressionHasTwoConsecutiveOperators(List<string> operationList)
        {
            double sum = 0;

            if (operationList.Contains(addition))
            {
                operationList.Remove(addition);

                foreach (var item in operationList.ToArray().Reverse())
                {
                    sum += Convert.ToDouble(item);
                }
            }
            else if (operationList.Contains(subtraction))
            {
                operationList.Remove(subtraction);

                foreach (var item in operationList.ToArray().Reverse())
                {
                    sum = sum == 0 ? Convert.ToDouble(item) : sum - Convert.ToDouble(item);
                }

                return sum;
            }
            else if (operationList.Contains(multiplication))
            {
                operationList.Remove(multiplication);

                foreach (var item in operationList)
                {
                    sum = sum == 0 ? 1 : sum;
                    sum *= Convert.ToDouble(item);
                }
            }

            return sum;
        }


        private double GetSumOfPhrases(List<string> levelParent)
        {
            double sum = 0;

            foreach (var item in levelParent.Skip(1))
            {
                var groupedChars = GetGroupedCharsFromOperationPhrase(item);

                if (levelParent.First() == addition)
                {
                    sum += CalculateGroupedChars(groupedChars);
                }
                else if (levelParent.First() == subtraction)
                {
                    sum = sum == 0 ? CalculateGroupedChars(groupedChars) : sum - CalculateGroupedChars(groupedChars);
                }
                else
                {
                    sum = sum == 0 ? 1 : sum;
                    sum *= CalculateGroupedChars(groupedChars);
                }
            }

            return sum;
        }


        private string[] GetGroupedCharsFromOperationPhrase(string operations)
        {
            string[] separatedWords = operations
                .Aggregate(" ", (seed, next) => seed + (seed.Last() == next ? "" : " ") + next)
                .Trim()
                .Split(' ');

            return separatedWords;
        }


        private double CalculateGroupedChars(string[] groupedChars)
        {
            double sum = 0;

            foreach (var item in groupedChars)
            {
                int coefficient;

                if (item.Contains("a"))
                {
                    coefficient = 1;
                }
                else if (item.Contains("b"))
                {
                    coefficient = 2;
                }
                else if (item.Contains("c"))
                {
                    coefficient = 3;
                }
                else if (item.Contains("d"))
                {
                    coefficient = 4;
                }
                else
                {
                    coefficient = 5;
                }

                sum += Math.Pow(((item.Length * coefficient) % 5), 2);
            }

            return sum;
        }

    }
}
