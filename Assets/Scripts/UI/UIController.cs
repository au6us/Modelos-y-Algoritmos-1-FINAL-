using System.Collections;
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

    [Header("Lives Settings")]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private float livesPunchScale = 0.4f;
    [SerializeField] private float livesPunchDuration = 0.25f;
    private Coroutine livesPunchRoutine;

    private void Awake()
    {
        // Hacer persistente el Canvas entre escenas
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Suscripci�n a eventos
        GameEventManager.OnCollectibleEvent += HandleCollectibleEvent;
        GameEventManager.OnPlayerLifeEvent += HandlePlayerLifeEvent;
        GameEventManager.OnPlayerLivesEvent += HandlePlayerLivesEvent;

        // Inicializaci�n
        UpdateScoreDisplay();

        // Sync inicial de vidas: el Awake de GameplayManager (que dispara el evento) corre
        // antes que este Start -todos los Awake de la escena van antes que cualquier
        // Start-, asi que pudimos habernos perdido el primer aviso. Lo leemos directo
        // por las dudas, asi el HUD nunca arranca desactualizado.
        if (GameplayManager.Instance != null)
        {
            HandlePlayerLivesEvent(new PlayerLivesEventData
            {
                CurrentLives = GameplayManager.Instance.CurrentLives,
                MaxLives = GameplayManager.Instance.MaxLives
            });
        }
    }

    private void OnDestroy()
    {
        GameEventManager.OnCollectibleEvent -= HandleCollectibleEvent;
        GameEventManager.OnPlayerLifeEvent -= HandlePlayerLifeEvent;
        GameEventManager.OnPlayerLivesEvent -= HandlePlayerLivesEvent;
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

    private void HandlePlayerLivesEvent(PlayerLivesEventData livesData)
    {
        if (livesText == null) return;

        livesText.text = livesData.CurrentLives.ToString();

        if (livesPunchRoutine != null) StopCoroutine(livesPunchRoutine);
        livesPunchRoutine = StartCoroutine(PunchLivesText());
    }

    // Chiquita animacion de "punch" (escala hacia arriba y vuelta a la normalidad) cada
    // vez que cambian las vidas. Usa tiempo sin escalar porque HandleGameOver() pone
    // Time.timeScale en 0 apenas se pierde la ultima vida, y no queremos que la
    // animacion quede congelada a mitad de camino justo cuando mas se nota.
    private IEnumerator PunchLivesText()
    {
        float elapsed = 0f;
        while (elapsed < livesPunchDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / livesPunchDuration);
            float scale = 1f + Mathf.Sin(t * Mathf.PI) * livesPunchScale;
            livesText.transform.localScale = Vector3.one * scale;
            yield return null;
        }
        livesText.transform.localScale = Vector3.one;
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

    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
    }
}