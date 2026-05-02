using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("Deteccion")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Transform respawnPoint;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    private bool activated;

    private void Awake()
    {
        Collider2D checkpointCollider = GetComponent<Collider2D>();
        checkpointCollider.isTrigger = true;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null && inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated || other == null)
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

        Vector3 checkpointPosition = respawnPoint != null ? respawnPoint.position : transform.position;
        mario.EstablecerCheckpoint(checkpointPosition);
        activated = true;
        ActualizarSprite();
    }

    private void ActualizarSprite()
    {
        if (spriteRenderer == null || activeSprite == null)
        {
            return;
        }

        spriteRenderer.sprite = activeSprite;
    }
}
