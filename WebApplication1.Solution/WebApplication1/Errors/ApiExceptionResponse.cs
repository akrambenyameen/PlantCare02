namespace WebApplication1.Api.Errors
{
    public class ApiExceptionResponse : ApiErrorResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int StatusCode,string? message=null,string? details=null):base(StatusCode,message)
        {
            Details = details;
        }
    }
}
