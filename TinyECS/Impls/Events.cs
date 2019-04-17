using System;
using TinyECS.Interfaces;


namespace TinyECS.Impls
{
    public struct TNewEntityCreatedEvent: IEvent
    {
        public uint mEntityId;
    }


    public struct TNewComponentAddedEvent: IEvent
    {
        public uint mOwnerId;

        public Type mComponentType;
    }


    public struct TComponentRemovedEvent: IEvent
    {
        public uint mOwnerId;

        public Type mComponentType;
    }
}
