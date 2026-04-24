using System.Collections;
using UnityEngine;

public class MonedaEmergente : MonoBehaviour
{
    [SerializeField] private float distanciaSubida = 1.2f;
    [SerializeField] private float duracionSubida = 0.18f;
    [SerializeField] private float tiempoVisible = 0.35f;
    [SerializeField] private float duracionDesaparicion = 0.12f;

    private SpriteRenderer spriteRenderer;
    private bool yaSeReprodujo = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (!yaSeReprodujo)
        {
            Reproducir();
        }
    }

    public void Reproducir()
    {
        if (yaSeReprodujo) return;

        yaSeReprodujo = true;
        StartCoroutine(AnimarYDestruir());
    }

    private IEnumerator AnimarYDestruir()
    {
        Vector3 posicionInicial = transform.position;
        Vector3 posicionFinal = posicionInicial + Vector3.up * distanciaSubida;

        float tiempo = 0f;
        while (tiempo < duracionSubida)
        {
            tiempo += Time.deltaTime;
            float progreso = Mathf.Clamp01(tiempo / duracionSubida);
            transform.position = Vector3.Lerp(posicionInicial, posicionFinal, progreso);
            yield return null;
        }

        transform.position = posicionFinal;

        if (tiempoVisible > 0f)
        {
            yield return new WaitForSeconds(tiempoVisible);
        }

        if (spriteRenderer != null && duracionDesaparicion > 0f)
        {
            Color colorInicial = spriteRenderer.color;
            tiempo = 0f;

            while (tiempo < duracionDesaparicion)
            {
                tiempo += Time.deltaTime;
                float progreso = Mathf.Clamp01(tiempo / duracionDesaparicion);
                Color colorActual = colorInicial;
                colorActual.a = Mathf.Lerp(1f, 0f, progreso);
                spriteRenderer.color = colorActual;
                yield return null;
            }
        }

        Destroy(gameObject);
    }
}
