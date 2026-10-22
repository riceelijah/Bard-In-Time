using TMPro;
using UnityEngine;

// Put on a TextMeshPro - Text (UI) object inside a Canvas. One per scene.
// Change the message from a HintTrigger, or from any UnityEvent (e.g. a PressurePlate's onPressed) by picking
// TutorialText > Show (string) in the event list and typing the new message right there in the Inspector.
[RequireComponent(typeof(TMP_Text))]
public class TutorialText : MonoBehaviour
{
    [SerializeField, TextArea] string startMessage;

    public static TutorialText Current { get; private set; }

    TMP_Text label;

    void Awake()
    {
        Current = this;
        label = GetComponent<TMP_Text>();
        Show(startMessage);
    }

    public void Show(string message) => label.text = message;

    public void Hide() => label.text = "";
}
