using UnityEngine;

// One copy lives for the whole game so the music never restarts between scenes.
// It spawns itself from Assets/Resources/AudioManager.prefab, so no scene needs to contain it,
// and pressing Play on any level still gets music and sound effects.
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip music;
    [SerializeField, Range(0f, 1f)] float musicVolume = 0.5f;
    [SerializeField, Range(0f, 1f)] float sfxVolume = 1f;
    [SerializeField] float sustainHold = 0.25f; // an instrument keeps playing this long after the last scroll step
    [SerializeField] float sustainFade = 0.4f; // then fades out over this many seconds

    static AudioManager instance;
    AudioSource musicSource, sfxSource, instrumentSource;
    float lastSustain;

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

        instrumentSource = gameObject.AddComponent<AudioSource>();
        instrumentSource.loop = true; // scrolling longer than the clip keeps the music going
    }

    void Update()
    {
        if (!instrumentSource.isPlaying) return;

        float quiet = Time.time - lastSustain - sustainHold; // seconds into the fade-out
        if (quiet <= 0f) return;
        instrumentSource.volume = sfxVolume * (1f - quiet / sustainFade);
        if (quiet >= sustainFade) instrumentSource.Stop();
    }

    // Safe to call with a null clip or before the prefab exists, so instruments can be left without a sound.
    // PlayOneShot lets sounds overlap instead of cutting each other off.
    public static void PlaySfx(AudioClip clip)
    {
        if (instance != null && clip != null) instance.sfxSource.PlayOneShot(clip);
    }

    // Call every time the instrument is used: the clip starts once, keeps playing while calls keep coming,
    // and fades out shortly after they stop, so rapid scroll steps never stack copies of it.
    public static void Sustain(AudioClip clip)
    {
        if (instance == null || clip == null) return;

        AudioSource s = instance.instrumentSource;
        if (s.clip != clip || !s.isPlaying)
        {
            s.clip = clip;
            s.Play();
        }
        s.volume = instance.sfxVolume; // undo any fade already in progress
        instance.lastSustain = Time.time;
    }
}
