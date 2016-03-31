namespace Horde.Engine.Events {
    public enum EventType {
        KEYDOWN,
        KEYUP,
        FRAME_START,
        MOUSE_DOWN,
        MOUSE_UP,
        MOUSE_MOVE,
        NUM_EVENTS
    }


    public interface IEvent {
        string GetName();
        EventType GetEventType();
    }
}
