using System;
using System.Collections.Generic;

namespace SagePay.Helpers
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + " ..";
        }

        public static Dictionary<string, string> ExtractDataFromSageResponse(this string responseString,string seperator)
        {
            var data = new Dictionary<string, string>();

            foreach (var line in responseString.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries))
            {
                var splitter = line.IndexOf("=", StringComparison.Ordinal);//todo:test multi = in a line
                data.Add(line.Substring(0, splitter), line.Substring(splitter + 1));
            }
            return data;
        }
    }
}
