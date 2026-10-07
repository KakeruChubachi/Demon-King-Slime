using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; }

    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip startClip;
    [SerializeField, Range(0f, 1f)] private float volume = 0.8f;

    private AudioSource source;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
    }

    public void PlayClick() => Play(clickClip);
    public void PlayStart() => Play(startClip);

    private void Play(AudioClip clip)
    {
        if (clip == null) return;
        source.PlayOneShot(clip, volume);
    }
}