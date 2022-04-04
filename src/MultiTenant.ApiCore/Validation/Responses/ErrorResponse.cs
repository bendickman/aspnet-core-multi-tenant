namespace MultiTenant.ApiCore.Validation.Responses
{
    public class ErrorResponse
    {
        public IList<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
