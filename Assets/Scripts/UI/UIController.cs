using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private PlayerModel playerModel;

    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentScore = 0;

    [Header("Time Settings")]
    [SerializeField] private Image timeBar;
    [SerializeField] private float maxTime = 90f;
    private float startTime;

    private void Awake()
    {
        playerModel.OnLifeChanged += UpdateHealthBar;
        GameEventManager.OnCollectibleEvent += HandleCollectibleEvent;

        InitializeUI();
    }

    private void OnDestroy()
    {
        playerModel.OnLifeChanged -= UpdateHealthBar;
        GameEventManager.OnCollectibleEvent -= HandleCollectibleEvent;
    }

    private void InitializeUI()
    {
        healthSlider.maxValue = playerModel.MaxLife;
        healthSlider.value = playerModel.Life;

        startTime = Time.time;
        if (timeBar != null) timeBar.fillAmount = 1f;

        currentScore = 0;
        UpdateScoreDisplay();
    }

    private void Update()
    {
        UpdateTimeBar();
    }

    private void UpdateHealthBar(int newLife)
    {
        healthSlider.value = newLife;
    }

    private void HandleCollectibleEvent(CollectibleEventData eventData)
    {
        currentScore += eventData.Points;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"{currentScore}";
    }

    private void UpdateTimeBar()
    {
        if (timeBar == null) return;

        float elapsed = Time.time - startTime;
        float fillValue = 1f - (elapsed / maxTime);
        timeBar.fillAmount = Mathf.Clamp01(fillValue);
    }
}