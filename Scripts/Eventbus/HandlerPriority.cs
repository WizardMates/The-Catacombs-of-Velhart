namespace TheCatacombsOfVelhart.Scripts.Eventbus;

public enum HandlerPriority
{
    Sensor = 0, // lowest priority
    Low = 1,
    Medium = 2,
    High = 3,
    VeryHigh = 4, // highest priority
}