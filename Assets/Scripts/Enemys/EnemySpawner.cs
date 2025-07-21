using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyType type;
    public Transform spawnPoint;
    public float spawnDelay = 2f;
    public bool spawnOnAwake = true;

    private EnemyBase current;

    private void Awake()
    {
        if (spawnOnAwake) StartCoroutine(SpawnAfter(0f));
    }

    private IEnumerator SpawnAfter(float t)
    {
        yield return new WaitForSeconds(t);
        current = EnemyFactory.Create(type, spawnPoint.position);
        current.OnDie += OnEnemyDie;
    }

    private void OnEnemyDie(EnemyBase e)
    {
        e.OnDie -= OnEnemyDie;
        current = null;
        StartCoroutine(SpawnAfter(spawnDelay));
    }
}
