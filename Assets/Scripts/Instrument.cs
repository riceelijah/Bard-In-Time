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
    public AudioClip useSound; // plays when the instrument acts on a shape
    public AudioClip equipSound; // plays when switching to it with the number keys
    // on: useSound plays while you keep using it and fades when you stop (scroll tools)
    // off: useSound plays once, all the way through, per use (click tools like the hammer)
    public bool sustain = true;
}
