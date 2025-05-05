using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IDFCR.Shared.Http.SwaggerGen;

public class OpenApiContactDocumentFilter(IHttpContextAccessor accessor) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var config = accessor.HttpContext?.RequestServices.GetRequiredService<IConfiguration>();

        var email = config?.GetValue<string>("OpenApi:Contact:Email");
        var name = config?.GetValue<string>("OpenApi:Contact:Name");

        var contact = new OpenApiContact();

        if (!string.IsNullOrWhiteSpace(email))
        {
            contact.Email = email;
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            contact.Name = name;
        }

        swaggerDoc.Info.Contact = contact;
    }
}
