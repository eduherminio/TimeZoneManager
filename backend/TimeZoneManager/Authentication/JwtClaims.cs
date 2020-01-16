namespace TimeZoneManager.Authentication
{
    internal static class CustomClaimTypes
    {
        public const string Name = nameof(CustomClaimTypesName.name);
        public const string Permissions = nameof(CustomClaimTypesName.prms);
    }

    /// <summary>
    /// These names are the keys in token payload
    /// </summary>
    public enum CustomClaimTypesName
    {
        name,
        prms
    }
}
