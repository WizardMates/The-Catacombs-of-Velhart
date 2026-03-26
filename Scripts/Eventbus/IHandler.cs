using System;

namespace TheCatacombsOfVelhart.Scripts.Eventbus;

public interface IHandler
{
	HandlerPriority Priority { get; }
	bool CanHandle(Type type);
	void Handle(Event e);
	bool IsOwnedBy(object callback);
}