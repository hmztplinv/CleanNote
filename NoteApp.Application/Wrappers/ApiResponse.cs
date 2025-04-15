namespace NoteApp.Application.Wrappers;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public List<string>? Errors { get; set; }
    public string? Message { get; set; }

    public ApiResponse() { }

    public ApiResponse(T data)
    {
        Data = data;
    }

    public ApiResponse(string error)
    {
        Success = false;
        Errors = new List<string> { error };
    }
}
