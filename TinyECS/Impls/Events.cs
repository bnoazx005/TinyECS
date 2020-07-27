using System;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    public struct TNewEntityCreatedEvent: IEvent
    {
        public EntityId mEntityId;
    }

    public struct TEntityDestroyedEvent: IEvent
    {
        public EntityId mEntityId;

        public string   mEntityName;
    }

    public struct TNewComponentAddedEvent: IEvent
    {
        public EntityId mOwnerId;

        public Type     mComponentType;
    }


    public struct TComponentChangedEvent<T>: IEvent
    {
        public EntityId mOwnerId;

        public T        mValue;
    }


    public struct TComponentRemovedEvent: IEvent
    {
        public EntityId mOwnerId;

        public Type     mComponentType;
    }
}
