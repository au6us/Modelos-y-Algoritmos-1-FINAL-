using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Fruit : MonoBehaviour
{
    [Header("Fruit Settings")]
    [SerializeField] private int fruitPoints = 20;
    [SerializeField] private int healAmount = 1;
    [SerializeField] private AudioSource collectSFX;
    [SerializeField] private ParticleSystem collectVFX;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || collected) return;
        collected = true;

        StartCoroutine(CollectRoutine(other));
    }

    private IEnumerator CollectRoutine(Collider2D player)
    {
        // Desactivar componentes
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        // Reproducir sonido
        if (collectSFX != null)
        {
            collectSFX.Play();
            // Esperar a que termine el sonido
            yield return new WaitForSeconds(collectSFX.clip.length);
        }

        // Reproducir VFX
        if (collectVFX != null)
        {
            var vfx = Instantiate(collectVFX, transform.position, Quaternion.identity);
            Destroy(vfx.gameObject, vfx.main.duration);
        }

        // Curar al jugador
        var playerModel = player.GetComponent<PlayerModel>();
        if (playerModel != null)
            playerModel.Heal(healAmount);

        // Disparar evento
        GameEventManager.TriggerCollectibleEvent(
            CollectibleType.Fruit,
            fruitPoints,
            transform.position
        );

        // Destruir finalmente
        Destroy(gameObject);
    }
}