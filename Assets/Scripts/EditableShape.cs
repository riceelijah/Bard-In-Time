using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public abstract class EditableShape : MonoBehaviour
{
    [SerializeField] Color selectedTint = Color.yellow;
    [SerializeField] Vector2 minSize = new Vector2(1.5f, 1.5f);
    [SerializeField] Vector2 maxSize = new Vector2(25f, 25f);
    [SerializeField] float growDuration = 0.15f; // seconds per scroll tick
    [SerializeField] float levitateStiffness = 8f; // how hard it chases its target height (1/s)

    protected Rigidbody2D rb;
    SpriteRenderer sr;
    Color baseColor;

    Vector3 start, target;
    float elapsed;

    Collider2D col;
    float lastTop;
    public float SurfaceSpeed { get; private set; } // how fast the top edge is rising (units/s)

    float gravity;
    bool levitating;
    float levitateY;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        rb.useAutoMass = true; // mass follows size
        gravity = rb.gravityScale;

        start = target = transform.localScale;
        elapsed = growDuration; // start "finished" so nothing animates on spawn
        col = GetComponent<Collider2D>();
        lastTop = col.bounds.max.y;
    }

    protected virtual void FixedUpdate()
    {
        // bounds reflect the last physics step, so measure before changing scale
        float top = col.bounds.max.y;
        SurfaceSpeed = (top - lastTop) / Time.fixedDeltaTime;
        lastTop = top;

        elapsed += Time.fixedDeltaTime;
        transform.localScale = Vector3.Lerp(start, target, elapsed / growDuration);

        if (levitating)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, (levitateY - rb.position.y) * levitateStiffness);
    }

    public virtual void Select() => sr.color = selectedTint;
    public virtual void Deselect() => sr.color = baseColor;

    public virtual void Scale(Vector2 factor)
    {
        start = transform.localScale;
        elapsed = 0f;
        target.x = Mathf.Clamp(target.x * factor.x, minSize.x, maxSize.x);
        target.y = Mathf.Clamp(target.y * factor.y, minSize.y, maxSize.y);
    }

    // instant resize with no animation, for freshly spawned shapes
    public virtual void SetSize(Vector3 size) => start = target = transform.localScale = size;

    public virtual void Move(Vector2 delta) => rb.position += delta;
    public virtual void Rotate(float degrees) => rb.rotation += degrees;

    public void Levitate(float dy)
    {
        if (!levitating) levitateY = rb.position.y; // start from where it is now
        levitating = true;
        rb.gravityScale = 0f;
        levitateY += dy;
    }

    public void Drop()
    {
        levitating = false;
        rb.gravityScale = gravity;
    }
}