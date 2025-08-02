namespace SharedKernel.Transversal.Responses;

public class ResponseGeneric<T>
{
    public T Data { get; set; } = default!;
    public bool IsSuccess { get; set; } = false;
    public string Message { get; set; } = default!;
}
