﻿using System;
using System.Collections.Generic;
using TimeZoneManager.Orm.Model;

namespace TimeZoneManager.Orm
{
    /// <summary>
    /// Generic persistable entity
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public abstract class Entity : IEquatable<Entity>, IAutoGeneratedKey
    {
        // By default [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string AutoGeneratedKey { get; set; }

        /// <summary>
        /// Business key, unique identifier of each entity. Exported outside
        /// </summary>
        /// <returns></returns>
        public virtual string GetKey() => AutoGeneratedKey;

        protected const string EnumSeparator = ";";

        #region IEquatable implementation

        public bool Equals(Entity other)
        {
            if (other == null)
            {
                return false;
            }

            return GetKey().Equals(other.GetKey());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity);
        }

        public override int GetHashCode()
        {
            return -409944030 + EqualityComparer<string>.Default.GetHashCode(GetKey());
        }

        #endregion
    }
}