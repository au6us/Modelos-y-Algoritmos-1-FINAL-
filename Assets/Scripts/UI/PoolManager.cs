using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private class Pool { public Queue<EnemyBase> queue = new(); public EnemyBase prefab; public Pool(EnemyBase p) { prefab = p; } }
    private Dictionary<EnemyBase, Pool> pools = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    public EnemyBase Get(EnemyBase prefab, Vector3 pos, Quaternion rot)
    {
        if (!pools.TryGetValue(prefab, out var pool))
        {
            pool = new Pool(prefab);
            pools[prefab] = pool;
        }

        EnemyBase inst;
        if (pool.queue.Count > 0)
        {
            inst = pool.queue.Dequeue();
            inst.gameObject.SetActive(true);
            inst.transform.SetPositionAndRotation(pos, rot);
        }
        else inst = Instantiate(prefab, pos, rot);

        // Reactivar todos los colliders
        foreach (var c in inst.GetComponentsInChildren<Collider2D>(true))
            c.enabled = true;

        return inst;
    }

    public void Release(EnemyBase inst)
    {
        inst.gameObject.SetActive(false);
        pools[inst.Prefab].queue.Enqueue(inst);
    }
}
