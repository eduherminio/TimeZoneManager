using System.Collections.Generic;
using TimeZoneManager.Model;
using TimeZoneManager.Orm.Dao;

namespace TimeZoneManager.Dao
{
    public interface ITimeZoneDao : IDao<TimeZone>
    {
        ICollection<TimeZone> FindByName(string name);
    }
}
