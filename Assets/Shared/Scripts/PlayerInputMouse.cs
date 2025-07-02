using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMouse : MonoBehaviour
{
    public float GetScroll()
    {
        return Input.GetAxis("Mouse ScrollWheel");
    }
    public Vector2 GetMousePosition()
    {
        return Input.mousePosition;
    }
    public bool IsLeftClickDown()
    {
        return Input.GetMouseButtonDown(0);
    }
    public bool IsRightClickDown()
    {
        return Input.GetMouseButtonDown(1);
    }
    public bool IsLeftClickHeld()
    {
        return Input.GetMouseButton(0);
    }
    public bool IsRightClickHeld()
    {
        return Input.GetMouseButton(1);
    }
    public bool IsLeftClickUp()
    {
        return Input.GetMouseButtonUp(0);
    }
    public bool IsRightClickUp()
    {
        return Input.GetMouseButtonUp(1);
    }
}
