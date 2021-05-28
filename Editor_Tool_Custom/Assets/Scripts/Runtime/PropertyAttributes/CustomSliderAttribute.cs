using UnityEngine;

public class CustomSliderAttribute : PropertyAttribute
{
    public int Min { get; private set; } = 0;
    public int Max { get; private set; } = 4;

    public CustomSliderAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}
