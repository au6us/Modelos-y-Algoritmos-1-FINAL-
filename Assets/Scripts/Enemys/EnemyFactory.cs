using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Hongo, Tronco, Murcielago }

public static class EnemyFactory
{
    private static readonly Dictionary<EnemyType, EnemyBase> prefabMap;

    static EnemyFactory()
    {
        prefabMap = new Dictionary<EnemyType, EnemyBase>
        {
            { EnemyType.Hongo, Resources.Load<EnemyBase>("Enemies/Hongo") },
            { EnemyType.Tronco, Resources.Load<EnemyBase>("Enemies/Tronco") },
            { EnemyType.Murcielago, Resources.Load<EnemyBase>("Enemies/Murcielago") },
        };
    }

    public static EnemyBase Create(EnemyType type, Vector3 pos)
    {
        var prefab = prefabMap[type];
        return PoolManager.Instance.Get(prefab, pos, Quaternion.identity);
    }
}
