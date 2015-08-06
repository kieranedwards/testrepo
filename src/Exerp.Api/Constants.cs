namespace Exerp.Api
{
   public class Constants
   {
       internal const string ApiDateFormat = "yyyy-MM-dd";
       internal const string ApiTimeFormat = "HH:mm";

       //Extended Attributes
       internal const string ShoeSizeEaKey = "SHOE_SIZE";
       internal const string TermsEaKey = "TERMS_ACCEPTED";
       internal const string HealthEaKey = "PARQ_ACCEPTED";

       public const int MaxLengthCampaignCode = 25; //Docs say more however errors if you use more than this
       public const int MaxLengthName = 50; //Docs say more however errors if you use more than this
       public const int MaxLengthPassword = 50;
       public const int MaxLengthEmail = 50;
   }
}
