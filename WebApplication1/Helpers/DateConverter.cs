using System;
using System.Globalization;

namespace API.Helpers
{
    public class DateConverter
    {
        public static DateTime StringToDateTime(string dateString)
        {
            string dateFormats = "yyyy/MM/dd";
            CultureInfo provider = new CultureInfo("en-US");
            DateTime.TryParseExact(dateString, dateFormats, provider, DateTimeStyles.AdjustToUniversal, out DateTime date);

            return date;
        }
    }
}
