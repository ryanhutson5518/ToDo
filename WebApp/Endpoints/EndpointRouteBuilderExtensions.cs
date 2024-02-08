namespace WebApp.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && t.IsAbstract == false);

        foreach (var type in types)
        {
            if (type.IsAssignableTo(typeof(IEndpoint)))
            {
                var endpoint = (type.FullName == null ? null : type.Assembly.CreateInstance(type.FullName)) as IEndpoint;
                if (endpoint is not null)
                {
                    endpointRouteBuilder.MapMethods(
                        endpoint.Pattern,
                        new[] { endpoint.HttpMethod.ToString() },
                        endpoint.Handler);
                }
            }
        }
    }
}
