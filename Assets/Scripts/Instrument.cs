using UnityEngine;

public enum Abilities { Scale, Rotate, Levitate, Destroy, ChangeShape };

[CreateAssetMenu(fileName = "NewInstrument", menuName = "Instruments/Instrument")]
public class Instrument : ScriptableObject
{
    public string instrumentName;
    public Sprite instrumentSprite;
    public Abilities ability;
    public Vector3 offset;
    public Vector3 scale = Vector3.one;
    public float rotation;
    public AudioClip useSound; // plays each time the instrument acts on a shape
    public AudioClip equipSound; // plays when switching to it with the number keys
}
