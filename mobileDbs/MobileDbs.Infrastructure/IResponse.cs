namespace MobileDbs.Infrastructure
{
    public interface IResponse
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
    }
}
