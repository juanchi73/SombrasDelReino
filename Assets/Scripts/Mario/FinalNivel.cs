using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class FinalNivel : MonoBehaviour
{
    [Header("Deteccion")]
    [SerializeField] private string playerTag = "Player";

    [Header("Escena de Victoria")]
    [SerializeField] private string winSceneName = "WIN";

    private bool nivelCompletado;

    private void Awake()
    {
        Collider2D goalCollider = GetComponent<Collider2D>();
        goalCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (nivelCompletado || other == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(playerTag) && !other.CompareTag(playerTag))
        {
            return;
        }

        MarioMovement mario = other.GetComponent<MarioMovement>();
        if (mario == null)
        {
            mario = other.GetComponentInParent<MarioMovement>();
        }

        if (mario == null)
        {
            return;
        }

        nivelCompletado = true;
        if (!string.IsNullOrWhiteSpace(winSceneName))
        {
            SceneManager.LoadScene(winSceneName);
        }
    }
}
