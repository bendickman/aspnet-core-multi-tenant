using MultiTenant.Core.Responses;

namespace MultiTenant.Core.Interfaces
{
    public interface IListResponse<T> : IResponse
    {
        List<T> Results { get; }   

        Pagination Pagination { get; }
    }
}
