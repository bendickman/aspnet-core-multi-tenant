namespace MultiTenant.Core.DTOs
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public static AuthenticationResult Error(IEnumerable<string> errors)
        {
            return new AuthenticationResult
            {
                Errors = errors,
            };
        }

        public static AuthenticationResult Success(string token)
        {
            return new AuthenticationResult
            {
                Token = token,
                IsSuccess = true,
                Errors = Enumerable.Empty<string>(),
            };
        }
    }
}
