namespace MultiTenant.Api.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }

        public static AuthSuccessResponse Success(string token)
        {
            return new AuthSuccessResponse
            {
                Token = token,
            };
        }
    }
}
