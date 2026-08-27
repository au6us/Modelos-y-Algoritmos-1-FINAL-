using System.Collections.Generic;
using UnityEngine;

// Contrato mínimo para cualquier objeto que el Pool pueda reciclar: solo necesita
// saber cómo dejarse en un estado "recién salido de fábrica" al ser reutilizado.
public interface IPoolable
{
    void ResetState();
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    // Un pool por prefab, sin importar de qué tipo concreto sea (EnemyBase, EnemyBullet, etc.)
    private readonly Dictionary<Component, Queue<Component>> pools = new Dictionary<Component, Queue<Component>>();

    // Recuerda de qué prefab vino cada instancia viva, para saber a qué cola devolverla en Release.
    private readonly Dictionary<Component, Component> instancePrefabs = new Dictionary<Component, Component>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public T Get<T>(T prefab, Vector3 pos, Quaternion rot) where T : Component, IPoolable
    {
        if (!pools.TryGetValue(prefab, out var queue))
        {
            queue = new Queue<Component>();
            pools[prefab] = queue;
        }

        Component instance;
        if (queue.Count > 0)
        {
            instance = queue.Dequeue();
            instance.transform.SetPositionAndRotation(pos, rot);
            instance.gameObject.SetActive(true);
            ((IPoolable)instance).ResetState();
        }
        else
        {
            instance = Instantiate(prefab, pos, rot);
            instance.gameObject.SetActive(true);
            instancePrefabs[instance] = prefab;
        }

        Debug.Log($"Get from pool: {instance.name}");
        return (T)instance;
    }

    public void Release(Component instance)
    {
        if (instance == null || instance.gameObject == null) return;

        Debug.Log($"Release to pool: {instance.name}");

        // Desactivar y mover fuera de vista
        instance.gameObject.SetActive(false);
        instance.transform.position = new Vector3(1000, 1000, 0);

        if (instancePrefabs.TryGetValue(instance, out var prefab) && pools.TryGetValue(prefab, out var queue))
        {
            queue.Enqueue(instance);
        }
        else
        {
            Debug.LogWarning($"PoolManager: No se encontró pool para el {instance.GetType().Name}");
            Destroy(instance.gameObject);
        }
    }
}
