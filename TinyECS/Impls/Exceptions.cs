using System;


namespace TinyECS.Impls
{
    /// <summary>
    /// class ComponentDoesntExistException
    /// 
    /// The class is an exception type which occurs when someone
    /// tries to get a component that an entity doesn't have
    /// </summary>

    public class ComponentDoesntExistException: Exception
    {
        public ComponentDoesntExistException(Type type, uint entityId):
            base($"A component of [{type}] doesn't belong to entity with id [{entityId}]")
        {
        }
    }


    /// <summary>
    /// class ComponentAlreadyExistException
    /// 
    /// The class is an exception type which occurs when someone
    /// tries to add unique component to another entity
    /// </summary>

    public class ComponentAlreadyExistException : Exception
    {
        public ComponentAlreadyExistException(Type type, uint entityId) :
            base($"A component of [{type}] already exists on entity [{entityId}]")
        {
        }
    }


    /// <summary>
    /// class EntityDoesntExistException
    /// 
    /// The class is an exception's type which occurs when someone 
    /// ask for an entity which doesn't exist
    /// </summary>

    public class EntityDoesntExistException: Exception
    {
        public EntityDoesntExistException(uint entityId):
            base($"An entity with the specified identifier [{entityId}] doesn't exist")
        {
        }
    }


    /// <summary>
    /// class InvalidIdentifierException
    /// 
    /// The class is an exception's type which occurs when someone pass an invalid identifier
    /// as an argument
    /// </summary>

    public class InvalidIdentifierException: Exception
    {
        public InvalidIdentifierException(uint id):
            base($"The given identifier [{id}] is not valid")
        {
        }
    }


    /// <summary>
    /// class ListenerDoesntExistException
    /// 
    /// The class is an exception's type which occurs when someone pass an invalid identifier as an argument
    /// </summary>

    public class ListenerDoesntExistException: Exception
    {
        public ListenerDoesntExistException(uint id) :
            base($"The given identifier [{id}] is not valid")
        {
        }
    }


    /// <summary>
    /// class InvalidIteratorException
    /// 
    /// The class is an exception's type which occurs when someone tries to access to a component that doesn't exist
    /// </summary>

    public class InvalidIteratorException: Exception
    {
        public InvalidIteratorException() :
            base("Iterator to a particular component is invalid")
        {
        }
    }
}
