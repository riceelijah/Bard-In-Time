using UnityEngine;
using UnityEngine.SceneManagement;

// Walk into it to load the next scene in the build's scene list.
// Opens only while every plate in the list is pressed; an empty list means always open.
[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
{
    [SerializeField] PressurePlate[] plates;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color lockedTint = new Color(0.35f, 0.35f, 0.35f);

    bool Open => System.Array.TrueForAll(plates, p => p.Pressed);

    void FixedUpdate() => spriteRenderer.color = Open ? Color.white : lockedTint;

    // Stay, not Enter, so a player already standing in the doorway goes through the moment it opens
    void OnTriggerStay2D(Collider2D other)
    {
        if (!Open || other.GetComponentInParent<PlayerController>() == null) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
