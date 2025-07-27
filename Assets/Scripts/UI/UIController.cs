using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private Slider healthSlider;

    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentScore = 0;

    [Header("Time Settings")]
    [SerializeField] private Image timeBar;
    [SerializeField] private float maxTime = 90f;
    private float startTime;

    private void Awake()
    {
        // Hacer persistente el Canvas entre escenas
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Suscripción a eventos
        GameEventManager.OnCollectibleEvent += HandleCollectibleEvent;
        GameEventManager.OnPlayerLifeEvent += HandlePlayerLifeEvent;

        // Inicialización
        startTime = Time.time;
        UpdateScoreDisplay();
    }

    private void OnDestroy()
    {
        GameEventManager.OnCollectibleEvent -= HandleCollectibleEvent;
        GameEventManager.OnPlayerLifeEvent -= HandlePlayerLifeEvent;
    }

    private void Update()
    {
        UpdateTimeBar();
    }

    private void HandlePlayerLifeEvent(PlayerLifeEventData lifeData)
    {
        // Actualizar solo la barra de vida
        if (healthSlider != null)
        {
            healthSlider.maxValue = lifeData.MaxLife;
            healthSlider.value = lifeData.CurrentLife;
        }
    }

    private void HandleCollectibleEvent(CollectibleEventData eventData)
    {
        currentScore += eventData.Points;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{currentScore}";
        }
    }

    private void UpdateTimeBar()
    {
        if (timeBar == null) return;

        float elapsed = Time.time - startTime;
        float fillValue = 1f - (elapsed / maxTime);
        timeBar.fillAmount = Mathf.Clamp01(fillValue);
    }

    // Lógica adicional según tus necesidades
    public void ResetTimer()
    {
        startTime = Time.time;
    }

    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
    }
}