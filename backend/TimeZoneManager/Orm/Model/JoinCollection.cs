using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TimeZoneManager.Orm.Model
{
    /// <summary>
    /// Based on: https://blog.oneunicorn.com/2017/09/25/many-to-many-relationships-in-ef-core-2-0-part-4-a-more-general-abstraction/
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TOtherEntity"></typeparam>
    /// <typeparam name="TJoinEntity"></typeparam>
    public class JoinCollectionFacade<TEntity, TOtherEntity, TJoinEntity>
        : ICollection<TEntity>
    where TJoinEntity : IJoinEntity<TEntity>, IJoinEntity<TOtherEntity>, new()
    {
        private readonly TOtherEntity _ownerEntity;
        private readonly ICollection<TJoinEntity> _collection;

        public JoinCollectionFacade(
            TOtherEntity ownerEntity,
            ICollection<TJoinEntity> collection)
        {
            _ownerEntity = ownerEntity;
            _collection = collection;
        }

        public IEnumerator<TEntity> GetEnumerator()
            => _collection.Select(e => ((IJoinEntity<TEntity>)e).Navigation).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(TEntity item)
        {
            Add(item, _ => { });
        }

        public void Add(TEntity item, Action<TJoinEntity> initializer)
        {
            var entity = new TJoinEntity();
            ((IJoinEntity<TEntity>)entity).Navigation = item;
            ((IJoinEntity<TOtherEntity>)entity).Navigation = _ownerEntity;
            initializer(entity);
            _collection.Add(entity);
        }

        public void Clear() => _collection.Clear();

        public bool Contains(TEntity item) => _collection.Any(e => Equals(item, e));

        public void CopyTo(TEntity[] array, int arrayIndex) => this.ToList().CopyTo(array, arrayIndex);

        public bool Remove(TEntity item) => _collection.Remove(
            _collection.FirstOrDefault(e => Equals(item, e)));

        public int Count => _collection.Count;

        public bool IsReadOnly => _collection.IsReadOnly;

        private static bool Equals(TEntity item, TJoinEntity e)
            => Equals(((IJoinEntity<TEntity>)e).Navigation, item);
    }
}
