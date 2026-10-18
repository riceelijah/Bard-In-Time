using UnityEngine;

public class CircleShape : EditableShape
{
    // Ctrl/Shift lock an axis in ScaleTool; a circle must grow evenly, so use whichever axis changed
    public override void Scale(Vector2 factor)
    {
        float f = factor.x != 1f ? factor.x : factor.y;
        base.Scale(new Vector2(f, f));
    }

    // a squashed box turning into a circle takes its larger side
    public override void SetSize(Vector3 size)
    {
        float d = Mathf.Max(size.x, size.y);
        base.SetSize(new Vector3(d, d, size.z));
    }
}