using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonesFinNivel : MonoBehaviour
{
    [Header("Escenas")]
    [SerializeField] private string nombreEscenaMenu = "MenuPrincipal";
    [SerializeField] private string nombreEscenaNivel = "1-1";

    public void VolverAlMenu()
    {
        if (!string.IsNullOrWhiteSpace(nombreEscenaMenu))
        {
            SceneManager.LoadScene(nombreEscenaMenu);
        }
    }

    public void ReintentarNivel()
    {
        if (!string.IsNullOrWhiteSpace(nombreEscenaNivel))
        {
            SceneManager.LoadScene(nombreEscenaNivel);
        }
    }
}
