namespace Clone_Backend_Twitter.Models.Response;

public class ResponseModel<T>
{
    public string Message { get; set; } = string.Empty;
    public Boolean Status { get; set; } = true;
    public T? Data { get; set; }
}