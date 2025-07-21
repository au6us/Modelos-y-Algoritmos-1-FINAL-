using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private class Pool
    {
        public readonly Queue<EnemyBase> queue = new Queue<EnemyBase>();
        public readonly EnemyBase prefab;
        public Pool(EnemyBase p) { prefab = p; }
    }

    private Dictionary<EnemyBase, Pool> pools = new Dictionary<EnemyBase, Pool>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
            instance.gameObject.SetActive(true);
            instance.transform.SetPositionAndRotation(pos, rot);
        }
        else
        {
            instance = Instantiate(prefab, pos, rot);
        }

        instance.Prefab = prefab;
        return instance;
    }

    public void Release(EnemyBase instance)
    {
        instance.gameObject.SetActive(false);
        pools[instance.Prefab].queue.Enqueue(instance);
    }
}
