using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Escenas")]
    [SerializeField] private string nombreEscenaNivel1 = "1-1";

    [Header("Paneles")]
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private GameObject panelJugar;
    [SerializeField] private GameObject panelCreditos;

    private void Start()
    {
        MostrarPanelPrincipal();
    }

    public void MostrarPanelPrincipal()
    {
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(true);
        }

        if (panelJugar != null)
        {
            panelJugar.SetActive(false);
        }

        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }
    }

    public void AbrirPanelJugar()
    {
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(false);
        }

        if (panelJugar != null)
        {
            panelJugar.SetActive(true);
        }

        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }
    }

    public void AbrirPanelCreditos()
    {
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(false);
        }

        if (panelJugar != null)
        {
            panelJugar.SetActive(false);
        }

        if (panelCreditos != null)
        {
            panelCreditos.SetActive(true);
        }
    }

    public void CargarNivel1()
    {
        if (!string.IsNullOrWhiteSpace(nombreEscenaNivel1))
        {
            SceneManager.LoadScene(nombreEscenaNivel1);
        }
    }

    public void SalirJuego()
    {
        Debug.Log("Cerrando juego...");
        Application.Quit();
    }
}
