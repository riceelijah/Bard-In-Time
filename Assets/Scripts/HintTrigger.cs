using UnityEngine;

// An invisible zone: when the player walks in, the tutorial text switches to this message.
// Needs a Collider2D with "Is Trigger" ticked; size the collider to the area the hint should cover.
[RequireComponent(typeof(Collider2D))]
public class HintTrigger : MonoBehaviour
{
    [SerializeField, TextArea] string message;
    [SerializeField] bool hideOnExit; // clear the text again when the player walks back out

    void OnTriggerEnter2D(Collider2D other)
    {
        if (TutorialText.Current == null || other.GetComponentInParent<PlayerController>() == null) return;
        TutorialText.Current.Show(message);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!hideOnExit || TutorialText.Current == null || other.GetComponentInParent<PlayerController>() == null) return;
        TutorialText.Current.Hide();
    }
}
