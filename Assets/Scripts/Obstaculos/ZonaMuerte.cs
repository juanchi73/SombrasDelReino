using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ZonaMuerte : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void Reset()
    {
        Collider2D collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            collider2D.isTrigger = true;
        }
    }

    private void Awake()
    {
        Collider2D collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            collider2D.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
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

        if (mario != null)
        {
            mario.MorirPorPeligro();
        }
    }
}
