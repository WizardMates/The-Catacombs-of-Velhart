using System;
using System.Collections.Generic;
using System.Linq;

namespace TheCatacombsOfVelhart.Scripts.Eventbus;

/// <summary>
/// Global event bus for decoupled communication between game systems.<br/>
/// Allows any system to publish or subscribe to events without direct references.<br/>
/// <code>
/// // Subscribe:
/// EventBus.Subscribe&lt;DamageTakenEvent&gt;(OnDamageTaken, HandlerPriority.Medium);
/// // Publish:
/// EventBus.Raise(new DamageTakenEvent(target, 50f));
/// </code>
/// </summary>
public static class EventBus
{
    private static readonly Dictionary<Type, List<IHandler>> Handlers = new();

    /// <summary>
    /// Publishes the event to all subscribed handlers in descending priority order.
    /// Does nothing if no handlers are subscribed to this event type.
    /// </summary>
    /// <param name="e">The event instance to be published.</param>
    /// <typeparam name="TEvent">The event type, must inherit from <c>Event</c>.</typeparam>
    public static void Raise<TEvent>(TEvent e) where TEvent : Event {
        foreach (var handler in Handlers[typeof(TEvent)]) {
            handler.Handle(e);
        }
    }
    
    /// <summary>
    /// Subscribes a handler to the specified event type.<br/>
    /// The handler will be invoked when the event is raised via <c>Raise</c>.<br/>
    /// Handlers are sorted by priority after each subscription — higher priority is executed first.
    /// </summary>
    /// <param name="handler">Callback invoked when <c>TEvent</c> is raised.</param>
    /// <param name="priority">Determines execution order relative to other handlers.</param>
    /// <typeparam name="TEvent">The event type to subscribe to, must inherit from <c>Event</c>.</typeparam>
    public static void Subscribe<TEvent>(Action<TEvent> handler, HandlerPriority priority)
        where TEvent : Event 
    {
        Handlers.TryAdd(typeof(TEvent), new List<IHandler>()); // if Handlers dictionary doesn't have targeted key - TryAdd creates it
        Handlers[typeof(TEvent)].Add(new Handler<TEvent>(handler, priority)); // "subscribes" the handler to the event type
        Handlers[typeof(TEvent)] = Handlers[typeof(TEvent)].OrderByDescending(h => h.Priority).ToList(); // after the subscribing sorting list of handlers according to their priority
    }

    /// <summary>
    /// Unsubscribes a previously registered handler from the specified event type.
    /// Does nothing if the handler is not found.
    /// </summary>
    /// <param name="handler">The callback to unsubscribe.</param>
    /// <typeparam name="TEvent">The event type to unsubscribe from.</typeparam>
    public static void Unsubscribe<TEvent>(Action<TEvent> handler)
        where TEvent : Event 
    {
        if (!Handlers.TryGetValue(typeof(TEvent), out var handlers)) return;

        handlers.RemoveAll(h => h.IsOwnedBy(handler));
    }
}
