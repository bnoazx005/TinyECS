using TinyECS.Interfaces;
using static TinyECSTests.EventManagerTests;


namespace TinyECSTests
{
    public class BrokenEventListenerMock : IEventListener<TSimpleEvent>
    {
        public bool IsSimpleOnEventInvoked { get; set; } = false;

        public BrokenEventListenerMock(IEventManager eventManager)
        {
            eventManager.Subscribe<TSimpleEvent>(this);
            eventManager.Subscribe<TAnotherEvent>(this);
        }

        public void OnEvent(TSimpleEvent eventData)
        {
            IsSimpleOnEventInvoked = true;
        }
    }
}