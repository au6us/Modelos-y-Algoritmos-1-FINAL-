using UnityEngine;

public class Coin : CollectibleBase
{
    protected override CollectibleType GetCollectibleType() => CollectibleType.Coin;

    protected override void OnCollected(Collider2D player)
    {
        base.OnCollected(player);
        Debug.Log("Moneda recolectada!");
    }
}