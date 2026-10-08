using UnityEngine;

[CreateAssetMenu(fileName = "NewInstrument", menuName = "Instruments/Instrument")]
public class Instrument : ScriptableObject
{
    public string instrumentName;
    public Sprite instrumentSprite;
}
