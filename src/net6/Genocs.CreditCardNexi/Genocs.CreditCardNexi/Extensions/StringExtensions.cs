namespace Genocs.CreditCardNexi.Extensions
{
    public static class StringExtensions
    {
        public static bool IsCVV2Valid(this string input)
        {

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            if (!input.All(Char.IsDigit))
            {
                return false;
            }

            if (input.Length == 3 || input.Length == 4)
            {
                return true;
            }

            return false;
        }
    }
}
