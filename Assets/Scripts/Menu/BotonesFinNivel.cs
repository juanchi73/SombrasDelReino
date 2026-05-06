using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BotonesFinNivel : MonoBehaviour
{
    [Header("Escenas")]
#if UNITY_EDITOR
    [SerializeField] private SceneAsset menuSceneAsset;
    [SerializeField] private SceneAsset nivelSceneAsset;
#endif
    [SerializeField] private string nombreEscenaMenu = "MenuPrincipal";
    [SerializeField] private string nombreEscenaNivel = "1-1";

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrWhiteSpace(nombreEscenaMenu))
        {
            SceneManager.LoadScene(nombreEscenaMenu);
        }
        else
        {
            Debug.LogWarning("BotonesFinNivel: No hay escena de menu configurada.");
        }
    }

    public void ReintentarNivel()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrWhiteSpace(nombreEscenaNivel))
        {
            SceneManager.LoadScene(nombreEscenaNivel);
        }
        else
        {
            Debug.LogWarning("BotonesFinNivel: No hay escena de nivel configurada.");
        }
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (menuSceneAsset != null)
        {
            nombreEscenaMenu = menuSceneAsset.name;
        }

        if (nivelSceneAsset != null)
        {
            nombreEscenaNivel = nivelSceneAsset.name;
        }
#endif
    }
}
