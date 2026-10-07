using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [SerializeField] private AudioClip[] bgmList;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;

    private AudioSource source;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = false;
        source.volume = volume;
    }

    public void Play(int index)
    {
        if (index < 0 || index >= bgmList.Length) return;
        if (source.clip == bgmList[index] && source.isPlaying) return; // “¯‚¶‹È‚È‚çÄ¶‚µ’¼‚³‚È‚¢
        source.clip = bgmList[index];
        source.Play();
    }

    public void Stop() => source.Stop();

    public void SetVolume(float v)
    {
        volume = Mathf.Clamp01(v);
        source.volume = volume;
    }
}