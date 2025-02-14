namespace dev_refined
{
    public static class StringExtensions
    {
        public static string PadBoth(this string str, int length, char character)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft, character).PadRight(length, character);
        }
    }
}
