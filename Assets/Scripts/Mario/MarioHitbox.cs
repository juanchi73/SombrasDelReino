using UnityEngine;

public class MarioHitbox : MonoBehaviour
{
    [Header("Configuracion del Detector")]
    public float rayDistance = 0.7f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, rayDistance, groundLayer);

        if (hit.collider == null) return;
        if (rb.linearVelocity.y <= 0.1f) return;

        IBloqueGolpeable bloqueGolpeable = hit.collider.GetComponent<IBloqueGolpeable>();
        if (bloqueGolpeable == null)
        {
            bloqueGolpeable = hit.collider.GetComponentInParent<IBloqueGolpeable>();
        }

        if (bloqueGolpeable != null)
        {
            bloqueGolpeable.RecibirGolpe();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector2.up * rayDistance);
    }
}
