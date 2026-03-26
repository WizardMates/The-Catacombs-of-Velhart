using System;

namespace TheCatacombsOfVelhart.Scripts.Core;

public interface IHandler
{
	HandlerPriority Priority { get; }
	bool CanHandle(Type type);
	void Handle(Event e);
	bool IsOwnedBy(object callback);
}