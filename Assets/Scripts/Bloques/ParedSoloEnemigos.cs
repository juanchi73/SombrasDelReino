using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ParedSoloEnemigos : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemigos";
    [SerializeField] private string playerTag = "Player";

    private Collider2D wallCollider;

    private void Awake()
    {
        wallCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        IgnorarColisionesConJugador();
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

    private void IgnorarColisionesConJugador()
    {
        if (wallCollider == null || string.IsNullOrEmpty(playerTag))
        {
            return;
        }

        GameObject[] jugadores = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (GameObject jugador in jugadores)
        {
            if (jugador == null)
            {
                continue;
            }

            Collider2D[] collidersJugador = jugador.GetComponentsInChildren<Collider2D>(true);
            foreach (Collider2D colliderJugador in collidersJugador)
            {
                if (colliderJugador != null)
                {
                    Physics2D.IgnoreCollision(wallCollider, colliderJugador, true);
                }
            }
        }
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
