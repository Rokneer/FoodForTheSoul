using UnityEngine;

public class SoundFXManager : Singleton<SoundFXManager>
{
    [SerializeField]
    private GameObject soundFXObject;

    private int currentId = -1;

    public AudioClip PlaySFXClip(
        AudioClip audioClip,
        Transform spawnTransform,
        float volume,
        bool hasParent = false
    )
    {
        AudioSource audioSource = hasParent
            ? ObjectPoolManager
                .SpawnObject(
                    soundFXObject,
                    spawnTransform.position,
                    Quaternion.identity,
                    PoolType.SFXs
                )
                .GetComponent<AudioSource>()
            : ObjectPoolManager
                .SpawnObject(soundFXObject, spawnTransform)
                .GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        StartCoroutine(
            ObjectPoolManager.ReturnToPool(audioSource.gameObject, audioSource.clip.length)
        );

        return audioClip;
    }

    public AudioClip PlayRandomSFXClip(
        AudioClip[] audioClips,
        Transform spawnTransform,
        float volume,
        bool hasParent = false
    )
    {
        int sfxId = RandomIndex.GetRandomIndex(audioClips, currentId);
        currentId = sfxId;

        AudioClip selectedAudioClip = audioClips[sfxId];

        PlaySFXClip(selectedAudioClip, spawnTransform, volume, hasParent);

        return selectedAudioClip;
    }
}
