using UnityEngine;

public class RotateTool : ShapeTool
{
    public float step = 15f; // degrees per scroll tick

    public override Abilities Ability => Abilities.Rotate;

    public override bool Tick(EditableShape shape)
    {
        float scroll = Scroll();
        if (Mathf.Approximately(scroll, 0f)) return false;
        shape.Rotate(scroll * step);
        return true;
    }
}