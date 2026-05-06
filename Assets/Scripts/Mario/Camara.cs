using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;

    [Header("Limites de la Zona Muerta")]
    public float xThreshold = 1f;
    public float yThreshold = 2f;

    private MarioMovement marioMovement;

    private void Awake()
    {
        CacheMarioMovement();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        if (marioMovement == null)
        {
            CacheMarioMovement();
        }

        if (marioMovement != null && marioMovement.EstaMuerto())
        {
            return;
        }

        Vector3 targetPosition = transform.position;

        float deltaX = target.position.x - transform.position.x;
        if (Mathf.Abs(deltaX) > xThreshold)
        {
            targetPosition.x = target.position.x - (xThreshold * Mathf.Sign(deltaX));
        }

        float deltaY = target.position.y - transform.position.y;
        if (deltaY > yThreshold)
        {
            targetPosition.y = target.position.y - yThreshold;
        }
        else if (deltaY < -yThreshold)
        {
            targetPosition.y = target.position.y + yThreshold;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    private void CacheMarioMovement()
    {
        if (target == null)
        {
            marioMovement = null;
            return;
        }

        marioMovement = target.GetComponent<MarioMovement>();
        if (marioMovement == null)
        {
            marioMovement = target.GetComponentInParent<MarioMovement>();
        }
    }
}
