using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public string id;
        public EnemyBase prefab;
    }

    [Header("Lista de enemigos disponibles")]
    [SerializeField] private List<EnemyEntry> enemies;

    private Dictionary<string, EnemyBase> enemyDict;

    private void Awake()
    {
        enemyDict = new Dictionary<string, EnemyBase>();
        foreach (var entry in enemies)
        {
            if (!enemyDict.ContainsKey(entry.id) && entry.prefab != null)
                enemyDict.Add(entry.id, entry.prefab);
        }
    }

    public EnemyBase CreateEnemy(string id, Vector3 position)
    {
        if (!enemyDict.TryGetValue(id, out var prefab))
        {
            Debug.LogError($"[EnemyFactory] Prefab no encontrado con ID: {id}");
            return null;
        }

        var enemy = PoolManager.Instance.Get(prefab, position, Quaternion.identity);
        return enemy;
    }
}