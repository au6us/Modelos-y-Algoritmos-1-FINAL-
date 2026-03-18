using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerUpPickup : MonoBehaviour
{
    [Header("Tipo de Power-Up")]
    [SerializeField] private PowerUpType type;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // ¡Le avisa al Manager que lo active instantáneamente!
        PowerUpManager.Instance.ActivatePowerUp(type);

        // Destruye el objeto del mapa
        Destroy(gameObject);
    }
}