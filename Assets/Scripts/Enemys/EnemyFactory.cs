using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Hongo,
    Tronco,
    Murcielago
}

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }

    public EnemyType[] TiposDeEnemigos;

    public EnemyBase[] PrefabsDeEnemigos;

    // Diccionario interno para lookup rápido
    private Dictionary<EnemyType, EnemyBase> map;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        map = new Dictionary<EnemyType, EnemyBase>();
        int len = Mathf.Min(TiposDeEnemigos.Length, PrefabsDeEnemigos.Length);
        for (int i = 0; i < len; i++)
        {
            if (PrefabsDeEnemigos[i] != null)
                map[TiposDeEnemigos[i]] = PrefabsDeEnemigos[i];
        }
    }
    public EnemyBase Create(EnemyType type, Vector3 pos)
    {
        if (!map.ContainsKey(type))
        {
            Debug.LogError($"EnemyFactory: no existe prefab para tipo {type}");
            return null;

            //por si da error, no se pq no funca
        }

        var prefab = map[type];
        // El poolsito
        return PoolManager.Instance.Get(prefab, pos, Quaternion.identity);
    }
}
