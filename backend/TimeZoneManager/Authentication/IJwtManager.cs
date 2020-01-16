namespace TimeZoneManager.Authentication
{
    public interface IJwtManager
    {
        string GenerateToken(JwtTokenPayload payload);

        string GenerateToken(JwtTokenPayload payload, int minutesTimeout);

        JwtTokenPayload GetPayload(string authHeader);

        string GetTokenFromAuthorizationHeader(string authHeader);
    }
}
