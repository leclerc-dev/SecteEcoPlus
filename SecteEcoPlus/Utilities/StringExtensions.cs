using System.Linq;

namespace SecteEcoPlus.Utilities
{
    public static class StringExtensions
    {
        public static string AdaptToRoute(this string s)
        {
            var chars = s.Where(FilterCharacter).Select(ProcessCharToNormalize).ToArray();
            return new string(chars);
        }

        private static bool FilterCharacter(char c) => !char.IsPunctuation(c);
        private static char ProcessCharToNormalize(char c)
        {
            if (char.IsWhiteSpace(c)) return '-';
            if (char.IsLetterOrDigit(c) || char.IsSymbol(c) || char.IsSeparator(c)) return c;
            return '-';
        }
    }
}