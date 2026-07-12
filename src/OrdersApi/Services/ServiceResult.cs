namespace OrdersApi.Services;

/// <summary>Тип ошибки операции сервисного слоя.</summary>
public enum ServiceErrorType
{
    None,
    NotFound,
    ValidationError
}

/// <summary>Результат операции сервисного слоя.</summary>
public class ServiceResult<T>
{
    public T? Value { get; private init; }

    public ServiceErrorType ErrorType { get; private init; }

    public string? ErrorMessage { get; private init; }

    public bool IsSuccess => ErrorType == ServiceErrorType.None;

    public static ServiceResult<T> Ok(T value) =>
        new() { Value = value };

    public static ServiceResult<T> NotFound() =>
        new() { ErrorType = ServiceErrorType.NotFound };

    public static ServiceResult<T> Invalid(string message) =>
        new() { ErrorType = ServiceErrorType.ValidationError, ErrorMessage = message };
}
