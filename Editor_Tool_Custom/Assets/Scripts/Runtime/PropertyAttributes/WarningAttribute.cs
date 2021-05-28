using UnityEngine;

public class WarningAttribute : PropertyAttribute
{
    public string Message { get; private set; }

    public WarningAttribute(string message)
    {
        Message = message;
    }
}
