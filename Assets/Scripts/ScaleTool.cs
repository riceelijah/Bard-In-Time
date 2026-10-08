using UnityEngine;
using UnityEngine.InputSystem;

public class ScaleTool : ShapeTool
{
    public float step = 0.1f;

    public override void Tick(EditableShape shape)
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Approximately(scroll, 0f)) return;

        float f = 1f + Mathf.Sign(scroll) * step;
        var kb = Keyboard.current;
        bool scaleX = !kb.ctrlKey.isPressed;
        bool scaleY = !kb.shiftKey.isPressed;
        shape.Scale(new Vector2(scaleX ? f : 1f, scaleY ? f : 1f));
    }
}