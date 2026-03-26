using System;

namespace TheCatacombsOfVelhart.Scripts.Eventbus;

/// <summary>
/// Wraps a callback function and binds it to a specific event type.<br/>
/// Used internally by <c>EventBus</c> to store and invoke subscribed handlers.
/// </summary>
/// <typeparam name="TEvent">The event type this handler is bound to, must inherit from <c>Event</c>.</typeparam>
public class Handler<TEvent>(Action<TEvent> callback, HandlerPriority priority) : IHandler
    where TEvent : Event
{
    /// <summary>
    /// Priority of the handler.
    /// </summary>
    public HandlerPriority Priority { get; } = priority;

    /// <summary>
    /// The callback delegate associated with this handler.
    /// Invoked when a matching event is processed.
    /// </summary>
    private Action<TEvent> Callback { get; } = callback;

    /// <summary>
    /// Checks whether this handler is associated with the specified callback instance.
    /// </summary>
    /// <param name="callback">The callback instance to compare against.</param>
    /// <returns>True if the stored callback matches the provided instance; otherwise false.</returns>
    public bool IsOwnedBy(object callback) => Callback.Equals(callback);
    
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
    public void Handle(Event e) => Callback?.Invoke((TEvent)e);
}