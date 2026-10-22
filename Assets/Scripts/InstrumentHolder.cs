using UnityEngine;
using UnityEngine.InputSystem;

public class InstrumentHolder : MonoBehaviour
{

    [SerializeField] Instrument[] instruments;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SelectionManager selectionManager;
    [SerializeField] int equippedIndex = 0;
    static readonly Key[] instrumentKeybinds = {Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5};

    void Start()
    {   
        spriteRenderer.enabled = false;
        if (instruments.Length > 0) {
            Equip(equippedIndex);
        }
    }   

    void OnSelectionChanged(EditableShape shape) {
        spriteRenderer.enabled = shape != null;
    }

    void OnEnable() {
        selectionManager.SelectionChanged += OnSelectionChanged;
    }

    void OnDisable() {
        selectionManager.SelectionChanged -= OnSelectionChanged;
    }

    public event System.Action<Instrument> InstrumentChanged;

    void Equip(int index) {
        if (index >= 0 && index < instruments.Length && instruments[index] != null) {
            equippedIndex = index;
            Instrument equippedInstrument = instruments[equippedIndex];
            spriteRenderer.sprite = equippedInstrument.instrumentSprite;
            spriteRenderer.transform.localPosition = equippedInstrument.offset;
            spriteRenderer.transform.localScale = equippedInstrument.scale;
            spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, equippedInstrument.rotation);

            InstrumentChanged?.Invoke(equippedInstrument);
        }
    }

    // ponytail: IMGUI label, swap for a Canvas + TMP text when the HUD needs styling
    [SerializeField] int labelFontSize = 48;
    GUIStyle labelStyle; // built in OnGUI since GUI.skin is only valid there

    void OnGUI() {
        if (equippedIndex >= instruments.Length || instruments[equippedIndex] == null) return;
        labelStyle ??= new GUIStyle(GUI.skin.label) { fontSize = labelFontSize };
        GUI.Label(new Rect(10, 10, 1200, 120), "" + instruments[equippedIndex].ability, labelStyle);
    }

    void Update() {
        var kb = Keyboard.current;
        if (kb == null) return;

        for (int i = 0; i < instrumentKeybinds.Length; i++)
        {
            Key currentKeyCheck = instrumentKeybinds[i];
            if (kb[currentKeyCheck].wasPressedThisFrame) {
                Equip(i);
                break;
            }
        }
    }

}
