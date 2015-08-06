using System;
using System.Linq;

namespace PureRide.Web.Providers
{
    /// <remarks>
    /// Based on http://stackoverflow.com/questions/9601593/how-can-i-format-07-03-2012-to-march-7th-2012-in-c-sharp 
    /// </remarks>
    public class DisplayDateProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!(arg is DateTime))
                throw new NotSupportedException();

            var dt = (DateTime)arg;

            if (dt.Date == DateTime.Today.Date)
                return "Today";

            if (dt.Date == DateTime.Today.AddDays(1).Date)
                return "Tomorrow";

            return string.Format("{0} {1:dd}/{1:MM}", dt.DayOfWeek.ToString().Substring(0, 3), dt);
        }
    }
}