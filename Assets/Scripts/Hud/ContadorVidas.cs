using TMPro;
using UnityEngine;

public class ContadorVidas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoVidas;
    [Min(0)] [SerializeField] private int vidasIniciales = 3;

    private int vidasTotales;

    public static ContadorVidas instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        vidasTotales = vidasIniciales;
    }

    void Start()
    {
        if (textoVidas == null)
        {
            Debug.LogWarning("ContadorVidas: No hay TextMeshProUGUI asignado en el Inspector");
        }

        ActualizarInterfaz();
    }

    public void SumarVidas(int cantidad = 1)
    {
        vidasTotales += Mathf.Max(0, cantidad);
        ActualizarInterfaz();
        Debug.Log("Vida extra obtenida. Total: " + vidasTotales);
    }

    public int RestarVidas(int cantidad = 1)
    {
        vidasTotales -= Mathf.Max(0, cantidad);
        if (vidasTotales < 0) vidasTotales = 0;
        ActualizarInterfaz();
        Debug.Log("Mario perdio una vida. Total: " + vidasTotales);
        return vidasTotales;
    }

    public int ObtenerVidas()
    {
        return vidasTotales;
    }

    public void ReiniciarVidas()
    {
        vidasTotales = vidasIniciales;
        ActualizarInterfaz();
    }

    private void ActualizarInterfaz()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidasTotales;
        }
    }
}
