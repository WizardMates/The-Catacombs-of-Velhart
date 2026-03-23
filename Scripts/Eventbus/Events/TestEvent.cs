namespace TheCatacombsOfVelhart.Scripts.Eventbus.Events;

public class TestEvent(string data) : Event
{
    public string Data { get; set; } = data;
}