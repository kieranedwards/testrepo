using System;

namespace Exerp.Api.DataTransfer
{
    public class Identity
    {
        private int _centreId;
        private int _entityId;

        public int CentreId
        {
            get { return _centreId; }
        }

        public int EntityId
        {
            get { return _entityId; }
        }

        public Identity(int centreId, int entityId)
        {
            _centreId = centreId;
            _entityId = entityId;
        }

        public Identity(string formatedIdentity) : this(formatedIdentity,true)
        {
        }

        private Identity(string formatedIdentity,bool throwError)
        {
            var parts = formatedIdentity.Split(':');

            if (ValidateAndAssignParts(parts) && throwError)
                throw new ArgumentException("formatedIdentity is not in a valid format", "formatedIdentity");
        }


        private bool ValidateAndAssignParts(string[] parts)
        {
            return (parts.Length != 2) || !int.TryParse(parts[0],out _centreId) || !int.TryParse(parts[1], out _entityId);
        }

        public override string ToString()
        {
            return string.Concat(CentreId,":",EntityId); 
        }

        public override bool Equals(object obj)
        {
            var compare = (Identity)obj;
            return this.CentreId == compare.CentreId && this.EntityId == compare.EntityId;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool IsValid(string formatedPersonIdentity)
        {
            var testValue = new Identity(formatedPersonIdentity,false);
            return (testValue.CentreId > 0 && testValue.EntityId > 0);
        }
    }
}
