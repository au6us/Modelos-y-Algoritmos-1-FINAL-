using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class CollectibleBase : MonoBehaviour
{
    [Header("Collectible Settings")]
    [Tooltip("Cuántos puntos da al recoger")]
    [SerializeField] protected int pointValue = 1;

    [Header("Feedback")]
    [SerializeField] private AudioSource collectSFX;
    [SerializeField] private ParticleSystem collectVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Sonido y partículas
        if (collectSFX != null) collectSFX.Play();
        if (collectVFX != null)
            Instantiate(collectVFX, transform.position, Quaternion.identity);

        // Lógica concreta de cada collectible
        OnCollected();

        // Destruir el objeto (o pooléalo si usas pooling)
        Destroy(gameObject);
    }

    /// <summary>
    /// Cada subclase dispara su evento: CoinCollected o FruitCollected
    /// </summary>
    protected abstract void OnCollected();
}
