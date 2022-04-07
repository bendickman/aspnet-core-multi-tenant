namespace MultiTenant.Core.Interfaces
{
    public interface IResponse
    {
        bool IsSuccess { get; }

        IDictionary<string, IList<string>> Errors { get; }
    }
}
