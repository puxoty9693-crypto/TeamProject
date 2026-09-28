using UnityEngine;
using System.Diagnostics;

public static class GameLogOnlyEditor
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }
}
