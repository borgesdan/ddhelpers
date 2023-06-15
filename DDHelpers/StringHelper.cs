using System.Globalization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DDHelpers
{
    public static class StringHelper
    {
        public static readonly int CPF_LENGTH = 11;
        public static readonly int CNPJ_LENGTH = 14;
        public static readonly int MIN_PHONE_LENGTH = 10;
        public static readonly int MAX_PHONE_LENGTH = 11;

        public static bool HasOnlyNumbers(this string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            var length = text.Length;            

            for(var i = 0; i < length; ++i)
            {
                if (!char.IsNumber(text[i]))
                    return false;
            }

            return true;
        }

        public static bool HasOnlyLetters(this string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            var length = text.Length;

            for (var i = 0; i < length; ++i)
            {
                if (!char.IsLetter(text[i]))
                    return false;
            }

            return true;
        }

        public static string? ApplyOnlyLetter(this string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var length = text.Length;
            var builder = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
            {
                var c = text[i];

                if (char.IsLetter(c))
                    builder.Append(c);
            }

            return builder.ToString();
        }

        public static string? ApplyOnlyLetterOrWhiteSpace(this string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var length = text.Length;
            var builder = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
            {
                var c = text[i];

                if (char.IsLetter(c) || char.IsWhiteSpace(c))
                    builder.Append(c);
            }

            return builder.ToString();
        }

        public static string? ApplyOnlyNumber(this string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var length = text.Length;
            var builder = new StringBuilder();

            for (var i = 0; i < length; ++i)
            {
                var c = text[i];

                if (char.IsNumber(c))
                    builder.Append(c);
            }

            return builder.ToString();
        }

        public static string? ApplyOnlyNumberOrWhiteSpace(this string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var length = text.Length;
            var builder = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
            {
                var c = text[i];

                if (char.IsNumber(c) || char.IsWhiteSpace(c))
                    builder.Append(c);
            }

            return builder.ToString();
        }

        public static string? ApplyOnlyNumberOrLetter(this string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var length = text.Length;
            var builder = new StringBuilder();

            for (var i = 0; i < length; ++i)
            {
                var c = text[i];

                if(char.IsNumber(c) || char.IsLetter(c))
                    builder.Append(c);
            }

            return builder.ToString();
        }

        public static string? ApplyOnlyNumberOrChars(this string? text, char[] selectedChars)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var length = text.Length;
            var builder = new StringBuilder();

            for (var i = 0; i < length; ++i)
            {
                var c = text[i];

                if (char.IsNumber(c) || selectedChars.Contains(c))
                    builder.Append(c);
            }

            return builder.ToString();
        }

        public static string? RemoveDiacritics(this string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var length = normalizedString.Length;
            var stringBuilder = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                var c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        public static string? ApplyPhoneMask(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value.Length < MIN_PHONE_LENGTH || value.Length > MAX_PHONE_LENGTH)
                return value;

            var phone = value;
            var ddd = phone.Substring(0, 2);
            var number = phone.Substring(ddd.Length, phone.Length - ddd.Length);

            var lastNumbers = number.Substring(number.Length - 4);
            var firstNumbers = number.Substring(0, number.Length - lastNumbers.Length);

            return string.Concat("(", ddd, ") ", firstNumbers, "-", lastNumbers);
        }

        public static string? ApplyCPFMask(this string? value)
        { 
            if (string.IsNullOrWhiteSpace(value) || value.Length != CPF_LENGTH)
                return value;

            return string.Concat(
                value.Substring(0, 3), ".",
                value.Substring(3, 3), ".",
                value.Substring(6, 3), "-",
                value.Substring(9, 2));
        }

        public static string? ApplyCNPJMask(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != CNPJ_LENGTH)
                return value;

            return string.Concat(
                   value.Substring(0, 2), ".",
                   value.Substring(2, 3), ".",
                   value.Substring(5, 3), "/",
                   value.Substring(8, 4), "-",
                   value.Substring(12, 2));
        }

        public static string? RemoveSelectedChars(this string? text, char[] selectedChars)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var length = text.Length;
            var builder = new StringBuilder();

            for (var i = 0; i < length; ++i)
            {
                var c = text[i];

                if (!selectedChars.Contains(c))
                    builder.Append(c);
            }

            return builder.ToString();
        }

        public static bool IsValidCpf(this string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        public static bool IsValidCnpj(this string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        public static bool IsValidCreditCardNumber(this string? number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return false;

            var deltas = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
            var checksum = 0;
            var chars = number.ToCharArray();

            for (var i = chars.Length - 1; i > -1; i--)
            {
                var j = chars[i] - 48;
                checksum += j;
                if (((i - chars.Length) % 2) == 0)
                    checksum += deltas[j];
            }

            return ((checksum % 10) == 0);
        }
    }
}