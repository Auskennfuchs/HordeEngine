namespace Horde.Engine.Events {
    public interface IParentEventListener {
        bool HandleEvent(IEvent ev);
    }
}
