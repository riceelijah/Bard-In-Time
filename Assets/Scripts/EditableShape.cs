using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public abstract class EditableShape : MonoBehaviour
{
    [SerializeField] Color selectedTint = Color.yellow;

    protected Rigidbody2D rb;
    SpriteRenderer sr;
    Color baseColor;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        rb.useAutoMass = true; // mass follows size
    }

    public virtual void Select() => sr.color = selectedTint;
    public virtual void Deselect() => sr.color = baseColor;

    // Shapes override only what they support.
    public virtual void Scale(Vector2 factor) { }
    public virtual void Move(Vector2 delta) => rb.position += delta;
    public virtual void Rotate(float degrees) => rb.rotation += degrees;
}