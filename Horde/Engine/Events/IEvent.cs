namespace Horde.Engine.Events {
    public enum EventType {
        KEYDOWN,
        KEYUP,
        FRAME_START,
        NUM_EVENTS
    }


    public interface IEvent {
        string GetName();
        EventType GetEventType();
    }
}
