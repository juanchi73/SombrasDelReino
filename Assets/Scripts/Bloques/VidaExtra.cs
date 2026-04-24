using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class VidaExtra : MonoBehaviour
{
    [Header("Recolectable")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private int vidasQueOtorga = 1;

    [Header("Salida del Bloque")]
    [SerializeField] private float distanciaSalida = 1f;
    [SerializeField] private float duracionSalida = 0.25f;

    [Header("Movimiento")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool startFacingRight = true;

    [Header("Salto Aleatorio")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float minJumpInterval = 0.8f;
    [SerializeField] private float maxJumpInterval = 2f;

    [Header("Suelo")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.08f;

    private Rigidbody2D rb;
    private Collider2D itemCollider;
    private int direction;
    private bool isGrounded;
    private bool movementEnabled;
    private bool wasCollected;
    private float nextJumpTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        itemCollider = GetComponent<Collider2D>();
        rb.freezeRotation = true;
        direction = startFacingRight ? 1 : -1;
        ActualizarOrientacionVisual();
    }

    void Start()
    {
        StartCoroutine(SalirDelBloque());
    }

    void Update()
    {
        if (!movementEnabled || wasCollected) return;

        ActualizarSuelo();

        if (isGrounded && Time.time >= nextJumpTime)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            ProgramarProximoSalto();
        }
    }

    void FixedUpdate()
    {
        if (!movementEnabled || wasCollected) return;

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    private IEnumerator SalirDelBloque()
    {
        Vector3 posicionInicial = transform.position;
        Vector3 posicionFinal = posicionInicial + Vector3.up * distanciaSalida;

        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        rb.simulated = false;

        float tiempo = 0f;
        while (tiempo < duracionSalida)
        {
            tiempo += Time.deltaTime;
            float progreso = Mathf.Clamp01(tiempo / duracionSalida);
            transform.position = Vector3.Lerp(posicionInicial, posicionFinal, progreso);
            yield return null;
        }

        transform.position = posicionFinal;
        rb.simulated = true;

        if (itemCollider != null)
        {
            itemCollider.enabled = true;
        }

        movementEnabled = true;
        ProgramarProximoSalto();
    }

    private void ProgramarProximoSalto()
    {
        nextJumpTime = Time.time + Random.Range(minJumpInterval, maxJumpInterval);
    }

    private void ActualizarSuelo()
    {
        if (itemCollider == null) return;

        Bounds bounds = itemCollider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y + 0.02f);
        isGrounded = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundMask);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (wasCollected) return;

        if (collision.collider.CompareTag(playerTag))
        {
            Recolectar();
            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                direction *= -1;
                ActualizarOrientacionVisual();
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (wasCollected) return;

        if (other.CompareTag(playerTag))
        {
            Recolectar();
        }
    }

    private void Recolectar()
    {
        if (wasCollected) return;
        wasCollected = true;

        if (ContadorVidas.instancia != null)
        {
            ContadorVidas.instancia.SumarVidas(vidasQueOtorga);
        }
        else
        {
            Debug.LogWarning("VidaExtra: No se encontro ContadorVidas en la escena.");
        }

        Destroy(gameObject);
    }

    private void ActualizarOrientacionVisual()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction >= 0 ? 1f : -1f);
        transform.localScale = scale;
    }
}
