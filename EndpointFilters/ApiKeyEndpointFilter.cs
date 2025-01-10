namespace TextToSpeech.EndpointFilters;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyEndpointFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        string? userApiKey = context.HttpContext.Request.Headers.Authorization;
        if (string.IsNullOrWhiteSpace(userApiKey) || !userApiKey.StartsWith("Bearer ")) 
        {
            return Results.BadRequest(new { Message = "API key is missing" });
        }

        userApiKey = userApiKey[7..];
        if (userApiKey != _configuration.GetValue<string>("API_AUTH_KEY"))
        {
            return Results.Unauthorized();
        }
        return await next(context);
    }
}