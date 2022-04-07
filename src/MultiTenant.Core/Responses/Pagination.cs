namespace MultiTenant.Core.Responses
{
    public class Pagination
    {
        public Pagination(
            int currentPage,
            int pageSize,
            long totalResults)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalResults = totalResults;
        }
        public long TotalResults { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages
        {
            get
            {
                if (TotalResults.Equals(0) || PageSize.Equals(0))
                {
                    return 0;
                }

                return (int)Math.Ceiling((double)TotalResults / (double)PageSize);
            }
        }
    }
}
