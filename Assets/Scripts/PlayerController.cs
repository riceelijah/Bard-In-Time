using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 12f;
    public float launchMultiplier = 0.5f; // how much of a rising block's speed we inherit
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    float moveInput;
    Collider2D ground;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        moveInput = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) moveInput -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveInput += 1f;

        if (moveInput != 0f) {
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x) * moveInput;
            transform.localScale = s;
        }

        ground = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (kb.spaceKey.wasPressedThisFrame && ground)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, SurfaceSpeed() + jumpForce);
    }

    void FixedUpdate()
    {
        // ride a rising surface; when it stops, we keep that speed and fly off (inertia)
        float vy = rb.linearVelocity.y;
        float surface = SurfaceSpeed();
        if (surface > 0f) vy = Mathf.Max(vy, surface); // only a rising floor pushes; 0 must not stop falling
        // change this to a force or accel to stop the stutter
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, vy);
    }

    // Only counts when our feet are over the block, not beside it.
    float SurfaceSpeed() {
        if (!ground || !ground.TryGetComponent(out EditableShape r)) return 0f;
        float x = groundCheck.position.x;
        if (x < ground.bounds.min.x || x > ground.bounds.max.x) return 0f;
        return r.SurfaceSpeed * launchMultiplier;
    }
}