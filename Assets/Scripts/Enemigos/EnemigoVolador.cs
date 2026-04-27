using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class EnemigoVolador : MonoBehaviour
{
    [Header("Jugador")]
    [SerializeField] private string playerTag = "Player";

    [Header("Movimiento Horizontal")]
    [SerializeField] private float horizontalSpeed = 2f;
    [SerializeField] private bool direccionInicialAleatoria = true;
    [SerializeField] private bool startFacingRight = true;
    [SerializeField] private float tiempoCambioDireccion = 2f;

    [Header("Movimiento Vertical")]
    [SerializeField] private float verticalAmplitude = 0.8f;
    [SerializeField] private float verticalCycleDuration = 1.6f;

    [Header("Stomp")]
    [SerializeField] private float stompBounce = 12f;
    [SerializeField] private float stompNormalThreshold = 0.5f;
    [SerializeField] private float stompContactHeight = 0.1f;

    [Header("Muerte")]
    [SerializeField] private Sprite deathSprite;
    [SerializeField] private float deathBounce = 2f;
    [SerializeField] private float deathDestroyDelay = 1.5f;
    [SerializeField] private float deathGravityScale = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 startPosition;
    private int direction = 1;
    private float elapsed;
    private float directionChangeTimer;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = rb.position;

        rb.freezeRotation = true;
        rb.gravityScale = 0f;

        if (direccionInicialAleatoria)
        {
            direction = Random.value < 0.5f ? -1 : 1;
        }
        else
        {
            direction = startFacingRight ? 1 : -1;
        }

        ActualizarOrientacionVisual();
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        elapsed += Time.fixedDeltaTime;
        directionChangeTimer += Time.fixedDeltaTime;

        if (tiempoCambioDireccion > 0f && directionChangeTimer >= tiempoCambioDireccion)
        {
            direction *= -1;
            directionChangeTimer = 0f;
            ActualizarOrientacionVisual();
        }

        float cycleDuration = Mathf.Max(0.01f, verticalCycleDuration);
        float phase = elapsed / cycleDuration * Mathf.PI * 2f;
        float verticalOffset = Mathf.Sin(phase) * verticalAmplitude;

        Vector2 siguientePosicion = rb.position;
        siguientePosicion.x += direction * horizontalSpeed * Time.fixedDeltaTime;
        siguientePosicion.y = startPosition.y + verticalOffset;

        rb.MovePosition(siguientePosicion);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
        {
            return;
        }

        if (!string.IsNullOrEmpty(playerTag) && collision.collider.CompareTag(playerTag))
        {
            if (FuePisadoPorMario(collision))
            {
                MorirPisado(collision.collider);
            }
            else
            {
                MarioMovement mario = collision.collider.GetComponent<MarioMovement>();
                if (mario == null)
                {
                    mario = collision.collider.GetComponentInParent<MarioMovement>();
                }

                if (mario != null)
                {
                    mario.MorirPorPeligro();
                }
            }

            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                direction *= -1;
                directionChangeTimer = 0f;
                ActualizarOrientacionVisual();
                return;
            }
        }
    }

    private bool FuePisadoPorMario(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            bool contactoVertical = Mathf.Abs(contact.normal.y) > stompNormalThreshold;
            bool contactoArriba = contact.point.y > transform.position.y + stompContactHeight;
            if (contactoVertical && contactoArriba)
            {
                return true;
            }
        }

        return false;
    }

    private void MorirPisado(Collider2D player)
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        RebotarMario(player);

        if (deathSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = deathSprite;
        }

        DesactivarHitboxes();
        rb.gravityScale = deathGravityScale;
        rb.linearVelocity = new Vector2(0f, deathBounce);
        StartCoroutine(EsperarYDestruir());
    }

    private void RebotarMario(Collider2D player)
    {
        Rigidbody2D playerRb = player.attachedRigidbody;
        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounce);
        }
    }

    private void DesactivarHitboxes()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private IEnumerator EsperarYDestruir()
    {
        yield return new WaitForSeconds(deathDestroyDelay);
        Destroy(gameObject);
    }

    private void ActualizarOrientacionVisual()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction >= 0 ? 1f : -1f);
        transform.localScale = scale;
    }
}
