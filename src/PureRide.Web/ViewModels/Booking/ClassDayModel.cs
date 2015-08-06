using System;
using System.Collections.Generic;

namespace PureRide.Web.ViewModels.Booking
{
    public class ClassDayModel
    {
        public string DisplayName { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<ScheduledClassModel> Classes { get; set; }
    }
}