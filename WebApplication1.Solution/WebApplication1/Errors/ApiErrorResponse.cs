namespace WebApplication1.Api.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiErrorResponse(int statuscode , string? message = null)
        {
            this.StatusCode = statuscode ;
            Message = message?? GetDefaultErrorMessage(statuscode);
        }

        private string? GetDefaultErrorMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => " Bad Request ",
                401 => "You Are Not Authorized",
                404 => "Resources Not Found",
                500 => " Server Error Occured ",
                _ => null
            };
        }
    }
}
