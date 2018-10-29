namespace MobileDbs.Infrastructure.Responses
{
    public class Response : IResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public Response(bool isSuccess, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
