namespace MultiTenant.Api.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public static AuthSuccessResponse Success(
            string token,
            string refreshToken)
        {
            return new AuthSuccessResponse
            {
                Token = token,
                RefreshToken = refreshToken,
            };
        }
    }
}
