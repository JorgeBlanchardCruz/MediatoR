namespace SharedKernel.Transversal.Responses;

public class Response<T> : ResponseGeneric<T>, IResponse
{
    private enum ResponseStatus
    {
        NotFound,
    }

    public static Response<T> NotFound()
    {
        return new Response<T>
        {
            IsSuccess = false,
            Message = $"{typeof(T).Name} {ResponseStatus.NotFound}"
        };
    }

}