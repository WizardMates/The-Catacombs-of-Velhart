using System;

namespace TheCatacombsOfVelhart.Scripts.Eventbus;

/// <summary>
/// Wraps a callback function and binds it to a specific event type.<br/>
/// Used internally by <c>EventBus</c> to store and invoke subscribed handlers.
/// </summary>
/// <typeparam name="TEvent">The event type this handler is bound to, must inherit from <c>Event</c>.</typeparam>
public class Handler<TEvent>(Action<TEvent> handler, HandlerPriority priority) : IHandler
    where TEvent : Event
{
    // The actual callback that will be executed when the event is raised

    /// <summary>
    /// Priority of the handler.
    /// </summary>
    public HandlerPriority Priority { get; } = priority;

    /// <summary>
    /// Checks if this handler can process the given event type.
    /// </summary>
    /// <param name="type">The event type to check.</param>
    /// <returns>True if <paramref name="type"/> matches <c>TEvent</c> exactly.</returns>
    public bool CanHandle(Type type) => typeof(TEvent) == type;

    /// <summary>
    /// Invokes the handler callback with the given event instance.
    /// Casts <c>Event</c> to <c>TEvent</c> before invoking.
    /// Does nothing if the callback is null.
    /// </summary>
    /// <param name="e">The event instance to pass to the callback.</param>
    public void Handle(Event e) => handler?.Invoke((TEvent)e);
}