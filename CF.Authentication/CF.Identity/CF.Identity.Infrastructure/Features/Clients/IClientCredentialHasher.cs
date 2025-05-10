using System.Text;

namespace CF.Identity.Infrastructure.Features.Clients;

public interface IClientCredentialHasher : IDomainHasher<IClient>
{
    
}

public class ClientCredentialHasher(Encoding? encoding) : DomainHasherBase<IClient>(encoding), IClientCredentialHasher
{
    protected override Func<IClient, string?> SecretProperty => x => x.SecretHash;
    protected override string GenerateSalt(IClient client)
    {
        //Uses two things that can't be changed to create a persistent salt
        return $"{client.Reference}-{client.Id}";
    }
}