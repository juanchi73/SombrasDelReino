using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class Woomba : MonoBehaviour
{
    [Header("Velocidad")]
    public float speed = 2f;

    [Header("Dirección inicial")]
    public bool startFacingRight = false;

    [Header("Configuración de exclusión")]
    [Tooltip("Etiqueta del jugador para que el woomba no cambie de dirección al chocar con Mario")]
    public string playerTag = "Player";

    [Header("Stomp")]
    [Tooltip("Velocidad vertical que recibe Mario al rebotar sobre el woomba")]
    public float stompBounce = 12f;
    [Tooltip("Umbral de normal para reconocer un stomp desde arriba")]
    public float stompNormalThreshold = 0.5f;
    [Tooltip("Altura mínima del punto de contacto para considerar un stomp")]
    public float stompContactHeight = 0.1f;

    [Header("Muerte")]
    public Sprite deathSprite;
    [Tooltip("Salto que da el woomba al morir")]
    public float deathBounce = 2f;
    [Tooltip("Segundos antes de que el woomba desaparezca después de morir")]
    public float deathDestroyDelay = 1.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private int direction = -1;
    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;

        direction = startFacingRight ? 1 : -1;
        UpdateSpriteDirection();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (!string.IsNullOrEmpty(playerTag) && collision.collider.CompareTag(playerTag))
        {
            bool stomped = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                bool verticalContact = Mathf.Abs(contact.normal.y) > stompNormalThreshold;
                bool topContact = contact.point.y > transform.position.y + stompContactHeight;
                if (verticalContact && topContact)
                {
                    stomped = true;
                    break;
                }
            }

            if (stomped)
            {
                StompedByPlayer(collision.collider);
            }

            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Si la colisión es principalmente horizontal, giramos.
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                ReverseDirection();
                return;
            }
        }
    }

    private void StompedByPlayer(Collider2D player)
    {
        if (isDead) return;

        isDead = true;
        BouncePlayer(player);
        StartDeathSequence();
    }

    private void BouncePlayer(Collider2D player)
    {
        Rigidbody2D playerRb = player.attachedRigidbody;
        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounce);
        }
    }

    private void StartDeathSequence()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }

        if (deathSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = deathSprite;
        }

        DisableHitboxes();
        rb.linearVelocity = new Vector2(0f, deathBounce);
        rb.gravityScale = Mathf.Abs(rb.gravityScale);
        StartCoroutine(WaitThenDestroy());
    }

    private void DisableHitboxes()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(deathDestroyDelay);
        Destroy(gameObject);
    }

    private void ReverseDirection()
    {
        direction *= -1;
        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction >= 0 ? 1f : -1f);
        transform.localScale = scale;
    }
}
