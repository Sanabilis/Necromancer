using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public static class MouseManager
{
    private static Vector2 _defaultMousePosition = Vector2.zero;

    public static void ShowCursor()
    {
        Cursor.visible = true;
    }

    public static void HideCursor()
    {
        InputState.Change(Mouse.current.position, _defaultMousePosition);
        Cursor.visible = false;
    }
}