using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public abstract class CollectibleBase : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] protected int pointValue = 1;

    [Header("Feedback")]
    [SerializeField] protected AudioSource collectSFX;
    [SerializeField] protected ParticleSystem collectVFX;

    private bool collected = false;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected abstract CollectibleType GetCollectibleType();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || collected) return;
        collected = true;

        // Desactivar inmediatamente los componentes visibles/interactivos
        col.enabled = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false;

        // Ejecutar lógica de recolección (incluyendo actualización de UI)
        OnCollected(other);

        // Manejar feedback y destrucción
        StartCoroutine(PlayCollectionFeedback());
    }

    protected virtual IEnumerator PlayCollectionFeedback()
    {
        // Reproducir sonido si existe
        if (collectSFX != null && collectSFX.clip != null)
        {
            collectSFX.Play();
            yield return new WaitForSeconds(collectSFX.clip.length);
        }
        else
        {
            yield return null;
        }

        // Destruir el objeto después del feedback
        Destroy(gameObject);
    }

    protected virtual void OnCollected(Collider2D player)
    {
        // Disparar evento inmediatamente para actualizar UI
        GameEventManager.TriggerCollectibleEvent(
            GetCollectibleType(),
            pointValue,
            transform.position
        );
    }
}