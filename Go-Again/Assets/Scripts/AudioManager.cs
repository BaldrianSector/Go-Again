using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AudioClipEntry
{
    public string customName;
    public AudioClip clip;

    [Header("Playback Settings")]
    public bool is3D = false;
    public bool isLooping = false;
    public float startTime = 0f;
    public float endTime = -1f; // -1 means play to end
    [Range(0f, 1f)]
    public float volume = 1f;

    [Header("Pitch Variation")]
    [Range(0.1f, 3f)]
    public float minPitch = 1f;
    [Range(0.1f, 3f)]
    public float maxPitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public List<AudioClipEntry> audioClips;

    private Dictionary<string, AudioClipEntry> clipDict;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            clipDict = new Dictionary<string, AudioClipEntry>();

            foreach (var entry in audioClips)
            {
                if (entry != null && entry.clip != null && !string.IsNullOrEmpty(entry.customName))
                {
                    if (!clipDict.ContainsKey(entry.customName))
                    {
                        clipDict.Add(entry.customName, entry);
                    }
                    else
                    {
                        Debug.LogWarning($"Duplicate customName detected in AudioManager: {entry.customName}");
                    }
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(string customName, Vector3? positionOverride = null)
    {
        if (!clipDict.ContainsKey(customName))
        {
            Debug.LogWarning("Audio clip not found: " + customName);
            return;
        }

        AudioClipEntry entry = clipDict[customName];
        AudioClip clip = entry.clip;

        GameObject audioObj = new GameObject("Audio_" + customName);
        AudioSource source = audioObj.AddComponent<AudioSource>();

        source.clip = clip;
        source.loop = entry.isLooping;
        source.volume = Mathf.Clamp01(entry.volume);
        source.spatialBlend = entry.is3D ? 1.0f : 0.0f;
        source.transform.position = positionOverride ?? Vector3.zero;

        float pitch = Random.Range(entry.minPitch, entry.maxPitch);
        source.pitch = pitch;

        source.time = Mathf.Clamp(entry.startTime, 0f, clip.length);
        source.Play();

        float effectiveEnd = (entry.endTime > 0f && entry.endTime > entry.startTime) ? entry.endTime : clip.length;
        float duration = Mathf.Min(effectiveEnd - entry.startTime, clip.length - entry.startTime) / pitch;

        if (!entry.isLooping)
        {
            Destroy(audioObj, duration + 0.1f);
        }
        else if (entry.endTime > 0f && entry.endTime > entry.startTime)
        {
            StartCoroutine(StopAudioAfter(source, duration, audioObj));
        }
    }

    private IEnumerator StopAudioAfter(AudioSource source, float delay, GameObject go)
    {
        yield return new WaitForSeconds(delay);
        source.Stop();
        Destroy(go);
    }
}
