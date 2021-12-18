using System;
using static HouseLayout;

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

    public static string TranslateRoomType(RoomType roomType, bool plural)
    {
        switch (roomType)
        {
            case RoomType.BEDROOM:
                return plural ? "Slaapkamers" : "Slaapkamer";

            case RoomType.LIVING_ROOM:
                return plural ? "Woonkamers" : "Woonkamer";

            case RoomType.KITCHEN:
                return plural ? "Keukens" : "Keuken";

            case RoomType.BATHROOM:
                return plural ? "Badkamers" : "Badkamer";

            case RoomType.TOILET:
                return plural ? "Toilets" : "Toilets";

            case RoomType.WORKSPACE:
                return plural ? "Werkkamers" : "Werkkamer";

            case RoomType.DINING_ROOM:
                return plural ? "Eetkamers" : "Eetkamer";
        }

        return roomType.ToString();
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