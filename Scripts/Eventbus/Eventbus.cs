using System;
using System.Collections.Generic;
using System.Linq;

namespace TheCatacombsOfVelhart.Scripts.Eventbus;

public static class EventBus
{
    private static readonly List<IHandler> Handlers = new();

    // Raise new event (somewhere in code)
    /// <summary>
    /// Raise(new EventType(data))
    /// </summary>
    /// <param name="e"></param>
    /// <typeparam name="TEvent"></typeparam>
    public static void Raise<TEvent>(TEvent e) where TEvent : Event
    {
        // Iterate through all handlers:
        // 1. Filter only those that can handle this event type
        // 2. Sort them by priority (lower value = executed earlier)
        foreach (var handler in Handlers
                     .Where(h => h.CanHandle(typeof(TEvent)))
                     .OrderBy(h => h.Priority))
        {
            // Invoke the handler with the event instance
            handler.Handle(e);

            // If the event was canceled by any handler,
            // stop further processing
            if (e.Canceled)
                break;
        }
    }
    
    // Subscribe to this event (somewhere in code)
    /// EventBus.Subscribe[EventType](e => Function/lambda, HandlerPriority)
    public static void Subscribe<TEvent>(Action<TEvent> handler, HandlerPriority priority)
        where TEvent : Event
    {
        Handlers.Add(new Handler<TEvent>(handler, priority));
    }
}

public enum HandlerPriority
{
    Sensor = 0, // lowest priority
    Low = 1,
    Medium = 2,
    High = 3,
    VeryHigh = 4, // highest priority
}

public abstract class Event(bool cancelled = false)
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    public bool Canceled = cancelled;
}

public interface IHandler
{
    HandlerPriority Priority { get; }
    bool CanHandle(Type type);
    void Handle(Event e);
}

// stores handler function and its priority
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