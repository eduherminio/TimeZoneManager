using System.Collections.Generic;
using TimeZoneManager.Model;
using TimeZoneManager.Orm.Dao;

namespace TimeZoneManager.Dao
{
    public interface IUserDao : IDao<User>
    {
        /// <summary>
        /// Loads a user, including all its Permissions (and Roles, therefore)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        User LoadWithPermissions(string userName);

        ICollection<User> FindByRole(Role role);

        User FindByUsername(string username);

        ICollection<User> FindByUsername(ICollection<string> usernameList);
    }
}
