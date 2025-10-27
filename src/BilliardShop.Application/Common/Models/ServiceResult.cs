namespace BilliardShop.Application.Common.Models;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ServiceResult<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static ServiceResult<T> Failure(string errorMessage) => new() { IsSuccess = false, ErrorMessage = errorMessage };
    public static ServiceResult<T> Failure(List<string> errors) => new() { IsSuccess = false, Errors = errors };
}

public class ServiceResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ServiceResult Success() => new() { IsSuccess = true };
    public static ServiceResult Failure(string errorMessage) => new() { IsSuccess = false, ErrorMessage = errorMessage };
    public static ServiceResult Failure(List<string> errors) => new() { IsSuccess = false, Errors = errors };
}