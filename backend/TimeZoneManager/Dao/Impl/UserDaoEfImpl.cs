using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TimeZoneManager.Model;
using TimeZoneManager.Orm;
using TimeZoneManager.Orm.Dao;

namespace TimeZoneManager.Dao.Impl
{
    public class UserDaoEfImpl : BaseDaoEfImpl<User>, IUserDao
    {
        public UserDaoEfImpl(IDatabaseContextContainer contextContainer) : base(contextContainer)
        {
        }

        protected override IQueryable<User> QueryTemplate()
        {
            return base.QueryTemplate()
                .Include(u => u.RoleInUsers)
                    .ThenInclude(ru => ru.Role);
        }

        public User LoadWithPermissions(string userName)
        {
            return QueryTemplate()
                .Include(u => u.RoleInUsers)
                    .ThenInclude(ru => ru.Role.PermissionInRoles)
                        .ThenInclude(pr => pr.Permission)
                .Where(user => user.Username == userName)
                .SingleOrDefault();
        }

        public ICollection<User> FindByRole(Role role)
        {
            return (from user in QueryTemplate()
                    from roleInUser in user.RoleInUsers
                    where roleInUser.RoleId == role.Id
                    select user)
                .ToList();
        }

        public User FindByUsername(string username)
        {
            return QueryTemplate()
                .SingleOrDefault(p => p.Username == username);
        }

        public ICollection<User> FindByUsername(ICollection<string> usernameList)
        {
            return QueryTemplate()
                .Where(p => usernameList.Contains(p.Username))
                .ToList();
        }
    }
}
