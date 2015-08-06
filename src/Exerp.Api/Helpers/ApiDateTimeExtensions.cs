using System;
using System.Globalization;

namespace Exerp.Api.Helpers
{
    public static class  ApiDateTimeExtensions
    {

        public static string ToApiTime(this DateTime input)
        {
            return input.ToString(Constants.ApiTimeFormat);
        }

        public static string ToApiDate(this DateTime input)
        {
            return input.ToString(Constants.ApiDateFormat);
        }
 
        public static DateTime FromApiDateString(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return DateTime.MinValue;

            return DateTime.ParseExact(input, Constants.ApiDateFormat, new CultureInfo("en-US"));
        }

        public static DateTime FromApiDateTimeString(this string inputDate,string inputTime)
        {
            if (string.IsNullOrWhiteSpace(inputDate) || string.IsNullOrWhiteSpace(inputTime))
                return DateTime.MinValue;

            return DateTime.ParseExact(string.Concat(inputDate, " ", inputTime), string.Format("{0} {1}", Constants.ApiDateFormat, Constants.ApiTimeFormat), new CultureInfo("en-US"));
        }

    }
}
