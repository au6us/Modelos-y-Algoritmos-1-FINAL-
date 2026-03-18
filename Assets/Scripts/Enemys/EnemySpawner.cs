using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] // ¡Ahora pide un BoxCollider!
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private EnemyFactory factory;
    [SerializeField] private string enemyType;
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private Transform spawnPoint;

    [Header("Activation Area (Cuadrado)")]
    [SerializeField] private Vector2 activationArea = new Vector2(10f, 5f); // Ancho (X) y Alto (Y)
    [SerializeField] private Vector2 areaOffset = Vector2.zero; // Para mover el cuadrado de lugar
    [SerializeField] private LayerMask playerLayer;

    [Header("Feedback (Pre-Spawn)")]
    [SerializeField] private AudioSource preSpawnAudio;
    [SerializeField] private ParticleSystem preSpawnParticles;
    [SerializeField] private float delayBetweenEffects = 0.5f;

    private bool hasSpawned = false;
    private EnemyBase currentEnemy;
    private bool playerInRange = false;
    private BoxCollider2D rangeCollider;

    private void Awake()
    {
        // Configurar el collider como trigger
        rangeCollider = GetComponent<BoxCollider2D>();
        rangeCollider.isTrigger = true;
        rangeCollider.size = activationArea;
        rangeCollider.offset = areaOffset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si es el jugador
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            playerInRange = true;
            TrySpawnEnemy();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Verificar si es el jugador
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            playerInRange = false;
        }
    }

    private void TrySpawnEnemy()
    {
        // Si no spawneó todavía y el jugador está cerca
        if (!hasSpawned && playerInRange)
        {
            hasSpawned = true;
            StartCoroutine(SpawnSequenceRoutine());
        }
    }

    private IEnumerator SpawnSequenceRoutine()
    {
        // 1ER AVISO
        if (preSpawnAudio != null) preSpawnAudio.Play();
        if (preSpawnParticles != null) preSpawnParticles.Play();

        yield return new WaitForSeconds(delayBetweenEffects);

        // 2DO AVISO 
        if (preSpawnAudio != null) preSpawnAudio.Play();
        if (preSpawnParticles != null) preSpawnParticles.Play();

        yield return new WaitForSeconds(delayBetweenEffects);

        // ¡APARECE EL ENEMIGO!
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        currentEnemy = factory.CreateEnemy(enemyType, spawnPoint.position);
        if (currentEnemy != null)
        {
            currentEnemy.OriginSpawner = this;
            Debug.Log($"Spawned enemy: {currentEnemy.name}");
        }
    }

    public void ScheduleRespawn()
    {
        Debug.Log("Scheduling respawn...");
        Invoke(nameof(ResetAndSpawn), respawnTime);
    }

    private void ResetAndSpawn()
    {
        Debug.Log("Respawning enemy...");
        hasSpawned = false;
        currentEnemy = null;

        // Intentar spawnear inmediatamente si el jugador sigue en el rango
        if (playerInRange)
        {
            TrySpawnEnemy();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmo fijo en color rojo para el CUADRADO
        Gizmos.color = Color.red;
        Vector3 center = transform.position + (Vector3)areaOffset;
        Gizmos.DrawWireCube(center, activationArea);
    }

    // Actualizar el collider cuando se cambia el tamaño en el inspector
    private void OnValidate()
    {
        if (rangeCollider == null)
            rangeCollider = GetComponent<BoxCollider2D>();

        if (rangeCollider != null)
        {
            rangeCollider.size = activationArea;
            rangeCollider.offset = areaOffset;
        }
    }
}