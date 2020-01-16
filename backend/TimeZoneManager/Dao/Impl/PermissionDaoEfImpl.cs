using System.Collections.Generic;
using System.Linq;
using TimeZoneManager.Authorization;
using TimeZoneManager.Model;
using TimeZoneManager.Orm;
using TimeZoneManager.Orm.Dao;

namespace TimeZoneManager.Dao.Impl
{
    public class PermissionDaoEfImpl : BaseDaoEfImpl<Permission>, IPermissionDao, IInternalPermissionDao
    {
        public PermissionDaoEfImpl(IDatabaseContextContainer contextContainer) : base(contextContainer)
        {
        }

        protected override IQueryable<Permission> QueryTemplate()
        {
            return base.QueryTemplate()
                .Where(p => p.Name != AdminPermissions.SuperAdminPermissionName);
        }

        public Permission FindByName(string name)
        {
            return QueryTemplate()
                .SingleOrDefault(p => p.Name == name);
        }

        public ICollection<Permission> FindByName(ICollection<string> nameList)
        {
            return QueryTemplate()
                .Where(p => nameList.Contains(p.Name))
                .ToList();
        }

        public Permission InternalFind(string name)
        {
            return Context.Set<Permission>()
                .SingleOrDefault(p => p.Name == name);
        }
    }
}
