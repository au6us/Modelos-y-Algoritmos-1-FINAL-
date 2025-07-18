using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [Header("Barra única de tiempo")]
    public Image starBar;         
        

    [Header("Tiempo total para vaciar todo (segundos)")]
    public float maxTime = 90f;   

    private float startTime;
    private float starTime;       

    void Start()
    {
        startTime = Time.time;

        starTime = maxTime / 3f;

        // Aseguramos que la barra y las estrellas estén llenas al inicio
        if (starBar != null) starBar.fillAmount = 1f;
    }

    void Update()
    {
        float elapsed = Time.time - startTime;
        
        // Actualizar la barra
        
        if (starBar != null)
        {
            float fillValue = 1f - (elapsed / maxTime);
            starBar.fillAmount = Mathf.Clamp01(fillValue);
        }
    }
}
