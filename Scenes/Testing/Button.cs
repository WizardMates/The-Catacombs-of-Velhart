using TheCatacombsOfVelhart.Scripts.Core;
using TheCatacombsOfVelhart.Scripts.Core.Events;

namespace TheCatacombsOfVelhart.Scenes.Testing;

public partial class Button : Godot.Button
{
	public override void _Pressed()
	{
		base._Pressed();
		EventBus.Raise(new TestEvent("pressed :3"));
		
	}
}
