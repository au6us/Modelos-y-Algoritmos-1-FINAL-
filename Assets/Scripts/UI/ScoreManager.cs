using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;

    private void OnEnable()
    {
        GameEventManager.OnEnemyKilled += AddScore;
        GameEventManager.OnCoinCollected += AddScore;
        GameEventManager.OnFruitCollected += AddScore;
    }

    private void OnDisable()
    {
        GameEventManager.OnEnemyKilled -= AddScore;
        GameEventManager.OnCoinCollected -= AddScore;
        GameEventManager.OnFruitCollected -= AddScore;
    }

    private void AddScore(int pts)
    {
        score += pts;
        scoreText.text = score.ToString();
    }
}
