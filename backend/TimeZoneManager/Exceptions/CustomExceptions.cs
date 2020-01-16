using System;
using System.Runtime.Serialization;

namespace TimeZoneManager.Exceptions
{
    [Serializable]
#pragma warning disable S4027 // Exceptions should provide standard constructors - Abstract, base exception, public constructors aren't needed
    public abstract class BaseCustomException : Exception
#pragma warning restore S4027 // Exceptions should provide standard constructors - Abstract, base exception, public constructors aren't needed
    {
        protected BaseCustomException() { }
        protected BaseCustomException(string message) : base(message) { }
        protected BaseCustomException(string message, Exception inner) : base(message, inner) { }
        protected BaseCustomException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public abstract string Description { get; }
    }

    [Serializable]
    public class InvalidTokenException : BaseCustomException
    {
        public InvalidTokenException() { }
        public InvalidTokenException(string message) : base(message) { }
        public InvalidTokenException(string message, Exception inner) : base(message, inner) { }
        protected InvalidTokenException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string Description => "Invalid token";
    }

    /// <summary>
    /// Unifieds different provider exceptions (i.e. SqlException, SqliteException, etc.)
    /// </summary>
    [Serializable]
    public class DatabaseException : BaseCustomException
    {
        public DatabaseException() { }
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception inner) : base(message, inner) { }
        protected DatabaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string Description => "Database exception";
    }

    [Serializable]
    public class EntityDoesNotExistException : BaseCustomException
    {
        public EntityDoesNotExistException() { }
        public EntityDoesNotExistException(string message) : base(message) { }
        public EntityDoesNotExistException(string message, Exception inner) : base(message, inner) { }
        protected EntityDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string Description => "Non existing entity";
    }

    [Serializable]
    public class InternalErrorException : BaseCustomException
    {
        public InternalErrorException() { }
        public InternalErrorException(string message) : base(message) { }
        public InternalErrorException(string message, Exception inner) : base(message, inner) { }
        protected InternalErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string Description => "Internal error exception";
    }

    [Serializable]
    public class EntityAlreadyExistsException : BaseCustomException
    {
        public EntityAlreadyExistsException() { }
        public EntityAlreadyExistsException(string message) : base(message) { }
        public EntityAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string Description => "Entity already exists";
    }

    [Serializable]
    public class InvalidDataException : BaseCustomException
    {
        public InvalidDataException() { }
        public InvalidDataException(string message) : base(message) { }
        public InvalidDataException(string message, Exception inner) : base(message, inner) { }
        protected InvalidDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string Description => "Invalid data";
    }
}
