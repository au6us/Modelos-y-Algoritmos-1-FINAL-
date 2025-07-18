using UnityEngine;
using UnityEngine.UI;  
using TMPro;          

public class MenuController : MonoBehaviour
{
    public Button buttonLevel2;
    public JSONSaveHandler saveHandler;

    // TextMeshPro en lugar de Text (UI)
    public TMP_Text starsText;

    void Start()
    {
        // Carga las estrellas del nivel 1 para habilitar/deshabilitar el botón
        int starsLevel1 = saveHandler.LoadStars(1);
        buttonLevel2.interactable = (starsLevel1 == 3);

        // Si quieres mostrar la suma de estrellas de varios niveles
        int totalStars = 0;
        totalStars += saveHandler.LoadStars(1);
        totalStars += saveHandler.LoadStars(2);

        // Asigna ese valor al texto TextMeshPro
        starsText.text = totalStars.ToString();
    }
}
