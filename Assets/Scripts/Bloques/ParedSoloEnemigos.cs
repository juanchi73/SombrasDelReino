using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ParedSoloEnemigos : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemigos";

    private Collider2D wallCollider;

    private void Awake()
    {
        wallCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IntentarIgnorarNoEnemigos(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IntentarIgnorarNoEnemigos(other);
    }

    private void IntentarIgnorarNoEnemigos(Collider2D other)
    {
        if (wallCollider == null || other == null)
        {
            return;
        }

        if (TieneTagEnemigo(other))
        {
            return;
        }

        Physics2D.IgnoreCollision(wallCollider, other, true);
    }

    private bool TieneTagEnemigo(Collider2D other)
    {
        if (string.IsNullOrEmpty(enemyTag))
        {
            return false;
        }

        return other.CompareTag(enemyTag) || other.transform.root.CompareTag(enemyTag);
    }
}
