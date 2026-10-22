using UnityEngine;

// One copy lives for the whole game so the music never restarts between scenes.
// It spawns itself from Assets/Resources/AudioManager.prefab, so no scene needs to contain it,
// and pressing Play on any level still gets music and sound effects.
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip music;
    [SerializeField, Range(0f, 1f)] float musicVolume = 0.5f;
    [SerializeField, Range(0f, 1f)] float sfxVolume = 1f;

    static AudioManager instance;
    AudioSource musicSource, sfxSource;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Spawn()
    {
        var prefab = Resources.Load<AudioManager>("AudioManager");
        if (prefab == null) return; // no prefab yet: the game just runs silent
        instance = Instantiate(prefab);
        DontDestroyOnLoad(instance.gameObject);
    }

    void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        if (music != null) musicSource.Play();

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = sfxVolume;
    }

    // Safe to call with a null clip or before the prefab exists, so instruments can be left without a sound.
    // PlayOneShot lets sounds overlap instead of cutting each other off.
    public static void PlaySfx(AudioClip clip)
    {
        if (instance != null && clip != null) instance.sfxSource.PlayOneShot(clip);
    }
}
