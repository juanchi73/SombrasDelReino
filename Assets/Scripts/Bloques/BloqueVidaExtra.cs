using System.Collections;
using UnityEngine;

public class BloqueVidaExtra : MonoBehaviour, IBloqueGolpeable
{
    [Header("Configuracion de Golpes")]
    [SerializeField] private int cantidadDeGolpes = 1;

    [Header("Ajustes de Animacion")]
    [SerializeField] private float alturaSalto = 0.5f;
    [SerializeField] private float tiempoSubida = 0.1f;
    [SerializeField] private float tiempoBajada = 0.15f;

    [Header("Vida Extra")]
    [SerializeField] private GameObject vidaExtraPrefab;
    [SerializeField] private Vector3 offsetSalidaVida = Vector3.zero;

    private Vector3 posicionOriginal;
    private bool estaAnimando;
    private int golpesRestantes;
    private Rigidbody2D rb;

    void Start()
    {
        posicionOriginal = transform.position;
        golpesRestantes = cantidadDeGolpes;
        rb = GetComponent<Rigidbody2D>();
    }

    public void RecibirGolpe()
    {
        if (golpesRestantes <= 0 || estaAnimando) return;

        golpesRestantes--;
        MostrarVidaExtra();
        StartCoroutine(AnimacionSalto());
    }

    private void MostrarVidaExtra()
    {
        if (vidaExtraPrefab == null) return;

        Vector3 posicionSpawn = transform.position + offsetSalidaVida;
        Instantiate(vidaExtraPrefab, posicionSpawn, Quaternion.identity);
    }

    private IEnumerator AnimacionSalto()
    {
        estaAnimando = true;

        RigidbodyConstraints2D constraintsBak = RigidbodyConstraints2D.None;
        if (rb != null)
        {
            constraintsBak = rb.constraints;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.linearVelocity = Vector2.zero;
        }

        transform.position = posicionOriginal;
        Vector3 posicionMaxima = posicionOriginal + Vector3.up * alturaSalto;

        float tiempoPasado = 0f;
        while (tiempoPasado < tiempoSubida)
        {
            tiempoPasado += Time.deltaTime;
            float progreso = tiempoPasado / tiempoSubida;
            float easedProgress = 1f - Mathf.Pow(1f - progreso, 2f);
            transform.position = Vector3.Lerp(posicionOriginal, posicionMaxima, easedProgress);
            yield return null;
        }

        tiempoPasado = 0f;
        while (tiempoPasado < tiempoBajada)
        {
            tiempoPasado += Time.deltaTime;
            float progreso = tiempoPasado / tiempoBajada;
            float easedProgress = progreso * progreso;
            transform.position = Vector3.Lerp(posicionMaxima, posicionOriginal, easedProgress);
            yield return null;
        }

        transform.position = posicionOriginal;

        if (rb != null)
        {
            rb.constraints = constraintsBak;
        }

        estaAnimando = false;
    }
}
