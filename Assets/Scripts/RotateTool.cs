using UnityEngine;

public class RotateTool : ShapeTool
{
    public float step = 15f; // degrees per scroll tick

    public override Abilities Ability => Abilities.Rotate;

    public override void Tick(EditableShape shape)
    {
        float scroll = Scroll();
        if (Mathf.Approximately(scroll, 0f)) return;
        shape.Rotate(Mathf.Sign(scroll) * step);
    }
}