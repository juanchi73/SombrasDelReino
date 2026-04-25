using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Proyectil : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadHaciaArriba = 5f;

    [Header("Tiempo de Vida")]
    [SerializeField] private float tiempoAntesDeDespawnear = 3f;

    [Header("Impacto")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool destruirAlImpactar = true;

    private Rigidbody2D rb;
    private bool yaImpacto;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadHaciaArriba);

        if (tiempoAntesDeDespawnear > 0f)
        {
            Destroy(gameObject, tiempoAntesDeDespawnear);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IntentarImpactarMario(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IntentarImpactarMario(other);
    }

    private void IntentarImpactarMario(Collider2D colliderImpactado)
    {
        if (yaImpacto || colliderImpactado == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(playerTag) && !colliderImpactado.CompareTag(playerTag))
        {
            return;
        }

        MarioMovement mario = colliderImpactado.GetComponent<MarioMovement>();
        if (mario == null)
        {
            mario = colliderImpactado.GetComponentInParent<MarioMovement>();
        }

        if (mario == null)
        {
            return;
        }

        yaImpacto = true;
        mario.MorirPorPeligro();

        if (destruirAlImpactar)
        {
            Destroy(gameObject);
        }
    }
}
