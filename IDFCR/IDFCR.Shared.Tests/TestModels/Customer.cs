namespace IDFCR.Shared.Tests.TestModels;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string NameHmac { get; set; } = null!;
    public string NameCI { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string EmailHmac { get; set; } = null!;
    public string EmailCI { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string PhoneNumberCI { get; set; } = null!;
    public string PhoneNumberHmac { get; set; } = null!;
    public string RowVersion { get; set; } = null!;
    public string? MetaData { get; set; }
}
