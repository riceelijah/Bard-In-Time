using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] LayerMask editableLayer;
    [SerializeField] InstrumentHolder instrumentHolder;

    Camera cam;
    EditableShape selected;
    ShapeTool[] tools;
    ShapeTool tool; // the one matching the equipped instrument, or null
    AudioSource audioSource; // its clip is the equipped instrument's sound

    void Awake()
    {
        cam = Camera.main;
        tools = GetComponents<ShapeTool>();
        audioSource = GetComponent<AudioSource>();
        instrumentHolder.InstrumentChanged += OnInstrumentChanged;
    }

    void OnDestroy()
    {
        instrumentHolder.InstrumentChanged -= OnInstrumentChanged;
    }

    void OnInstrumentChanged(Instrument instrument) {
        tool = System.Array.Find(tools, t => t.Ability == instrument.ability);
        audioSource.Stop();
        audioSource.clip = instrument.instrumentSound;
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector2 p = cam.ScreenToWorldPoint(mouse.position.ReadValue());
            Collider2D hit = Physics2D.OverlapPoint(p, editableLayer);
            SetSelected(hit ? hit.GetComponentInParent<EditableShape>() : null);
        }

        // only one sound at a time, so fast scrolling doesn't stack copies
        if (selected != null && tool != null && tool.Tick(selected)
            && audioSource.clip != null && !audioSource.isPlaying)
            audioSource.Play();
    }

    public event System.Action<EditableShape> SelectionChanged;

    // public so tools that destroy/replace the shape can clear or move the selection first
    public void SetSelected(EditableShape s)
    {
        if (selected != null) selected.Deselect();
        selected = s;
        if (selected != null) selected.Select();
        SelectionChanged?.Invoke(selected);
    }
}