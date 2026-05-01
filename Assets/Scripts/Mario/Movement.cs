using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MarioMovement : MonoBehaviour
{
    private const string AnimatorSpeedX = "SpeedX";
    private const string AnimatorIsGrounded = "IsGrounded";
    private const string AnimatorIsJumping = "IsJumping";
    private const string AnimatorFacingRight = "FacingRight";

    [Header("Configuracion de Movimiento")]
    public float maxSpeed = 8f;
    public float acceleration = 90f;
    public float deceleration = 90f;
    public float friction = 100f;

    [Header("Configuracion de Salto")]
    public float jumpForce = 13f;
    public float fallMultiplier = 7f;
    public float lowJumpMultiplier = 4f;
    [Range(0, 1)] public float airControl = 0.85f;

    [Header("Deteccion de Suelo")]
    public Transform groundCheck;
    public float groundRadius = 0.25f;
    public LayerMask groundMask;

    [Header("Muerte")]
    [Tooltip("Etiqueta usada para reconocer a los enemigos")]
    public string enemyTag = "Enemy";
    public Sprite deathSprite;
    [Tooltip("Segundos antes de que Mario reaparezca")]
    public float respawnDelay = 2f;
    [Tooltip("Salto que hace Mario al morir")]
    public float deathBounce = 4f;

    [Header("Temporizador")]
    [Tooltip("Tiempo total en segundos antes de que Mario muera automaticamente")]
    [Min(0f)] public float timerDuration = 30f;
    [HideInInspector] [SerializeField] private float remainingTime;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D[] colliders;
    private float horizontalInput;
    private bool isGrounded;
    private bool jumpHeld;
    private bool jumpRequest;
    private bool isDead;
    private bool facingRight = true;
    private Vector3 spawnPosition;
    private float originalGravityScale;
    private Sprite initialSprite;
    private Coroutine respawnCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>(true);
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        remainingTime = timerDuration;
        spawnPosition = transform.position;
        originalGravityScale = rb.gravityScale;

        if (spriteRenderer != null)
        {
            initialSprite = spriteRenderer.sprite;
        }
    }

    void Update()
    {
        if (!isDead)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                Die();
            }
        }

        float move = 0f;
        if (Keyboard.current.dKey.isPressed) move = 1f;
        else if (Keyboard.current.aKey.isPressed) move = -1f;
        horizontalInput = move;

        jumpHeld = Keyboard.current.wKey.isPressed;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            jumpRequest = true;
        }

        UpdateFacingDirection();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        CheckGround();
        ApplyMovement();
        ApplyJump();
        ApplyGravityMultipliers();
        CheckGround();
        UpdateAnimator();
    }

    private void CheckGround()
    {
        if (groundCheck == null) return;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);
    }

    private void ApplyMovement()
    {
        float targetSpeed = horizontalInput * maxSpeed;
        float currentAccel;

        if (isGrounded)
        {
            if (Mathf.Abs(horizontalInput) > 0.01f)
            {
                bool isTurning = Mathf.Sign(targetSpeed) != Mathf.Sign(rb.linearVelocity.x) && Mathf.Abs(rb.linearVelocity.x) > 0.1f;
                currentAccel = isTurning ? friction : acceleration;
            }
            else
            {
                currentAccel = deceleration;
            }
        }
        else
        {
            currentAccel = acceleration * airControl;
        }

        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, currentAccel * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    private void ApplyJump()
    {
        if (jumpRequest && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        jumpRequest = false;
    }

    private void UpdateFacingDirection()
    {
        if (horizontalInput > 0.01f)
        {
            facingRight = true;
        }
        else if (horizontalInput < -0.01f)
        {
            facingRight = false;
        }
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        bool isJumping = !isGrounded;

        animator.SetFloat(AnimatorSpeedX, Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool(AnimatorIsGrounded, isGrounded);
        animator.SetBool(AnimatorIsJumping, isJumping);
        animator.SetBool(AnimatorFacingRight, facingRight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (!string.IsNullOrEmpty(enemyTag) && collision.collider.CompareTag(enemyTag))
        {
            bool stomped = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.3f)
                {
                    stomped = true;
                    break;
                }
            }

            if (!stomped)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        int vidasRestantes = 0;
        if (ContadorVidas.instancia != null)
        {
            vidasRestantes = ContadorVidas.instancia.RestarVidas(1);
        }
        else
        {
            Debug.LogWarning("MarioMovement: No se encontro ContadorVidas en la escena.");
        }

        if (deathSprite != null && spriteRenderer != null)
        {
            if (animator != null)
            {
                animator.enabled = false;
            }

            spriteRenderer.sprite = deathSprite;
        }

        DisableHitboxes();
        rb.linearVelocity = new Vector2(0f, deathBounce);
        rb.gravityScale = Mathf.Abs(originalGravityScale);

        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }

        if (vidasRestantes > 0)
        {
            respawnCoroutine = StartCoroutine(RespawnAfterDelay());
        }
    }

    public void MorirPorPeligro()
    {
        Die();
    }

    private void DisableHitboxes()
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }

    private void EnableHitboxes()
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
        respawnCoroutine = null;
    }

    private void Respawn()
    {
        transform.position = spawnPosition;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = originalGravityScale;
        remainingTime = timerDuration;
        horizontalInput = 0f;
        jumpHeld = false;
        jumpRequest = false;
        isDead = false;
        facingRight = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = initialSprite;
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }

        if (animator != null)
        {
            animator.enabled = true;
        }

        EnableHitboxes();
        UpdateAnimator();
    }

    public float ObtenerTiempoRestante()
    {
        return remainingTime;
    }

    public void ConfigurarTiempo(float nuevoTiempo)
    {
        timerDuration = Mathf.Max(0f, nuevoTiempo);
        remainingTime = timerDuration;
    }

    public void SumarVida(int cantidad = 1)
    {
        if (ContadorVidas.instancia != null)
        {
            ContadorVidas.instancia.SumarVidas(cantidad);
        }
    }

    private void ApplyGravityMultipliers()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !jumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
