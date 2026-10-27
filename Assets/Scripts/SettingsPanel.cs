using UnityEngine;
using UnityEngine.InputSystem;

public enum Bind { Left, Right, Jump, Restart, Instrument1, Instrument2, Instrument3, Instrument4, Instrument5 }

// Keybinds and scroll sensitivity, saved in PlayerPrefs. Gameplay scripts read keys from here.
public static class GameSettings
{
    static readonly Key[] defaults = { Key.A, Key.D, Key.Space, Key.Z, Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5 };
    static readonly Key[] keys = (Key[])defaults.Clone();

    public static int Count => defaults.Length;
    public static float ScrollSensitivity = 1f; // multiplies each tool's step per scroll tick
    public static bool Paused; // settings panel is open: gameplay ignores input

    public static Key KeyFor(Bind b) => keys[(int)b];

    public static void Set(Bind b, Key k) { keys[(int)b] = k; PlayerPrefs.SetInt("bind_" + b, (int)k); }

    public static void SetScrollSensitivity(float v) { ScrollSensitivity = v; PlayerPrefs.SetFloat("scrollSensitivity", v); }

    public static void ResetDefaults()
    {
        for (int i = 0; i < Count; i++) Set((Bind)i, defaults[i]);
        SetScrollSensitivity(1f);
    }

    public static void Load()
    {
        for (int i = 0; i < Count; i++) {
            var k = (Key)PlayerPrefs.GetInt("bind_" + (Bind)i, (int)defaults[i]);
            // a bad saved value would make Keyboard.current[k] throw
            keys[i] = k != Key.None && System.Enum.IsDefined(typeof(Key), k) ? k : defaults[i];
        }
        ScrollSensitivity = PlayerPrefs.GetFloat("scrollSensitivity", 1f);
    }
}

// Esc opens the panel in every scene; it creates itself, so nothing needs adding to scenes.
// ponytail: IMGUI like the instrument label, swap for a Canvas panel when it needs styling
public class SettingsPanel : MonoBehaviour
{
    bool open;
    int listening = -1; // index of the Bind waiting for a key press, or -1
    float prevTimeScale = 1f;

    // matches the slot order of every level's InstrumentHolder; the Bind names stay so saved keys still load
    static readonly string[] instrumentNames = { "Trombone", "Hammer", "Flute", "Harp", "Lute" };
    static string LabelFor(Bind b) => b >= Bind.Instrument1 ? instrumentNames[b - Bind.Instrument1] : b.ToString();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Create()
    {
        GameSettings.Load();
        var go = new GameObject("SettingsPanel");
        go.AddComponent<SettingsPanel>();
        DontDestroyOnLoad(go);
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (listening >= 0) {
            if (kb.escapeKey.wasPressedThisFrame) { listening = -1; return; } // cancel rebinding
            foreach (var k in kb.allKeys) {
                if (k != null && k.wasPressedThisFrame) {
                    GameSettings.Set((Bind)listening, k.keyCode);
                    listening = -1;
                    break;
                }
            }
            return;
        }

        if (kb.escapeKey.wasPressedThisFrame) Toggle();
    }

    void Toggle()
    {
        open = !open;
        GameSettings.Paused = open;
        if (open) { prevTimeScale = Time.timeScale; Time.timeScale = 0f; }
        else { Time.timeScale = prevTimeScale; PlayerPrefs.Save(); }
    }

    void OnGUI()
    {
        if (!open) return;

        // lay out on a 540px-tall virtual screen so it's readable at any resolution
        float s = Screen.height / 540f;
        GUI.matrix = Matrix4x4.Scale(new Vector3(s, s, 1f));
        float w = Screen.width / s;

        GUILayout.BeginArea(new Rect(w / 2f - 200f, 40f, 400f, 440f), GUI.skin.box);
        GUILayout.Label("Settings (Esc to close)");

        for (int i = 0; i < GameSettings.Count; i++) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(LabelFor((Bind)i), GUILayout.Width(180f));
            string label = listening == i ? "Press a key... (Esc cancels)" : GameSettings.KeyFor((Bind)i).ToString();
            if (GUILayout.Button(label)) listening = i;
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10f);
        GUILayout.Label($"Scroll sensitivity: {GameSettings.ScrollSensitivity:0.00}x");
        float v = GUILayout.HorizontalSlider(GameSettings.ScrollSensitivity, 0.25f, 3f);
        if (v != GameSettings.ScrollSensitivity) GameSettings.SetScrollSensitivity(v);

        GUILayout.Space(10f);
        if (GUILayout.Button("Reset to defaults")) GameSettings.ResetDefaults();
        GUILayout.EndArea();
    }
}
