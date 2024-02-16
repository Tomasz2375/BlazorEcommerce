namespace BlazorEcommerce.Shared;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Sucess { get; set; }
    public string Message { get; set; } = string.Empty;
}
