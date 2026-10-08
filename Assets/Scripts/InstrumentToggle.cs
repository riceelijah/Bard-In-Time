using UnityEngine;

public class InstrumentToggle : MonoBehaviour
{
    [SerializeField] SelectionManager selectionManager;
    [SerializeField] GameObject instrument;

    void OnEnable()
    {
        selectionManager.SelectionChanged += OnSelectionChanged;
        instrument.SetActive(false);
    }

    void OnDisable() => selectionManager.SelectionChanged -= OnSelectionChanged;

    void OnSelectionChanged(EditableShape shape) => instrument.SetActive(shape != null);
}