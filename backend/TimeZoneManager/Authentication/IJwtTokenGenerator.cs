namespace TimeZoneManager.Authentication
{
    internal interface IJwtTokenGenerator
    {
        string GenerateToken(JwtTokenPayload payload, string secret);

        string GenerateToken(JwtTokenPayload payload, string secret, int minutesTimeout);
    }
}
