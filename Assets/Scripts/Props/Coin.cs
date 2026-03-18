using UnityEngine;
using System; // Necesario para Action

public class Coin : CollectibleBase
{
    // --- NUEVO: El evento del Patrón Observer ---
    public static event Action OnCoinCollectedEvent;

    protected override CollectibleType GetCollectibleType() => CollectibleType.Coin;

    protected override void OnCollected(Collider2D player)
    {
        base.OnCollected(player);
        Debug.Log("Moneda recolectada!");

        // Disparamos el evento para avisarle a la UI
        OnCoinCollectedEvent?.Invoke();
    }
}