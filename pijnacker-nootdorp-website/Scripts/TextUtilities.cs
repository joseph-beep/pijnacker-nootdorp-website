public static class TextUtilities
{
    public static string BeautifyNumber(int number)
    {
        return string.Format("{0:n0}", number);
    }
}