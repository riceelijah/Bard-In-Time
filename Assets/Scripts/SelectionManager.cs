using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] LayerMask editableLayer;

    Camera cam;
    EditableShape selected;
    ShapeTool[] tools;
    int toolIndex;

    void Awake()
    {
        cam = Camera.main;
        tools = GetComponents<ShapeTool>();
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector2 p = cam.ScreenToWorldPoint(mouse.position.ReadValue());
            Collider2D hit = Physics2D.OverlapPoint(p, editableLayer);
            SetSelected(hit ? hit.GetComponentInParent<EditableShape>() : null);
        }

        if (selected != null && tools.Length > 0)
            tools[toolIndex].Tick(selected);
    }

    public event System.Action<EditableShape> SelectionChanged;

    void SetSelected(EditableShape s)
    {
        if (selected != null) selected.Deselect();
        selected = s;
        if (selected != null) selected.Select();
        SelectionChanged?.Invoke(selected);
    }
}