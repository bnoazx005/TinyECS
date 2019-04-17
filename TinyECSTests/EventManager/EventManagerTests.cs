using NUnit.Framework;
using TinyECS.Interfaces;
using TinyECS.Impls;
using System;
using NSubstitute;
using System.Collections.Generic;

namespace TinyECSTests
{
    [TestFixture]
    public class EventManagerTests
    {
        public struct TSimpleEvent: IEvent
        {
        }

        public struct TAnotherEvent: IEvent
        {
        }

        protected IEventManager mEventManager;

        [SetUp]
        public void Init()
        {
            mEventManager = new EventManager();
        }

        [Test]
        public void TestSubscribe_PassNullArgument_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                mEventManager.Subscribe<TSimpleEvent>(null);
            });
        }

        [Test]
        public void TestSubscribe_PassCorrectReference_ReturnsListenerIdentifier()
        {
            Assert.DoesNotThrow(() =>
            {
                IEventListener eventListener = Substitute.For<IEventListener>();

                mEventManager.Subscribe<TSimpleEvent>(eventListener);
            });
        }

        [Test]
        public void TestSubscribe_MultipleSubscriptionsCreatesNewListener_ReturnsListenerIdentifier()
        {
            Assert.DoesNotThrow(() =>
            {
                HashSet<uint> createdListenersIds = new HashSet<uint>();

                IEventListener eventListener = Substitute.For<IEventListener>();

                uint registeredListenerId = 0;

                for (int i = 0; i < 4; ++i)
                {
                    registeredListenerId = mEventManager.Subscribe<TSimpleEvent>(eventListener);

                    // all registered listeners have unique identifiers
                    Assert.IsFalse(createdListenersIds.Contains(registeredListenerId));

                    createdListenersIds.Add(registeredListenerId);
                }
            });
        }

        [Test]
        public void TestUnsubscribe_PassInexistingId_ThrowsListenerDoesntExistException()
        {
            Assert.Throws<ListenerDoesntExistException>(() =>
            {
                mEventManager.Unsubscribe(0);
            });
        }

        [Test]
        public void TestUnsubscribe_PassCorrectExistingId_RemovesListenerFromManager()
        {
            Assert.DoesNotThrow(() =>
            {
                // subscribe to some event
                IEventListener eventListener = Substitute.For<IEventListener>();

                uint registeredListenerId = mEventManager.Subscribe<TSimpleEvent>(eventListener);

                // try to unsubscribe the listener
                mEventManager.Unsubscribe(registeredListenerId);
            });
        }

        [Test]
        public void TestNotify_InvokeOnEmptyArrayOfListeners_DoNothing()
        {
            Assert.DoesNotThrow(() =>
            {
                //mEventManager has no subscribed listeners
                mEventManager.Notify<TSimpleEvent>(new TSimpleEvent());
            });
        }

        [Test]
        public void TestNotify_InvokeForEventThatHasNoListeners_DoNothing()
        {
            Assert.DoesNotThrow(() =>
            {
                int counter = 0;

                // subscribe to TSimpleEvent event
                IEventListener<TSimpleEvent> eventListener = Substitute.For<IEventListener<TSimpleEvent>>();

                eventListener.When(e => e.OnEvent(new TSimpleEvent())).Do(e => ++counter);

                uint registeredListenerId = mEventManager.Subscribe<TSimpleEvent>(eventListener);

                // but try to notify listeners of TAnotherEvent
                mEventManager.Notify<TAnotherEvent>(new TAnotherEvent());

                Assert.AreEqual(0, counter);
            });
        }

        [Test]
        public void TestNotify_InvokeForEventThatHasFewListeners_DoNothing()
        {
            Assert.DoesNotThrow(() =>
            {
                int counter            = 0;
                int expectedCallsCount = 5;

                // subscribe to TSimpleEvent event
                IEventListener<TSimpleEvent> eventListener = Substitute.For<IEventListener<TSimpleEvent>>();

                eventListener.When(e => e.OnEvent(new TSimpleEvent())).Do(e => ++counter);

                uint registeredListenerId = mEventManager.Subscribe<TSimpleEvent>(eventListener);
                
                for (int i = 0; i < expectedCallsCount; ++i)
                mEventManager.Notify<TSimpleEvent>(new TSimpleEvent());

                Assert.AreEqual(expectedCallsCount, counter);
            });
        }
    }
}           