using System;

namespace Exerp.Api.DataTransfer
{
    public class ScheduledClass
    {

        public int ActivityId { get; set; }

        public int BookedCount{ get; set; }

        public Identity BookingId{ get; set; }

        public int ClassCapacity{ get; set; }

        public string Description{ get; set; }

        public string InstructorName{ get; set; }
        
        public string Name{ get; set; }

        public string RoomName{ get; set; }
        
        public DateTime StartTime{ get; set; }

        public DateTime EndTime { get; set; }

        public int WaitingListCount{ get; set; }
    }
}
