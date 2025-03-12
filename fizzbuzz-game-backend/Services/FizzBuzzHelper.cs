using FizzBuzzGameBackend.Models;

namespace FizzBuzzGameBackend.Services
{
    public static class FizzBuzzHelper
    {
        public static string ComputeResult(int number, List<Rule> rules)
        {
            string result = "";

            foreach (var rule in rules.OrderBy(r => r.Id))
            {
                if (rule.Divisor == 0)
                {
                    continue;
                }

                if (number % rule.Divisor == 0)
                    result += rule.ReplacementText;
            }

            return string.IsNullOrEmpty(result) ? number.ToString() : result;
        }
    }
}
