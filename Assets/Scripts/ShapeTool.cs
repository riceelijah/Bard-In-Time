using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ShapeTool : MonoBehaviour
{
    public abstract Abilities Ability { get; }
    public abstract void Tick(EditableShape shape);

    // dominant scroll axis, so horizontal trackpad swipes count too
    protected static float Scroll()
    {
        Vector2 s = Mouse.current.scroll.ReadValue();
        return Mathf.Abs(s.y) > Mathf.Abs(s.x) ? s.y : s.x;
    }
}