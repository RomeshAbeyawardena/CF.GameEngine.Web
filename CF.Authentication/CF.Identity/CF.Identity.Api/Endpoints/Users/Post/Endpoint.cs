namespace CF.Identity.Api.Endpoints.Users.Post;

public static class Endpoint
{
    public static async Task<IResult> SaveUserAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;


        return Results.Ok();
    }

    public static IEndpointRouteBuilder AddPostEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/users", SaveUserAsync);
        return builder;
    }
}
