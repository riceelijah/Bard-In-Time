using UnityEngine;
using UnityEngine.InputSystem;

public class ScaleTool : ShapeTool
{
    public float step = 0.1f;

    public override void Tick(EditableShape shape)
    {
        Vector2 s = Mouse.current.scroll.ReadValue();
        float scroll = Mathf.Abs(s.y) > Mathf.Abs(s.x) ? s.y : s.x;
        if (Mathf.Approximately(scroll, 0f)) return;

        float f = 1f + Mathf.Sign(scroll) * step;
        var kb = Keyboard.current;
        bool scaleX = !kb.ctrlKey.isPressed;
        bool scaleY = !kb.shiftKey.isPressed;
        shape.Scale(new Vector2(scaleX ? f : 1f, scaleY ? f : 1f));
    }
}