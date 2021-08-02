using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MagicColorString
{
    public static string Red = "red";
    public static string Blue = "blue";
    public static string Brown = "brown";
    public static string Purple = "purple";
    public static string Green = "green";
}

public static class NextGenDebug
{
    public static string DebugWithColor(string message, string color)
    {
        return "<color=" + color + ">" + message + "</color>";
        
    }
    public static string HeavyError(string message)
    {
        return DebugWithColor(message, MagicColorString.Red);
    }

    public static string NormalError(string message)
    {
        return DebugWithColor(message, MagicColorString.Purple);
    }

    public static string OneShotUsage(string message)
    {
        return DebugWithColor(message, MagicColorString.Blue);
    }
}


