using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [Header("Barra única de tiempo")]
    public Image starBar;         

    [Header("Estrellas (3 imágenes)")]
    public Image star3;           
    public Image star2;           
    public Image star1;          

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

        star3.fillAmount = 1f;
        star2.fillAmount = 1f;
        star1.fillAmount = 1f;
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

        // Actualizar las estrellas
        // Tramo 1: de 0 a starTime (ej. 0 a 30s si maxTime=90)
        if (elapsed < starTime)
        {
            float t = elapsed / starTime;
            star3.fillAmount = 1f - t;   // se vacía star3
            star2.fillAmount = 1f;       // star2 y star1 siguen llenas
            star1.fillAmount = 1f;
        }
        // De starTime a starTime*2 (30s a 60s)
        else if (elapsed < starTime * 2f)
        {
            star3.fillAmount = 0f;       // star3 ya está vacía
            float t = (elapsed - starTime) / starTime;
            star2.fillAmount = 1f - t;   // se vacía star2
            star1.fillAmount = 1f;
        }
        // De starTime*2 a starTime*3 (60s a 90s)
        else if (elapsed < starTime * 3f)
        {
            star3.fillAmount = 0f;
            star2.fillAmount = 0f;
            float t = (elapsed - starTime * 2f) / starTime;
            star1.fillAmount = 1f - t;   // se vacía star1
        }
        // Si se pasa de 90s (starTime*3), todo está vacío
        else
        {
            star3.fillAmount = 0f;
            star2.fillAmount = 0f;
            star1.fillAmount = 0f;
        }
    }
}
