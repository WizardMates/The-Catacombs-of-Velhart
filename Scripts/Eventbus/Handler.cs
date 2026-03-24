using System;

namespace TheCatacombsOfVelhart.Scripts.Eventbus;

// stores handler (delegate) function and its priority
public class Handler<TEvent>(Action<TEvent> handler, HandlerPriority priority) : IHandler
    where TEvent : Event
{
    // The actual callback that will be executed when the event is raised

    // Priority of this handler
    public HandlerPriority Priority { get; } = priority;
    

    // Checks if this handler can process the given event type
    // Returns true if TEvent is the same type or a base type of 'type'
    public bool CanHandle(Type type) => typeof(TEvent).IsAssignableFrom(type);

    // Executes the handler
    // Casts base Event to the specific TEvent type before invoking
    public void Handle(Event e) => handler((TEvent)e);
}