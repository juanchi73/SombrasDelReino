using System.Collections;
using UnityEngine;

public class ObstaculoDisparador : MonoBehaviour
{
    [Header("Proyectil")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Vector3 offsetSpawn = Vector3.up;

    [Header("Disparo")]
    [SerializeField] private float tiempoEntreSpawns = 1.5f;
    [SerializeField] private bool dispararAlIniciar = true;

    [Header("Retroceso")]
    [SerializeField] private float distanciaRetroceso = 0.15f;
    [SerializeField] private float tiempoBajada = 0.08f;
    [SerializeField] private float tiempoSubida = 0.12f;

    private Coroutine rutinaDisparo;
    private Coroutine rutinaRetroceso;
    private Vector3 posicionOriginal;

    private void Awake()
    {
        posicionOriginal = transform.position;
    }

    private void OnEnable()
    {
        posicionOriginal = transform.position;

        if (dispararAlIniciar)
        {
            IniciarDisparo();
        }
    }

    private void OnDisable()
    {
        DetenerDisparo();

        if (rutinaRetroceso != null)
        {
            StopCoroutine(rutinaRetroceso);
            rutinaRetroceso = null;
        }

        transform.position = posicionOriginal;
    }

    public void IniciarDisparo()
    {
        if (rutinaDisparo != null)
        {
            return;
        }

        rutinaDisparo = StartCoroutine(RutinaDisparo());
    }

    public void DetenerDisparo()
    {
        if (rutinaDisparo == null)
        {
            return;
        }

        StopCoroutine(rutinaDisparo);
        rutinaDisparo = null;
    }

    private IEnumerator RutinaDisparo()
    {
        while (true)
        {
            Disparar();

            if (tiempoEntreSpawns <= 0f)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(tiempoEntreSpawns);
            }
        }
    }

    private void Disparar()
    {
        if (proyectilPrefab == null)
        {
            return;
        }

        Vector3 posicionSpawn = transform.position + offsetSpawn;
        Instantiate(proyectilPrefab, posicionSpawn, Quaternion.identity);
        ReproducirRetroceso();
    }

    private void ReproducirRetroceso()
    {
        if (rutinaRetroceso != null)
        {
            StopCoroutine(rutinaRetroceso);
            transform.position = posicionOriginal;
        }

        rutinaRetroceso = StartCoroutine(AnimarRetroceso());
    }

    private IEnumerator AnimarRetroceso()
    {
        Vector3 posicionRetroceso = posicionOriginal + Vector3.down * distanciaRetroceso;

        float tiempo = 0f;
        while (tiempo < tiempoBajada)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempoBajada <= 0f ? 1f : Mathf.Clamp01(tiempo / tiempoBajada);
            transform.position = Vector3.Lerp(posicionOriginal, posicionRetroceso, progreso);
            yield return null;
        }

        transform.position = posicionRetroceso;

        tiempo = 0f;
        while (tiempo < tiempoSubida)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempoSubida <= 0f ? 1f : Mathf.Clamp01(tiempo / tiempoSubida);
            transform.position = Vector3.Lerp(posicionRetroceso, posicionOriginal, progreso);
            yield return null;
        }

        transform.position = posicionOriginal;
        rutinaRetroceso = null;
    }
}
