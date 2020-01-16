using System.Collections.Generic;
using TimeZoneManager.Model;
using TimeZoneManager.Orm.Dao;

namespace TimeZoneManager.Dao
{
    public interface IRoleDao : IDao<Role>
    {
        ICollection<Role> FindByPermission(Permission permission);

        Role FindByName(string name);

        ICollection<Role> FindByName(ICollection<string> nameList);
    }
}
