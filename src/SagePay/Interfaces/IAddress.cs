namespace SagePay.Interfaces
{
    public interface IAddress
    {
        string Surname { get; set; }
        string FirstNames { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string PostCode { get; set; }
        string Country { get; set; }
    }
}