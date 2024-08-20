using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] private AudioSource _soundFXObject;


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int random = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[random];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    internal void PlaySoundFXClip(AudioSource sonidoPatada, Transform transform, float v)
    {
        throw new System.NotImplementedException();
    }
}
