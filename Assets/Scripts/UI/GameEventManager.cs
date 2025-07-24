using System;
using UnityEngine;

public static class GameEventManager
{
    public static event Action<CollectibleEventData> OnCollectibleEvent;

    public static void TriggerCollectibleEvent(CollectibleType type, int points, Vector3 position)
    {
        OnCollectibleEvent?.Invoke(new CollectibleEventData
        {
            Type = type,
            Points = points,
            Position = position
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