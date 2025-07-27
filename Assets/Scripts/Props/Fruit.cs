using UnityEngine;

public class Fruit : CollectibleBase
{
    [Header("Fruit Settings")]
    [SerializeField] private int healAmount = 1;

    protected override CollectibleType GetCollectibleType()
    {
        return CollectibleType.Fruit;
    }

    protected override void OnCollected(Collider2D player)
    {
        base.OnCollected(player);

        var playerModel = player.GetComponent<PlayerModel>();
        if (playerModel != null)
            playerModel.Heal(healAmount);
    }
}