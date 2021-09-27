using System;
using System.Globalization;

namespace API.Helpers
{
    public class DateConverter
    {
        public static DateTime StringToDateTime(string dateString)
        {
            DateTime date;
            string dateFormats = "yyyy/MM/dd";
            CultureInfo provider = new CultureInfo("en-US");
            DateTime.TryParseExact(dateString, dateFormats, provider, DateTimeStyles.AdjustToUniversal, out date);

            return date;
        }
    }
}
