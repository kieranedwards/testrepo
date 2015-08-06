namespace PureRide.Domain.Account.Entities
{
    public class User
    {
        public int? UserId { get; set; }

        public User(int? userId)
        {
            UserId = userId;
        }
        
    }
}
