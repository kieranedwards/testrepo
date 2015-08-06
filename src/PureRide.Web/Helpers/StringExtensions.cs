using System.Globalization;

namespace PureRide.Web.Helpers
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            return textInfo.ToTitleCase(input);
        }


        public static string ToStudioSlug(this string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return location;

            return string.Concat(location.Replace(" ", "-").ToLower(),"-studio");
        }

        public static string FromStudioSlug(this string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return location;

            return location.Replace("-", " ").ToTitleCase();
        }

    }

}
