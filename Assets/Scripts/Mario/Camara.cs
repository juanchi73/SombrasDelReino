using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;        // El transform de Mario
    public float smoothSpeed = 5f;  // Qué tan suave es el seguimiento
    
    [Header("Límites de la Zona Muerta")]
    public float xThreshold = 1f;   // Distancia horizontal antes de que la cámara se mueva
    public float yThreshold = 2f;   // Distancia vertical antes de que la cámara suba

    void LateUpdate()
    {
        if (target == null) return;

        // Calculamos la posición deseada (empezamos con la actual de la cámara)
        Vector3 targetPosition = transform.position;

        // --- SEGUIMIENTO HORIZONTAL ---
        // Si la distancia entre Mario y la cámara es mayor al umbral X
        float deltaX = target.position.x - transform.position.x;
        if (Mathf.Abs(deltaX) > xThreshold)
        {
            // Movemos el objetivo de la cámara hacia Mario
            targetPosition.x = target.position.x - (xThreshold * Mathf.Sign(deltaX));
        }

        // --- SEGUIMIENTO VERTICAL (Estilo Mario) ---
        // Solo seguimos hacia arriba si Mario supera el umbral superior
        float deltaY = target.position.y - transform.position.y;
        if (deltaY > yThreshold)
        {
            targetPosition.y = target.position.y - yThreshold;
        }
        // Opcional: Si quieres que la cámara también baje cuando Mario cae mucho
        else if (deltaY < -yThreshold)
        {
            targetPosition.y = target.position.y + yThreshold;
        }

        // Aplicamos el movimiento de forma suave con Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}