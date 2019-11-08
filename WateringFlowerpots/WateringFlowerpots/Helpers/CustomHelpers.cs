using System;
namespace WateringFlowerpots.Helpers
{
    public static class CustomHelpers
    {
        public static string[] GetWeekdays(DayOfWeek firstDayOfWeek)
        {
            string[] weekdays = new string[7];
            DateTime dateTime = DateTime.Now;
            while (dateTime.DayOfWeek != firstDayOfWeek)
            {
                dateTime = dateTime.AddDays(1);
            }
            for (int i = 0; i < 7; i++)
            {
                weekdays[i] = dateTime.DayOfWeek.ToString();
                dateTime = dateTime.AddDays(1);
            }
            return weekdays;
        }
    }
}
