using UnityEngine;

public class Coin : CollectibleBase
{
    protected override CollectibleType GetCollectibleType() => CollectibleType.Coin;

    protected override void OnCollected(Collider2D player)
    {
        base.OnCollected(player); // Pasar el par�metro al m�todo base

        Debug.Log("Moneda recolectada!");
    }
}