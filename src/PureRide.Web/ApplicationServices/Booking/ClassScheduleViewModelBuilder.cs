using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.Configuration;
using PureRide.Web.Helpers;
using PureRide.Web.Providers;
using PureRide.Web.ViewModels.Booking;

namespace PureRide.Web.ApplicationServices.Booking
{

    public class ClassScheduleViewModelBuilder : IClassScheduleViewModelBuilder
    {
        private readonly ICentreService _centreService;
        private readonly IBookingService _bookingService;
        private readonly IScheduleSettings _scheduleSettings;
        private readonly IScheduledClassModelAdapter _scheduledClassModelAdapter;

        public ClassScheduleViewModelBuilder(ICentreService centreService, 
            IBookingService bookingService,
            IScheduleSettings scheduleSettings, 
            IScheduledClassModelAdapter scheduledClassModelAdapter)
        {
            _centreService = centreService;
            _bookingService = bookingService;
            _scheduleSettings = scheduleSettings;
            _scheduledClassModelAdapter = scheduledClassModelAdapter;
        }

        public ClassScheduleModel BuildModel(string location)  
        {
            location = location.FromStudioSlug();

            var centre = _centreService.GetActiveCentreByName(location);

            if (centre == null)
                return null;

            var results = _bookingService.GetClassListForCentre(centre.CenterId, DateTime.Today, DateTime.Today.AddDays(_scheduleSettings.MaxVisibleDays));

            return NewClassScheduleModel(results, centre.WebName);
        }

        private ClassScheduleModel NewClassScheduleModel(IEnumerable<ScheduledClass> results, string location)  
        {
            return new ClassScheduleModel
            {
                Location = location,
                Days = results.OrderBy(a=>a.StartTime).GroupBy(p => p.StartTime.Date,
                    p => p,
                    (key, g) => new ClassDayModel()
                    {
                        Date = key,
                        DisplayName = string.Format(new DisplayDateProvider(), "{0}", key),
                        Classes = g.Select(c => _scheduledClassModelAdapter.Create(c))
                    })
            };
        }

    }
    
    public interface IClassScheduleViewModelBuilder
    {
        ClassScheduleModel BuildModel(string location);
    }
}