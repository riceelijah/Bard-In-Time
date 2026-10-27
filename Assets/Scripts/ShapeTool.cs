using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ShapeTool : MonoBehaviour
{
    public abstract Abilities Ability { get; }
    // returns true on frames the tool actually changed the shape, so the instrument's sound can play
    public abstract bool Tick(EditableShape shape);

    [SerializeField] float scrollCooldown = 0.05f; // min seconds between steps; trackpads send scroll every frame

    float nextScroll;

    // dominant scroll axis, so horizontal trackpad swipes count too; 0 while cooling down.
    // Returns ±ScrollSensitivity, so tools multiply it by their step.
    protected float Scroll()
    {
        Vector2 s = Mouse.current.scroll.ReadValue();
        float v = Mathf.Abs(s.y) > Mathf.Abs(s.x) ? s.y : s.x;
        if (v == 0f || Time.time < nextScroll) return 0f;

        nextScroll = Time.time + scrollCooldown;
        return Mathf.Sign(v) * GameSettings.ScrollSensitivity;
    }
}