using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trofeo : MonoBehaviour
{
    private void Reset()
    {
        // Nos aseguramos de que sea un Trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si es el jugador el que tocó el trofeo
        if (!other.CompareTag("Player")) return;

        Debug.Log("¡El jugador tocó el Trofeo!");

        // --- LLAMADA CLAVE AL CONTROLADOR CENTRAL ---
        if (GameplayManager.Instance != null)
        {
            GameplayManager.Instance.HandleWin();

            // Opcional: Desactivamos el objeto Trofeo para que no se toque dos veces
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("ERROR: No se encontró el 'GameplayManager' en la escena.");
        }
    }
}