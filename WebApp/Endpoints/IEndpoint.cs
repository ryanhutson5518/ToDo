namespace WebApp.Endpoints;

public interface IEndpoint
{
    string Pattern { get; }

    HttpMethod HttpMethod { get; }

    Delegate Handler { get; }
}
