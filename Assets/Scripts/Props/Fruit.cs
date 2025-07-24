using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fruit : CollectibleBase
{
    [Header("Fruit Settings")]
    [SerializeField] private int healAmount = 1;
    protected override CollectibleType GetCollectibleType() => CollectibleType.Fruit;

    protected override void OnCollected()
    {
        base.OnCollected(); // Evento de colección (puntos)

        // Cura al player
        var playerModel = PlayerController.Instance?.GetComponent<PlayerModel>();
        if (playerModel != null)
            playerModel.Heal(healAmount);
    }
}