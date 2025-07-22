using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyFactory factory;
    [SerializeField] private string enemyType;
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private Transform spawnPoint;

    private bool hasSpawned = false;
    private EnemyBase currentEnemy;

    private void Update()
    {
        if (!hasSpawned && PlayerIsNear())
        {
            hasSpawned = true;
            SpawnEnemy();
        }
    }

    private bool PlayerIsNear()
    {
        return currentEnemy == null &&
               PlayerController.Instance != null &&
               Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < 10f;
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

        // Intentar spawnear inmediatamente si el jugador está cerca
        if (PlayerIsNear())
        {
            SpawnEnemy();
        }
    }
}