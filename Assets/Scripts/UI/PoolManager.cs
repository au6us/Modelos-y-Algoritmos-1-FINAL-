using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private class Pool
    {
        public Queue<EnemyBase> queue = new Queue<EnemyBase>();
        public EnemyBase prefab;
        public Pool(EnemyBase p) { prefab = p; }
    }

    private Dictionary<EnemyBase, Pool> pools = new Dictionary<EnemyBase, Pool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public EnemyBase Get(EnemyBase prefab, Vector3 pos, Quaternion rot)
    {
        if (!pools.TryGetValue(prefab, out var pool))
        {
            pool = new Pool(prefab);
            pools[prefab] = pool;
        }

        EnemyBase instance;
        if (pool.queue.Count > 0)
        {
            instance = pool.queue.Dequeue();
            instance.transform.SetPositionAndRotation(pos, rot);
            instance.gameObject.SetActive(true);
            instance.ResetState();
        }
        else
        {
            instance = Instantiate(prefab, pos, rot);
            instance.Prefab = prefab;
            instance.gameObject.SetActive(true);
        }

        Debug.Log($"Get from pool: {instance.name}");
        return instance;
    }

    public void Release(EnemyBase instance)
    {
        if (instance == null || instance.gameObject == null) return;

        Debug.Log($"Release to pool: {instance.name}");

        // Desactivar y mover fuera de vista
        instance.gameObject.SetActive(false);
        instance.transform.position = new Vector3(1000, 1000, 0);

        if (instance.Prefab != null && pools.ContainsKey(instance.Prefab))
        {
            pools[instance.Prefab].queue.Enqueue(instance);
        }
        else
        {
            Debug.LogWarning($"PoolManager: No se encontró pool para el {instance.GetType().Name}");
            Destroy(instance.gameObject);
        }
    }
}