
namespace Ejercicio.Utilities.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class StringExtentions
    {
        public static HashSet<UnicodeCategory> Eliminados = new HashSet<UnicodeCategory>() {
            UnicodeCategory.NonSpacingMark,
            UnicodeCategory.ClosePunctuation,
            UnicodeCategory.ConnectorPunctuation,
            UnicodeCategory.CurrencySymbol,
            UnicodeCategory.DashPunctuation,
            UnicodeCategory.FinalQuotePunctuation,
            UnicodeCategory.InitialQuotePunctuation,
            UnicodeCategory.SpaceSeparator,
            UnicodeCategory.Surrogate,
            UnicodeCategory.OtherPunctuation,
            UnicodeCategory.MathSymbol,
            UnicodeCategory.ModifierSymbol,
            UnicodeCategory.OtherSymbol,
            UnicodeCategory.OpenPunctuation
        };

        public static string NormalizeDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => !Eliminados.Contains(CharUnicodeInfo.GetUnicodeCategory(c))).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string ToLowerCapitalCase(this string text) {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Substring(0, 1).ToLower() + text.Substring(1);
        }
        public static bool Like(this string toSearch, string toFind)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }
        public static string PrintTabulado(this string formatoPlano)
        {
            var formatoTabulado = string.Empty;
            var tabulacionActual = 0;

            foreach (var c in formatoPlano)
            {
                switch (c)
                {
                    case ConstTexto.CHAR_PARENTESIS_CIERRE:
                        tabulacionActual--;
                        formatoTabulado = SaltoLinea(formatoTabulado, tabulacionActual);
                        break;
                    case ConstTexto.CHAR_PARENTESIS_APERTURA:
                        //tabulacionActual++;
                        formatoTabulado = SaltoLinea(formatoTabulado, tabulacionActual);
                        break;
                }

                formatoTabulado += c;

                switch (c)
                {
                    case ConstTexto.CHAR_PARENTESIS_CIERRE:
                        formatoTabulado = SaltoLinea(formatoTabulado, tabulacionActual);
                        break;
                    case ConstTexto.CHAR_PARENTESIS_APERTURA:
                        tabulacionActual++;
                        formatoTabulado = SaltoLinea(formatoTabulado, tabulacionActual);
                        break;
                }
            }

            return formatoTabulado;
        }

        private static string SaltoLinea(string formatoTabulado, int tabulacionActual)
        {
            formatoTabulado += ConstTexto.CHAR_SALTO_DE_LINEA;
            formatoTabulado += new String(ConstTexto.CHAR_TABULADOR, tabulacionActual);
            return formatoTabulado;
        }

        public static string SpacesFromCamel(this string value)
        {
            if (value.Length > 0)
            {
                var result = new List<char>();
                char[] array = value.ToCharArray();
                foreach (var item in array)
                {
                    if (char.IsUpper(item))
                    {
                        result.Add(' ');
                    }
                    result.Add(item);
                }

                return new string(result.ToArray());
            }
            return value;
        }

    }
}
