namespace TheCatacombsOfVelhart.Scripts.Core;

public abstract class Event(bool cancelled = false)
{
    public bool Canceled = cancelled;
}