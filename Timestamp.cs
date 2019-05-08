using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CA_ShoppingCart.Util
{
    public class Timestamp
    {
        // We use unix timestamps to track our dates. This class provides some
        // helper methods to perform conversion.
       public static string[] monthNames = { "Jan", "Feb", "Mar",
            "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            public static long unixTimestamp()
            {
                return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }

            public static string dateFromTimestamp(long timestamp)
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();

                return dateTime.Day + " " + monthNames[dateTime.Month] + " " + dateTime.Year;
            }
        
    }
}