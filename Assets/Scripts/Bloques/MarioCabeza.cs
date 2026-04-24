using UnityEngine;

public class MarioCabeza : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si chocamos con el sensor del bloque
        if (collision.CompareTag("SensorBloque"))
        {
            // Buscamos cualquier bloque golpeable en el padre y ejecutamos el golpe
            IBloqueGolpeable bloque = collision.GetComponentInParent<IBloqueGolpeable>();
            if (bloque != null)
            {
                bloque.RecibirGolpe();
            }
        }
    }
}
