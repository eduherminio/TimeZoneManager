using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TimeZoneManager.Model;
using TimeZoneManager.Orm;
using TimeZoneManager.Orm.Dao;

namespace TimeZoneManager.Dao.Impl
{
    public class RoleDaoEfImpl : BaseDaoEfImpl<Role>, IRoleDao
    {
        public RoleDaoEfImpl(IDatabaseContextContainer contextContainer) : base(contextContainer)
        {
        }

        protected override IQueryable<Role> QueryTemplate()
        {
            return base.QueryTemplate()
                .Include("PermissionInRoles.Permission");
        }

        public ICollection<Role> FindByPermission(Permission permission)
        {
            IEnumerable<Role> rolesInMemory = QueryTemplate().ToList();

            return rolesInMemory
                .Where(role => role.Permissions.Contains(permission))
                .ToList();
        }

        public Role FindByName(string name)
        {
            return QueryTemplate()
                .SingleOrDefault(p => p.Name == name);
        }

        public ICollection<Role> FindByName(ICollection<string> nameList)
        {
            return QueryTemplate()
                .Where(p => nameList.Contains(p.Name))
                .ToList();
        }
    }
}
