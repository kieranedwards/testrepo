using umbraco.providers.members;

namespace PureRide.Web
{
    public class PureRideMembershipProvider : UmbracoMembershipProvider 
    {
        public override bool ValidateUser(string username, string password)
        {
            return base.ValidateUser(username, password);
        }
        
    }
}
