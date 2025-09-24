namespace MovieRental.Common
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string? Details { get; set; }

        //TODO improve error handling structure
        public ErrorResponse(string message, string? details = null)
        {
            Message = message;
            Details = details;
        }
    }
}