using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameEventManager
{
    public static event Action<CollectibleEventData> OnCollectibleEvent;
    public static event Action<PlayerLifeEventData> OnPlayerLifeEvent;
    public static event Action<PlayerLivesEventData> OnPlayerLivesEvent;

    // Nueva funcionalidad para manejar persistencia UI
    private static bool isInitialized = false;

    // Inicializar el sistema
    public static void Initialize()
    {
        if (isInitialized) return;
        isInitialized = true;

        // Suscribir a eventos de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Crear objeto persistente para UI
        CreatePersistentUICanvas();
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Buscar jugador en la nueva escena
        PlayerController player = FindPlayer();
        if (player != null)
        {
            // Forzar actualizaci�n de UI
            PlayerModel model = player.GetComponent<PlayerModel>();
            if (model != null)
            {
                TriggerPlayerLifeEvent(model.Life, model.MaxLife);
            }
        }
    }

    private static PlayerController FindPlayer()
    {
        return UnityEngine.Object.FindObjectOfType<PlayerController>();
    }

    private static void CreatePersistentUICanvas()
    {
        // Buscar si ya existe un UIController
        UIController uiController = UnityEngine.Object.FindObjectOfType<UIController>();
        if (uiController != null)
        {
            // Hacer persistente si no es persistente a�n
            if (uiController.transform.parent == null)
            {
                UnityEngine.Object.DontDestroyOnLoad(uiController.gameObject);
            }
            return;
        }

        // Crear nuevo Canvas si no existe
        GameObject uiCanvas = new GameObject("PersistentUICanvas");
        Canvas canvas = uiCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        uiCanvas.AddComponent<UIController>();

        // Crear elementos UI m�nimos
        CreateUIElements(uiCanvas);

        UnityEngine.Object.DontDestroyOnLoad(uiCanvas);
    }

    private static void CreateUIElements(GameObject parent)
    {
        // Implementaci�n b�sica de creaci�n de UI
        // Deber�as reemplazar esto con tu prefab de UI real
        GameObject healthSliderObj = new GameObject("HealthSlider");
        healthSliderObj.transform.SetParent(parent.transform);

        GameObject healthTextObj = new GameObject("HealthText");
        healthTextObj.transform.SetParent(parent.transform);
        healthTextObj.AddComponent<TextMeshProUGUI>();

        Debug.Log("Created basic UI elements. Replace with your actual UI prefab.");
    }

    public static void TriggerCollectibleEvent(CollectibleType type, int points, Vector3 position)
    {
        OnCollectibleEvent?.Invoke(new CollectibleEventData
        {
            Type = type,
            Points = points,
            Position = position
        });
    }

    public static void TriggerPlayerLifeEvent(int currentLife, int maxLife)
    {
        OnPlayerLifeEvent?.Invoke(new PlayerLifeEventData
        {
            CurrentLife = currentLife,
            MaxLife = maxLife
        });
    }

    // "Lives" (plural) = vidas extra / continues del GameplayManager (arranca en 3).
    // No confundir con "Life" (singular) de arriba, que es la salud/HP del frog.
    public static void TriggerPlayerLivesEvent(int currentLives, int maxLives)
    {
        OnPlayerLivesEvent?.Invoke(new PlayerLivesEventData
        {
            CurrentLives = currentLives,
            MaxLives = maxLives
        });
    }
}

public enum CollectibleType
{
    EnemyKilled,
    Coin,
    Fruit
}

public struct CollectibleEventData
{
    public CollectibleType Type;
    public int Points;
    public Vector3 Position;
}

public struct PlayerLifeEventData
{
    public int CurrentLife;
    public int MaxLife;
}

public struct PlayerLivesEventData
{
    public int CurrentLives;
    public int MaxLives;
}