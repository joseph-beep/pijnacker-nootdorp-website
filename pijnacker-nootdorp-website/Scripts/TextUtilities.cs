public static class TextUtilities
{
    public static string BeautifyNumber(int number)
    {
        return string.Format("{0:n0}", number);
    }

    public static string GetCheckboxText(bool boolean)
    {
        return boolean ? "checked" : "";
    }
    public static bool ParseCheckboxText(string value)
    {
        return value == "on" ? true : false;
    }
}