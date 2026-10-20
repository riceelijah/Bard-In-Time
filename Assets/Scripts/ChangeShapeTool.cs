using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeShapeTool : ShapeTool
{
    [SerializeField] EditableShape[] shapePrefabs; // cycle order, e.g. Box, Triangle, Circle

    SelectionManager selection;

    public override Abilities Ability => Abilities.ChangeShape;

    void Awake() => selection = GetComponent<SelectionManager>();

    public override void Tick(EditableShape shape)
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame || shapePrefabs.Length == 0) return;

        // -1 (unknown type) wraps to the first prefab
        int i = System.Array.FindIndex(shapePrefabs, p => p.GetType() == shape.GetType());
        EditableShape next = shapePrefabs[(i + 1) % shapePrefabs.Length];

        Transform t = shape.transform;
        EditableShape made = Instantiate(next, t.position, t.rotation);
        made.SetSize(t.localScale);

        var oldRb = shape.GetComponent<Rigidbody2D>();
        var newRb = made.GetComponent<Rigidbody2D>();
        newRb.linearVelocity = oldRb.linearVelocity;
        newRb.angularVelocity = oldRb.angularVelocity;
        newRb.constraints = oldRb.constraints; // keep a frozen shape frozen

        selection.SetSelected(made);
        Destroy(shape.gameObject);
    }
}