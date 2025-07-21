using UnityEngine;

public class Coin : CollectibleBase
{
    [Header("Coin Specific")]
    [Tooltip("Puntos que otorga esta moneda")]
    [SerializeField] private int coinPoints = 5;

    protected override void OnCollected()
    {
        GameEventManager.CoinCollected(coinPoints);
    }
}
