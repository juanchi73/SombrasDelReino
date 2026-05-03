using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicaEscena : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.playOnAwake = true;
        }
    }
}
