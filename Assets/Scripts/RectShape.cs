using UnityEngine;

public class RectShape : EditableShape
{
    [SerializeField] Vector2 minSize = new Vector2(0.5f, 0.5f);
    [SerializeField] Vector2 maxSize = new Vector2(10f, 10f);

    public override void Scale(Vector2 factor)
    {
        Vector3 s = transform.localScale;
        s.x = Mathf.Clamp(s.x * factor.x, minSize.x, maxSize.x);
        s.y = Mathf.Clamp(s.y * factor.y, minSize.y, maxSize.y);
        transform.localScale = s;
    }
}