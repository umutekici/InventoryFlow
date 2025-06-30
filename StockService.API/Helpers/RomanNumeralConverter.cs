namespace StockService.API.Helpers
{
    public class RomanNumeralConverter
    {
        public string ToRoman(int number)
        {
            if (number < 1 || number > 11)
                return "";
           
            var romanMap = new[]
            {           
                new { Value = 10,   Symbol = "X" },
                new { Value = 9,    Symbol = "IX" },
                new { Value = 5,    Symbol = "V" },
                new { Value = 4,    Symbol = "IV" },
                new { Value = 1,    Symbol = "I" }
            };

            var result = new System.Text.StringBuilder();
            foreach (var item in romanMap)
            {
                while (number >= item.Value)
                {
                    result.Append(item.Symbol);
                    number -= item.Value;
                }
            }

            return result.ToString();
        }
    }
}
