using UnityEngine;

public abstract class ShapeTool : MonoBehaviour
{
    public abstract Abilities Ability { get; }
    public abstract void Tick(EditableShape shape);
}