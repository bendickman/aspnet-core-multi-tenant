using MultiTenant.Core.Interfaces;

namespace MultiTenant.Core.Responses
{
    public class ListResponse<T> : IListResponse<T>
    {
        public List<T> Results { get; set; }

        public bool IsSuccess { get; set; }

        public IDictionary<string, IList<string>> Errors { get; set; }

        public Pagination Pagination { get; set; }

        public static ListResponse<T> Empty()
        {
            return new ListResponse<T>
            {
                Results = new List<T>(),
                Pagination = new Pagination(0, 0, 0),
                Errors = new Dictionary<string, IList<string>>(),
                IsSuccess = true,
            };
        }
    }
}
