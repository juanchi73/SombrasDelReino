using UnityEngine;
using System.Collections;

public class BloqueInteractuable : MonoBehaviour, IBloqueGolpeable
{
    [Header("Configuración de Saltos")]
    [SerializeField] private int cantidadDeSaltos = 5; 
    [SerializeField] private int saltos = 0;

    [Header("Ajustes de Animación")]
    [SerializeField] private float alturaSalto = 0.5f;
    [SerializeField] private float tiempoSubida = 0.1f;
    [SerializeField] private float tiempoBajada = 0.15f;

    private Vector3 posicionOriginal;
    private bool estaAnimando = false;
    private Rigidbody2D rb;

    void Start()
    {
        posicionOriginal = transform.position;
        saltos = cantidadDeSaltos;
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Llamado cuando Mario golpea el bloque
    /// </summary>
    public void RecibirGolpe()
    {
        if (saltos > 0 && !estaAnimando)
        {
            saltos--;
            StartCoroutine(AnimacionSalto());
        }
    }

    /// <summary>
    /// Anima el salto del bloque hacia arriba y luego cae
    /// </summary>
    private IEnumerator AnimacionSalto()
    {
        estaAnimando = true;
        
        // Congelamos el Rigidbody para que no interfiera (si existe)
        RigidbodyConstraints2D constraintsBak = RigidbodyConstraints2D.None;
        if (rb != null)
        {
            constraintsBak = rb.constraints;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.linearVelocity = Vector2.zero;
        }

        // Posición inicial exacta
        transform.position = posicionOriginal;
        Vector3 posicionMaxima = posicionOriginal + Vector3.up * alturaSalto;

        // FASE 1: EL BLOQUE SUBE (aceleración suave)
        float tiempoPasado = 0;
        while (tiempoPasado < tiempoSubida)
        {
            tiempoPasado += Time.deltaTime;
            float progreso = tiempoPasado / tiempoSubida;
            
            // Usamos EaseOut para una subida más natural
            float easedProgress = 1f - Mathf.Pow(1f - progreso, 2f);
            transform.position = Vector3.Lerp(posicionOriginal, posicionMaxima, easedProgress);
            
            yield return null;
        }

        // FASE 2: EL BLOQUE CAE (aceleración por gravedad)
        tiempoPasado = 0;
        while (tiempoPasado < tiempoBajada)
        {
            tiempoPasado += Time.deltaTime;
            float progreso = tiempoPasado / tiempoBajada;
            
            // Usamos EaseIn para simular gravedad
            float easedProgress = progreso * progreso;
            transform.position = Vector3.Lerp(posicionMaxima, posicionOriginal, easedProgress);
            
            yield return null;
        }

        // Aseguramos que termine en la posición original
        transform.position = posicionOriginal;
        
        // Restauramos los constraints del Rigidbody (si existe)
        if (rb != null)
        {
            rb.constraints = constraintsBak;
        }
        estaAnimando = false;
    }
}
