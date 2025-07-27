using System;
using UnityEngine;

public static class GameEventManager
{
    public static event Action<CollectibleEventData> OnCollectibleEvent;
    public static event Action<PlayerLifeEventData> OnPlayerLifeEvent;

    public static void TriggerCollectibleEvent(CollectibleType type, int points, Vector3 position)
    {
        OnCollectibleEvent?.Invoke(new CollectibleEventData
        {
            Type = type,
            Points = points,
            Position = position
        });
    }

    public static void TriggerPlayerLifeEvent(int currentLife, int maxLife)
    {
        OnPlayerLifeEvent?.Invoke(new PlayerLifeEventData
        {
            CurrentLife = currentLife,
            MaxLife = maxLife
        });
    }
}

public enum CollectibleType
{
    EnemyKilled,
    Coin,
    Fruit
}

public struct CollectibleEventData
{
    public CollectibleType Type;
    public int Points;
    public Vector3 Position;
}

public struct PlayerLifeEventData
{
    public int CurrentLife;
    public int MaxLife;
}