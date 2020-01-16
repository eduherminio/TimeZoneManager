using System.Collections.Generic;
using TimeZoneManager.Model;
using TimeZoneManager.Orm.Dao;

namespace TimeZoneManager.Dao
{
    public interface IPermissionDao : IDao<Permission>
    {
        Permission FindByName(string name);

        ICollection<Permission> FindByName(ICollection<string> nameList);
    }

    internal interface IInternalPermissionDao
    {
        Permission InternalFind(string name);
    }
}
