using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private EnemyFactory factory;
    [SerializeField] private string enemyType;
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private Transform spawnPoint;

    [Header("Activation Range")]
    [SerializeField] private float activationRange = 10f;
    [SerializeField] private LayerMask playerLayer;

    private bool hasSpawned = false;
    private EnemyBase currentEnemy;
    private bool playerInRange = false;
    private CircleCollider2D rangeCollider;

    private void Awake()
    {
        // Configurar el collider como trigger
        rangeCollider = GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = activationRange;
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
        if (!hasSpawned && playerInRange)
        {
            hasSpawned = true;
            SpawnEnemy();
        }
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

        // Intentar spawnear inmediatamente si el jugador está en el rango
        if (playerInRange)
        {
            TrySpawnEnemy();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmo fijo en color rojo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }

    // Actualizar el collider cuando se cambia el rango en el inspector
    private void OnValidate()
    {
        if (rangeCollider != null)
        {
            rangeCollider.radius = activationRange;
        }
    }
}