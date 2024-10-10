namespace Store.Route.APIs.Errors
{
    public class ApiErrorResponse
    {
        

        public int StatusCode { get; set; }
        public string? Message { get; set; }
        
        public ApiErrorResponse(int statusCode, string? message = null)
        {
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            StatusCode = statusCode;
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "bad request you have made",
                401 => "Authorized, you are not",
                404 => "Resource Was Not Found",
                500 => "Server Error",
                _ => null
            };



            return message;
        }
    }
}
