using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject panelPausa;

    [Header("Escenas")]
    [SerializeField] private string nombreEscenaMenu = "MenuPrincipal";

    [Header("Controles")]
    [SerializeField] private Key teclaPausa = Key.Escape;

    private bool juegoPausado;

    private void Start()
    {
        ReanudarEstadoInicial();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        if (keyboard[teclaPausa].wasPressedThisFrame)
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        juegoPausado = true;
        Time.timeScale = 0f;

        if (panelPausa != null)
        {
            panelPausa.SetActive(true);
        }
    }

    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f;

        if (panelPausa != null)
        {
            panelPausa.SetActive(false);
        }
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrWhiteSpace(nombreEscenaMenu))
        {
            SceneManager.LoadScene(nombreEscenaMenu);
        }
    }

    public void SalirJuego()
    {
        Time.timeScale = 1f;
        Debug.Log("Cerrando juego...");
        Application.Quit();
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }

    private void ReanudarEstadoInicial()
    {
        juegoPausado = false;
        Time.timeScale = 1f;

        if (panelPausa != null)
        {
            panelPausa.SetActive(false);
        }
    }
}
