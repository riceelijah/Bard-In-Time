using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public GameObject instrument;

    Rigidbody2D rb;
    float moveInput;
    bool isGrounded;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        moveInput = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) moveInput -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveInput += 1f;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (kb.spaceKey.wasPressedThisFrame && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}