using UnityEngine;
using TMPro;

public class ContadorTiempo : MonoBehaviour
{
    [SerializeField] private MarioMovement mario;
    [SerializeField] private TextMeshProUGUI textoTiempo;
    [Min(0f)] [SerializeField] private float duracionTiempo = 30f;

    void Start()
    {
        if (mario != null)
        {
            mario.ConfigurarTiempo(duracionTiempo);
        }
    }

    void Update()
    {
        if (mario == null || textoTiempo == null) return;

        float tiempoRestante = mario.ObtenerTiempoRestante();
        int segundos = Mathf.FloorToInt(tiempoRestante);
        int centesimas = Mathf.FloorToInt((tiempoRestante - segundos) * 100f);

        if (centesimas < 0) centesimas = 0;

        textoTiempo.text = segundos.ToString("00") + ":" + centesimas.ToString("00");
    }
}
