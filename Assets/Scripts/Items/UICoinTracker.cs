using UnityEngine;
using UnityEngine.UI;

public class UICoinTracker : MonoBehaviour
{
    [Header("Imágenes de Monedas en la UI")]
    [Tooltip("Arrastrá acá las 3 (o más) imágenes de las monedas del Canvas en orden")]
    [SerializeField] private Image[] coinIcons;

    [Header("Configuración de Color/Alpha")]
    [SerializeField] private Color collectedColor = Color.white; // Prendida (Alpha 1)
    [SerializeField] private Color missingColor = new Color(1f, 1f, 1f, 0.3f); // Apagada (Alpha 0.3)

    private int currentCoins = 0;

    private void OnEnable()
    {
        // Nos suscribimos al evento cuando la UI se prende
        Coin.OnCoinCollectedEvent += HandleCoinCollected;
    }

    private void OnDisable()
    {
        // Nos desuscribimos cuando la UI se apaga (¡Buena práctica obligatoria!)
        Coin.OnCoinCollectedEvent -= HandleCoinCollected;
    }

    private void Start()
    {
        // Al arrancar el nivel, apagamos todas las monedas poniéndolas transparentes
        foreach (var icon in coinIcons)
        {
            icon.color = missingColor;
        }
    }

    private void HandleCoinCollected()
    {
        // Verificamos que no nos pasemos del límite de imágenes que pusimos
        if (currentCoins < coinIcons.Length)
        {
            // Prendemos la moneda actual y sumamos 1 al contador
            coinIcons[currentCoins].color = collectedColor;
            currentCoins++;
        }
    }
}