using System.Text.RegularExpressions;

namespace Xulu.Services
{
    public class CalculatorValidationService : ICalculatorValidationService
    {
        public void IsValidOperations(string operations)
        {
            IsNullOrEmpty(operations);
            IsValidCharacters(operations);
        }


        private void IsNullOrEmpty(string operations)
        {
            if (operations == null || Regex.IsMatch(operations, @"^[\s]{0,}$"))
            {
                throw new ArgumentNullException("Your Input is empty");
            }
        }
        

        private void IsValidCharacters(string operations)
        {
            if (!Regex.IsMatch(operations, @"^[a-eA-E\s]+$"))
            {
                throw new ArgumentException("Your Input is not valid");
            }
        }
    
    }
}
