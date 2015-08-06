using System.Web.Mvc;
using PureRide.Web.ApplicationServices.Booking;
using PureRide.Web.Helpers;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Controllers.Booking
{

   public class SeatSelectionController : RenderMvcController
    {
        private readonly ISeatSelectionModelBuilder _seatSelectionModelBuilder;
        
        public SeatSelectionController(ISeatSelectionModelBuilder seatSelectionModelBuilder)
        {
            _seatSelectionModelBuilder = seatSelectionModelBuilder;
        }

        public ActionResult SeatSelection(RenderModel model, string location, int classId)
        {
            var viewModel = _seatSelectionModelBuilder.BuildModel(location, classId);
            return base.Index(viewModel.AsRenderModel());
        }
    }
 
}
