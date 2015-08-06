using Exerp.Api.WebServices.BookingAPI;

namespace Exerp.Api.DataTransfer
{
    public class Participation
    {

        public ScheduledClass ScheduledClass { get; set; }

        public bool CanShowUp { get; set; }

        public Identity ParticipationId { get; set; }

        public Identity PersonId { get; set; }

        public Seat Seat { get; set; }

        public participationState State { get; set; }

        public int WaitingListIndex { get; set; }

    }
}
