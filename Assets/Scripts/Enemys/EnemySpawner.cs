using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyType type;
    public Transform spawnPoint;
    public float spawnDelay = 2f;
    public bool spawnOnAwake = false;
    [Tooltip("Si true, spawnea cuando el jugador entre en proximityRadius")]
    public bool spawnOnProximity = true;
    public float proximityRadius = 5f;

    // Referencia al jugador (puedes arrastrar el Player.transform en el Inspector)
    [SerializeField] private Transform playerTransform;

    private EnemyBase current;
    private bool hasSpawned = false;

    private void Awake()
    {
        if (spawnOnAwake)
            StartCoroutine(SpawnAfter(0f));
    }

    private void Update()
    {
        if (!hasSpawned && spawnOnProximity && playerTransform != null)
        {
            float d = Vector3.Distance(playerTransform.position, spawnPoint.position);
            if (d <= proximityRadius)
            {
                hasSpawned = true;
                StartCoroutine(SpawnAfter(0f));
            }
        }
    }

    private IEnumerator SpawnAfter(float t)
    {
        yield return new WaitForSeconds(t);

        current = EnemyFactory.Instance.Create(type, spawnPoint.position);
        current.OnDie += OnEnemyDie;
    }

    private void OnEnemyDie(EnemyBase e)
    {
        e.OnDie -= OnEnemyDie;
        current = null;
        // Para volver a spawnear otra vez al salir y entrar en rango,
        // resetea hasSpawned a false aquí si quieres reactivar spawnOnProximity,
        // o simplemente respawnea tras delay:
        StartCoroutine(SpawnAfter(spawnDelay));
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnOnProximity && spawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spawnPoint.position, proximityRadius);
        }
    }
}
