using System;
using System.Collections.Generic;
using System.Linq;

namespace TheCatacombsOfVelhart.Scripts.Eventbus;

public static class EventBus
{
    private static readonly List<IHandler> Handlers = new();

    /// <summary>
    /// Publishes an event to all registered handlers in priority order.
    /// If any handler cancels the event, further processing is stopped.
    /// </summary>
    /// <param name="e">The event instance to be processed</param>
    /// <typeparam name="TEvent">The event type, must inherit from Event class</typeparam>
    public static void Raise<TEvent>(TEvent e) where TEvent : Event
    {
        // Iterate through all handlers:
        // 1. Filter only those that can handle this event type
        // 2. Sort them by priority (lower value = executed earlier)
        foreach (var handler in Handlers
                     .Where(h => h.CanHandle(typeof(TEvent)))
                     .OrderByDescending(h => h.Priority))
        {
            // Invoke the handler with the event instance
            handler.Handle(e);

            // If the event was canceled by any handler,
            // stop further processing
            if (e.Canceled)
                break;
        }
    }
    
    // Registers a handler for a specific event type in the EventBus.
    // The handler will be invoked when the event is Raise.
    // Priority determines the order in which handlers are executed.
    public static void Subscribe<TEvent>(Action<TEvent> handler, HandlerPriority priority)
        where TEvent : Event
    {
        Handlers.Add(new Handler<TEvent>(handler, priority));
    }
}
