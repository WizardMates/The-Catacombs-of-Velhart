namespace TheCatacombsOfVelhart.Scripts.Eventbus;

public abstract class Event(bool cancelled = false)
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    public bool Canceled = cancelled;
}