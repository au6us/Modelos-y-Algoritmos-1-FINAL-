using UnityEngine;

public class Coin : CollectibleBase
{
    protected override CollectibleType GetCollectibleType() => CollectibleType.Coin;

    // Opcional: puedes sobreescribir para comportamiento adicional
    protected override void OnCollected()
    {
        base.OnCollected(); // Llama a la implementación base que dispara el evento

        // Comportamiento adicional específico de monedas
        Debug.Log("Moneda recolectada!");
    }
}