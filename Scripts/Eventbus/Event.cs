namespace TheCatacombsOfVelhart.Scripts.Eventbus;

public abstract class Event(bool cancelled = false)
{
    public bool Canceled = cancelled;
}