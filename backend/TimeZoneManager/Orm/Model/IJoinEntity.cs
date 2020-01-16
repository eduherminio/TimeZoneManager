namespace TimeZoneManager.Orm.Model
{
    /// <summary>
    /// Based on: https://blog.oneunicorn.com/2017/09/25/many-to-many-relationships-in-ef-core-2-0-part-4-a-more-general-abstraction/
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IJoinEntity<TEntity>
    {
        TEntity Navigation { get; set; }
    }
}
