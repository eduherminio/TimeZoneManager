using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TimeZoneManager.Orm
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TransactionInterceptorAttribute : AbstractInterceptorAttribute
    {
        [FromServiceContext]
#pragma warning disable RCS1170 // Use read-only auto-implemented property. - Public set needed for DI
        public ILogger<TransactionInterceptorAttribute> Logger { get; set; }
#pragma warning restore RCS1170 // Use read-only auto-implemented property.

        public DbContext GetDbContext(AspectContext aspectContext)
        {
            return aspectContext.ServiceProvider.GetRequiredService<IDatabaseContextContainer>().Context;
        }

        public async override Task Invoke(AspectContext aspectContext, AspectDelegate next)
        {
            var context = GetDbContext(aspectContext);
            bool existsTransaction = (context.Database.CurrentTransaction != null);
            var transaction = context.Database.CurrentTransaction ?? context.Database.BeginTransaction();

            try
            {
                await next(aspectContext).ConfigureAwait(false);

                if (!existsTransaction)
                {
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                try
                {
                    if (!existsTransaction)
                    {
                        transaction.Rollback();
                    }
                }
                catch
                {
                    Logger.LogError("Error rolling back transaction");
                }
                throw;
            }
            finally
            {
                if (!existsTransaction)
                {
                    transaction.Dispose();
                }
            }
        }
    }
}
