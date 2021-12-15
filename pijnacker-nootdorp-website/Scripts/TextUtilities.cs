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

    public static string TranslateRoomType(string roomType, bool plural)
    {
        switch (roomType)
        {
            case "BEDROOM":
                return plural ? "Slaapkamers" : "Slaapkamer";

            case "LIVING_ROOM":
                return plural ? "Woonkamers" : "Woonkamer";

            case "KITCHEN":
                return plural ? "Keukens" : "Keuken";

            case "BATHROOM":
                return plural ? "Badkamers" : "Badkamer";

            case "TOILET":
                return plural ? "Toilets" : "Toilets";

            case "WORKSPACE":
                return plural ? "Werkkamers" : "Werkkamer";
        }

        return roomType;
    }

    public static string TranslateStorey(int storey)
    {
        switch (storey)
        {
            case 0:
                return "Begane grond";

            case 1:
                return "Eerste verdieping";

            case 2:
                return "Tweede verdieping";

            case 3:
                return "Derde verdieping";
        }

        return $"Verdieping {storey}";
    }
}