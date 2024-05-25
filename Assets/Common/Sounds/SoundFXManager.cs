using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    private static SoundFXManager _instance;
    public static SoundFXManager Instance => _instance;

    [SerializeField]
    private GameObject soundFXObject;

    private readonly int lastIndex = -1;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = ObjectPoolManager
            .SpawnObject(
                soundFXObject,
                spawnTransform.position,
                Quaternion.identity,
                PoolType.GameObjects
            )
            .GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        StartCoroutine(
            ObjectPoolManager.ReturnToPool(audioSource.gameObject, audioSource.clip.length)
        );
    }

    public void PlayRandomSoundFXClip(
        AudioClip[] audioClips,
        Transform spawnTransform,
        float volume
    )
    {
        int randomIndex = RandomIndex.GetRandomIndex(audioClips, lastIndex);
        PlaySoundFXClip(audioClips[randomIndex], spawnTransform, volume);
    }
}
