using Godot;
using TheCatacombsOfVelhart.Scripts.Eventbus;
using TheCatacombsOfVelhart.Scripts.Eventbus.Events;

namespace TheCatacombsOfVelhart.Scripts.Systems;

public partial class EventBusTestSystem : Node
{
    public override void _Ready()
    {
        base._Ready();
        
        EventBus.Subscribe<TestEvent>(e => GD.Print(e.Data), HandlerPriority.Medium);
    }
}