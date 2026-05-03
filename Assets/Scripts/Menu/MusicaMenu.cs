using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicaMenu : MonoBehaviour
{
    [Header("Escena donde debe apagarse")]
    [SerializeField] private string nombreEscenaNivel1 = "1-1";

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = true;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        if (escena.name == nombreEscenaNivel1)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            Destroy(gameObject);
        }
    }
}
