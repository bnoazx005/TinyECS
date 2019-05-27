using System;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    public struct TNewEntityCreatedEvent: IEvent
    {
        public uint mEntityId;
    }

    public struct TEntityDestroyedEvent: IEvent
    {
        public uint   mEntityId;

        public string mEntityName;
    }

    public struct TNewComponentAddedEvent: IEvent
    {
        public uint mOwnerId;

        public Type mComponentType;
    }


    public struct TComponentChangedEvent<T>: IEvent
    {
        public uint mOwnerId;

        public T    mValue;
    }


    public struct TComponentRemovedEvent: IEvent
    {
        public uint mOwnerId;

        public Type mComponentType;
    }
}
