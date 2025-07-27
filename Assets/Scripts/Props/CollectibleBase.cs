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

        col.enabled = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false;

        OnCollected(other);

        StartCoroutine(PlayCollectionFeedback());
    }

    protected virtual IEnumerator PlayCollectionFeedback()
    {
        if (collectSFX != null && collectSFX.clip != null)
        {
            collectSFX.Play();
            yield return new WaitForSeconds(collectSFX.clip.length);
        }
        else
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    protected virtual void OnCollected(Collider2D player)
    {
        GameEventManager.TriggerCollectibleEvent(
            GetCollectibleType(),
            pointValue,
            transform.position
        );
    }
}