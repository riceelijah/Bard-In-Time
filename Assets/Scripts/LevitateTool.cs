using UnityEngine;
using UnityEngine.InputSystem;

public class LevitateTool : ShapeTool
{
    public float step = 0.25f; // units per scroll tick

    public override Abilities Ability => Abilities.Levitate;

    public override void Tick(EditableShape shape)
    {
        if (Mouse.current.rightButton.wasPressedThisFrame) { shape.Drop(); return; }

        float scroll = Scroll();
        if (Mathf.Approximately(scroll, 0f)) return;
        shape.Levitate(Mathf.Sign(scroll) * step);
    }
}