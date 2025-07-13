using System.Text.RegularExpressions;

namespace NationalCodeValidator;

public static class NationalCodeHelper
{
    public static bool IsValidateNationalCode(this string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return false;

        code = code.PadLeft(10, '0');
        if (!Regex.IsMatch(code, @"^\d{10}$")) return false;

        var check = int.Parse(code[9].ToString());
        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(code[i].ToString()) * (10 - i);

        var remainder = sum % 11;
        return (remainder < 2 && check == remainder) || (remainder >= 2 && check == (11 - remainder));
    }
}