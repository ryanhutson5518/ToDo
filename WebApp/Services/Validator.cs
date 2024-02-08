namespace WebApp.Services;

public interface IValidator
{
    ValidationResult Validate<TDto>(TDto dto);

    Task<ValidationResult> ValidateAsync<TDto>(TDto dto, CancellationToken cancellationToken = default);
}

public class Validator(
    IServiceProvider serviceProvider)
    : IValidator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public ValidationResult Validate<TDto>(TDto dto)
    {
        var fluentValidator = _serviceProvider.GetRequiredService<FluentValidation.IValidator<TDto>>();
        var fluentValidationResult = fluentValidator.Validate(dto);
        return MapResult(fluentValidationResult);
    }

    public async Task<ValidationResult> ValidateAsync<TDto>(TDto dto, CancellationToken cancellationToken = default)
    {
        var fluentValidator = _serviceProvider.GetRequiredService<FluentValidation.IValidator<TDto>>();
        var fluentValidationResult = await fluentValidator.ValidateAsync(dto, cancellationToken);
        return MapResult(fluentValidationResult);
    }

    private static ValidationResult MapResult(FluentValidation.Results.ValidationResult fluentValidationResult)
    {
        var errors = fluentValidationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToHashSet());

        return new ValidationResult(fluentValidationResult.IsValid, errors);
    }
}

public record ValidationResult(
    bool IsValid,
    Dictionary<string, HashSet<string>> Errors);
