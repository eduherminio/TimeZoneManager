using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TimeZoneManager.Model;
using TimeZoneManager.Orm;
using TimeZoneManager.Orm.Dao;

namespace TimeZoneManager.Dao.Impl
{
    public class TimeZoneDaoEfImpl : BaseDaoEfImpl<TimeZone>, ITimeZoneDao
    {
        public TimeZoneDaoEfImpl(IDatabaseContextContainer contextContainer) : base(contextContainer)
        {
        }

        protected override IQueryable<TimeZone> QueryTemplate()
        {
            return base.QueryTemplate()
                .Include(timezone => timezone.User);
        }

        public ICollection<TimeZone> FindByName(string name)
        {
            return QueryTemplate()
                .Where(entity => entity.Name.Contains(name))
                .ToList();
        }
    }
}
