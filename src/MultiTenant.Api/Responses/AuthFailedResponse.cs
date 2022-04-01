namespace MultiTenant.Api.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public static AuthFailedResponse Error(IEnumerable<string> errors)
        {
            return new AuthFailedResponse
            {
                Errors = errors,
            };
        }
    }
}
