using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Cryptography;
using System.Text;

namespace IDFCR.Shared.Tests;

[TestFixture]
internal class PIIProtectionProviderBaseTests
{
    [Test]
    public void Test()
    {
        var b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, null, "MyVeryLongApplicationSecret", "MyVeryLongClientApplicationSet");
        Assert.That(b.Item2, Has.Length.EqualTo(32));

        b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, null, "shortkey", "shtrky");
        Assert.That(b.Item2, Has.Length.EqualTo(32));

        var c = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, b.Item1, "shortkey", "shtrky");
        Assert.That(b.Item2, Has.Length.EqualTo(32));
        Assert.That(b.Item2, Is.EqualTo(c.Item2));

        b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, null, "Exactly 16 chars", "Exactly 16 chars");
        Assert.That(b.Item2, Has.Length.EqualTo(32));
    }


    public class Customer()
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClientId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string RowVersion { get; set; } = null!;
    }

    internal class MyProtectionModel: PIIProtectionBase<Customer>
    {
        protected override string GetKey(Customer entity)
        {
            //using something that should not change or collide
            return $"{entity.Id}:{entity.ClientId}";
        }

        public MyProtectionModel(Encoding encoding) : base(encoding)
        {
            SetRowVersion(x => x.RowVersion);

            For(x => x.Name, (provider, value, context) =>
            {
                var hmac = HashWithHMAC(GetKey(context), value);
                var caseImpressions = CasingImpression.Calculate(value);
                context.Name = Encrypt(value, UseAlgorithm(SymmetricAlgorithmName.Aes, context))!;
                return new DefaultProtectionInfo(hmac, caseImpressions);
            }, (provider, value, context, protectionInfo) =>
            {
                context.Name = Decrypt(value, UseAlgorithm(SymmetricAlgorithmName.Aes, context))!;
                context.Name = CasingImpression.Restore(context.Name, protectionInfo.CasingImpressions);
            });

            For(x => x.Email, (provider, value, context) =>
            {
                var hmac = HashWithHMAC(GetKey(context), value);
                var caseImpressions = CasingImpression.Calculate(value);
                context.Email = Encrypt(value, UseAlgorithm(SymmetricAlgorithmName.Aes, context))!;
                return new DefaultProtectionInfo(hmac, caseImpressions);
            }, (provider, value, context, protectionInfo) =>
            {
                context.Email = Decrypt(value, UseAlgorithm(SymmetricAlgorithmName.Aes, context))!;
                context.Email = CasingImpression.Restore(context.Email, protectionInfo.CasingImpressions);
            });

            For(x => x.Password, (provider, value, context) =>
            {
                var hmac = HashWithHMAC(GetKey(context), value);
                var caseImpressions = CasingImpression.Calculate(value);
                context.Password = Hash(System.Security.Cryptography.HashAlgorithmName.SHA384, "Mysecret", "A salt", 64);//TODO this would come from config or generated using secret known values
                return new DefaultProtectionInfo(hmac, caseImpressions);
            }, (provider, value, context, protectionInfo) =>
            {
                //one-way hash
            });

            For(x => x.PhoneNumber, (provider, value, context) =>
            {
                var hmac = HashWithHMAC(GetKey(context), value);
                var caseImpressions = CasingImpression.Calculate(value);
                context.PhoneNumber = Encrypt(value, UseAlgorithm(SymmetricAlgorithmName.Aes, context))!;
                return new DefaultProtectionInfo(hmac, caseImpressions);
            }, (provider, value, context, protectionInfo) =>
            {
                context.PhoneNumber = Decrypt(value, UseAlgorithm(SymmetricAlgorithmName.Aes, context))!;
                context.PhoneNumber = CasingImpression.Restore(context.PhoneNumber, protectionInfo.CasingImpressions);
            });
        }
    }
}
