using Godot;
using TheCatacombsOfVelhart.Scripts.Core;
using TheCatacombsOfVelhart.Scripts.Core.Events;

namespace TheCatacombsOfVelhart.Scripts.Systems;

public partial class EventBusTestSystem : Node
{
    public override void _Ready()
    {
        base._Ready();
        
        EventBus.Subscribe<TestEvent>(e => GD.Print(e.Data), HandlerPriority.Medium);
    }
}