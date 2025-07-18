using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed = 0f; // Tiempo transcurrido en segundos
    private float timeLimit = 300f;  // Límite de 5 minutos (300 segundos)

    void Update()
    {
        // Si el tiempo no ha alcanzado el límite, sigue incrementando
        if (timeElapsed < timeLimit)
        {
            timeElapsed += Time.deltaTime; // Aumenta el tiempo con cada frame
            UpdateTimerDisplay();
        }
        else
        {
            timeElapsed = timeLimit; // Asegura que el cronómetro no pase de los 5 minutos
            // Aquí puedes poner lo que sucede cuando se alcanza el tiempo (por ejemplo, fin del nivel)
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60); // Calcula los minutos
        int seconds = Mathf.FloorToInt(timeElapsed % 60); // Calcula los segundos restantes

        // Actualiza el texto con el tiempo transcurrido
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}
