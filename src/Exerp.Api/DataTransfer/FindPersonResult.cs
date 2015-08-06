namespace Exerp.Api.DataTransfer
{
    public class FindPersonResult
    {
        public FindPersonStatus FoundState { get; set; }
        public Identity PersonId { get; set; }
    }


    public enum FindPersonStatus
    {
        NotFound,
        FriendOnly,
        FullAccount
    }
}
