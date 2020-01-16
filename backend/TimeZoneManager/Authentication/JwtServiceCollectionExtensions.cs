using Microsoft.Extensions.DependencyInjection;
using TimeZoneManager.Authorization;

namespace TimeZoneManager.Authentication
{
    public static class JwtServiceCollectionExtensions
    {
        public static void AddJwtServices(this IServiceCollection services)
        {
            services.AddScoped<ISession, Session>();
            services.AddSingleton<IJwtManager, JwtManager>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        }
    }
}
