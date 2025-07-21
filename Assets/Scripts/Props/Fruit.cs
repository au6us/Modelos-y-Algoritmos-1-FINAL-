using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fruit : MonoBehaviour
{
    [Header("Fruit Settings")]
    [SerializeField] private int fruitPoints = 20;
    [SerializeField] private int healAmount = 1;
    [SerializeField] private AudioSource collectSFX;
    [SerializeField] private ParticleSystem collectVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Feedback
        collectSFX?.Play();
        if (collectVFX != null)
            Instantiate(collectVFX, transform.position, Quaternion.identity);

        // Eventos
        GameEventManager.FruitCollected(fruitPoints);

        // Curar al jugador
        var playerModel = other.GetComponent<PlayerModel>();
        if (playerModel != null)
            playerModel.Heal(healAmount);

        Destroy(gameObject);
    }
}
