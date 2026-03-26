namespace TheCatacombsOfVelhart.Scripts.Core.Events;

public class TestEvent(string data) : Event
{
    public string Data { get; set; } = data;
}