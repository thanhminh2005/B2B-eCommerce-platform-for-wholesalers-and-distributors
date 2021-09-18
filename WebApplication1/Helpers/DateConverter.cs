using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
