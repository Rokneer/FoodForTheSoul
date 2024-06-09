using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }

    [SerializeField]
    private GameObject soundFXObject;

    private int currentId = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
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
        int sfxId = RandomIndex.GetRandomIndex(audioClips, currentId);
        currentId = sfxId;

        PlaySoundFXClip(audioClips[sfxId], spawnTransform, volume);
    }
}
