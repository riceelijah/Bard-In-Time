using UnityEngine;
using UnityEngine.InputSystem;

public class ScaleTool : ShapeTool
{
    public float step = 0.1f;

    public override Abilities Ability => Abilities.Scale;

    public override bool Tick(EditableShape shape)
    {
        float scroll = Scroll();
        if (Mathf.Approximately(scroll, 0f)) return false;

        float f = 1f + scroll * step;
        var kb = Keyboard.current;
        bool scaleX = !kb.ctrlKey.isPressed;
        bool scaleY = !kb.shiftKey.isPressed;
        shape.Scale(new Vector2(scaleX ? f : 1f, scaleY ? f : 1f));
        return true;
    }
}