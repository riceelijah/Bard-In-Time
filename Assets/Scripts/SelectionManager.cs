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
    Instrument instrument;

    void Awake()
    {
        cam = Camera.main;
        tools = GetComponents<ShapeTool>();
        instrumentHolder.InstrumentChanged += OnInstrumentChanged;
    }

    void OnDestroy()
    {
        instrumentHolder.InstrumentChanged -= OnInstrumentChanged;
    }

    void OnInstrumentChanged(Instrument instrument) {
        this.instrument = instrument;
        tool = System.Array.Find(tools, t => t.Ability == instrument.ability);
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

        if (selected != null && tool != null && tool.Tick(selected))
        {
            if (instrument.sustain) AudioManager.Sustain(instrument.useSound);
            else AudioManager.PlaySfx(instrument.useSound);
        }
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