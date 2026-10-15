using UnityEngine;

public class RectShape : EditableShape
{
    [SerializeField] Vector2 minSize = new Vector2(0.5f, 0.5f);
    [SerializeField] Vector2 maxSize = new Vector2(10f, 10f);
    [SerializeField] float growDuration = 0.15f; // seconds per scroll tick

    Vector3 start, target;
    float elapsed;

    Collider2D col;
    float lastTop;
    public float SurfaceSpeed { get; private set; } // how fast the top edge is rising (units/s)

    protected override void Awake() {
        base.Awake();
        start = target = transform.localScale;
        elapsed = growDuration; // start "finished" so nothing animates on spawn
        col = GetComponent<Collider2D>();
        lastTop = col.bounds.max.y;
    }

    public override void Scale(Vector2 factor)
    {
        start = transform.localScale;
        elapsed = 0f;
        target.x = Mathf.Clamp(target.x * factor.x, minSize.x, maxSize.x);
        target.y = Mathf.Clamp(target.y * factor.y, minSize.y, maxSize.y);
    }

    void FixedUpdate() {
        // bounds reflect the last physics step, so measure before changing scale
        float top = col.bounds.max.y;
        SurfaceSpeed = (top - lastTop) / Time.fixedDeltaTime;
        lastTop = top;

        elapsed += Time.fixedDeltaTime;
        transform.localScale = Vector3.Lerp(start, target, elapsed / growDuration);
    }
}
