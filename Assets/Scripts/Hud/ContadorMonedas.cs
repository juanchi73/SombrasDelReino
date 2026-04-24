using UnityEngine;
using TMPro;

public class ContadorMonedas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoMonedas;
    private int monedasTotales = 0;

    // Singleton
    public static ContadorMonedas instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (textoMonedas == null)
        {
            Debug.LogWarning("ContadorMonedas: No hay TextMeshProUGUI asignado en el Inspector");
        }
        ActualizarInterfaz();
    }

    /// <summary>
    /// Suma monedas al contador
    /// </summary>
    public void SumarMonedas(int cantidad = 1)
    {
        monedasTotales += cantidad;
        ActualizarInterfaz();
        Debug.Log("¡Monedas obtenidas! Total: " + monedasTotales);
    }

    /// <summary>
    /// Resta monedas al contador
    /// </summary>
    public void RestarMonedas(int cantidad = 1)
    {
        monedasTotales -= cantidad;
        if (monedasTotales < 0) monedasTotales = 0;
        ActualizarInterfaz();
        Debug.Log("Monedas gastadas. Total: " + monedasTotales);
    }

    /// <summary>
    /// Retorna el total de monedas
    /// </summary>
    public int ObtenerMonedas()
    {
        return monedasTotales;
    }

    /// <summary>
    /// Reinicia el contador de monedas
    /// </summary>
    public void ReiniciarMonedas()
    {
        monedasTotales = 0;
        ActualizarInterfaz();
        Debug.Log("Contador de monedas reiniciado");
    }

    /// <summary>
    /// Actualiza el texto en pantalla
    /// </summary>
    private void ActualizarInterfaz()
    {
        if (textoMonedas != null)
        {
            textoMonedas.text = "Monedas: " + monedasTotales;
        }
    }
}
