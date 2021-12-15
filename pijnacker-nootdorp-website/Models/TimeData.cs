using System;

public class TimeData
{
    public int minute; // 0-59
    public int hour; // 0-23
    public int day;
    public int month; // 1-12
    public int year;

    // FORMAT: year-month-day hour:minute:second
    public TimeData(DateTime raw)
    {
        year = raw.Year;
        month = raw.Month;
        day = raw.Day;
        hour = raw.Hour;
        minute = raw.Minute;
    }

    public static string ConvertToGoogleCalendarFormat(TimeData start, TimeData end)
    {
        string startYear = start.year.ToString();
        string startMonth = start.month.ToString();
        string startDay = start.day.ToString();
        string startHour = (start.hour - 1).ToString();
        string startMinute = start.minute.ToString();

        if (startMonth.Length == 1) startMonth = "0" + startMonth;
        if (startDay.Length == 1) startDay = "0" + startDay;
        if (startHour.Length == 1) startHour = "0" + startHour;
        if (startMinute.Length == 1) startMinute = "0" + startMinute;

        string endYear = end.year.ToString();
        string endMonth = end.month.ToString();
        string endDay = end.day.ToString();
        string endHour = (end.hour - 1).ToString();
        string endMinute = end.minute.ToString();

        if (endMonth.Length == 1) endMonth = "0" + endMonth;
        if (endDay.Length == 1) endDay = "0" + endDay;
        if (endHour.Length == 1) endHour = "0" + endHour;
        if (endMinute.Length == 1) endMinute = "0" + endMinute;

        return $"{startYear}{startMonth}{startDay}T{startHour}{startMinute}00Z%2F{endYear}{endMonth}{endDay}T{endHour}{endMinute}00Z";
    }
}