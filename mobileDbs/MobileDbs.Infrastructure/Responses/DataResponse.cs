namespace MobileDbs.Infrastructure.Responses
{
    public class DataResponse<T> : Response, IDataResponse<T>
    {
        public T Data { get; set; }

        public DataResponse(T data, bool isSuccess, string message = null) : base(isSuccess, message)
        {
            Data = data;
        }
    }
}
