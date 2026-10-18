using UnityEngine;
using UnityEngine.InputSystem;

public class DestroyTool : ShapeTool
{
    SelectionManager selection;

    public override Abilities Ability => Abilities.Destroy;

    void Awake() => selection = GetComponent<SelectionManager>();

    public override void Tick(EditableShape shape)
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;
        selection.SetSelected(null); // fires SelectionChanged so the instrument sprite hides
        Destroy(shape.gameObject);
    }
}