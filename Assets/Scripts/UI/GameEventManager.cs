using System;

public static class GameEventManager
{
    public static event Action<int> OnEnemyKilled;
    public static event Action<int> OnCoinCollected;
    public static event Action<int> OnFruitCollected;

    public static void EnemyKilled(int pts) => OnEnemyKilled?.Invoke(pts);
    public static void CoinCollected(int pts) => OnCoinCollected?.Invoke(pts);
    public static void FruitCollected(int pts) => OnFruitCollected?.Invoke(pts);
}
