namespace ToDo.Endpoints;

public interface IEndpoint
{
    string Pattern { get; }

    HttpMethod HttpMethod { get; }

    Delegate Handler { get; }
}
