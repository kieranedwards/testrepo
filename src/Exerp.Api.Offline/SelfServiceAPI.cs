using System.Collections.Generic;
using System.Linq;
using Exerp.Api.Interfaces;

namespace Exerp.Api.Local
{
    public class SelfServiceAPI  
    {
        private Dictionary<int, SampleUser> _sampleUsers = new Dictionary<int, SampleUser>();

        public SelfServiceAPI()
        {
            _sampleUsers.Add(123,new SampleUser("test@test.local","1234"));
            _sampleUsers.Add(124, new SampleUser("test1@test.local", "1234"));
            _sampleUsers.Add(125, new SampleUser("test2@test.local", "1234"));
        }

        public bool UpdatePassword(int personId, string currentPassword, string newPassword)
        {
            if (ValidatePassword(personId, currentPassword))
            {
                _sampleUsers[personId].Password = newPassword;
                return true;
            }

            return false;
        }

        public bool ValidatePassword(int personId, string password)
        {
           if(_sampleUsers.ContainsKey(personId))
               return _sampleUsers[personId].Password == password;
           
            return false;
        }

        public bool SetPassword(int personId, string newPassword, string comment)
        {
            if (_sampleUsers.ContainsKey(personId))
                _sampleUsers[personId].Password = newPassword;

            return true;
        }

        public int? FindPersonByEmail(string email)
        {
            var result = _sampleUsers.Where(a => string.Compare(a.Value.Email, email, true) == 0);
            return result.Any() ? result.Single().Key : new int?();
        }


        private class SampleUser
        {
            public string Email { get; private set; }
            public string Password { get; set; }

            public SampleUser(string email, string password)
            {
                Email = email;
                Password = password;
            }
        }


    }
}
