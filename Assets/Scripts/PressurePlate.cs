using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PressurePlate : MonoBehaviour
{
    [SerializeField] LayerMask pressedBy; // e.g. Editable, so only shapes count
    [SerializeField] EditableShape[] ignoredShapes; // drag in shape prefabs (Box, Circle...) whose type can't press this
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;

    public UnityEvent onPressed;
    public UnityEvent onReleased;

    Collider2D plate;
    bool pressed;
    public bool Pressed => pressed;
    readonly List<Collider2D> hits = new();

    void Awake() => plate = GetComponent<Collider2D>();

    // poll instead of counting enter/exit, so destroyed or reshaped shapes can't leave a stale count
    void FixedUpdate()
    {
        bool now = IsPressed();
        if (now == pressed) return;

        pressed = now;
        spriteRenderer.sprite = pressed ? onSprite : offSprite;
        (pressed ? onPressed : onReleased).Invoke();
    }

    bool IsPressed()
    {
        var filter = new ContactFilter2D();
        filter.SetLayerMask(pressedBy);
        plate.Overlap(filter, hits);

        foreach (Collider2D hit in hits)
        {
            // match by type, like ChangeShapeTool, so it still works after a shape is swapped
            var shape = hit.GetComponentInParent<EditableShape>();
            if (shape == null || !System.Array.Exists(ignoredShapes, s => s.GetType() == shape.GetType()))
                return true;
        }
        return false;
    }
}
