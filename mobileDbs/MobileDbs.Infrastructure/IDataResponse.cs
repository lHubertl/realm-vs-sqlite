namespace MobileDbs.Infrastructure
{
    public interface IDataResponse<T> : IResponse
    {
        T Data { get; set; }
    }
}
