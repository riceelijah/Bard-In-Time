using UnityEngine;

public class InstrumentHolder : MonoBehaviour
{

    [SerializeField] Instrument[] instruments;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SelectionManager selectionManager;
    [SerializeField] int equippedIndex;

    void Start()
    {   
        spriteRenderer.enabled = false;
        if (instruments.Length > 0) {
            spriteRenderer.sprite = instruments[0].instrumentSprite;
            equippedIndex = 0;
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


}
