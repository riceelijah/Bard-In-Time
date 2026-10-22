using UnityEngine;
using UnityEngine.InputSystem;

public class LevitateTool : ShapeTool
{
    public float step = 0.25f; // units per scroll tick

    public override Abilities Ability => Abilities.Levitate;

    public override bool Tick(EditableShape shape)
    {
        if (Mouse.current.rightButton.wasPressedThisFrame) { shape.Drop(); return true; }

        float scroll = Scroll();
        if (Mathf.Approximately(scroll, 0f)) return false;
        shape.Levitate(Mathf.Sign(scroll) * step);
        return true;
    }
}