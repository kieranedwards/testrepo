using System;
using System.Collections.Generic;

namespace Exerp.Api.DataTransfer
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class PersonDetails : Person
    {
        public DateTime Birthday { get; set; }
        public Address Address { get; set; }
        public bool IsMale { get; set; }
        public bool WishToReceiveThirdPartyOffers { get; set; }
        public string Password { get; set; }
        public bool AllowSms { get; set; }
        public string ShoeSize { get; set; }
        public IEnumerable<PersonFriend> Friends { get; set; }
    }
}
