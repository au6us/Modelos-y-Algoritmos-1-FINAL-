using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contrato de Strategy: cada tipo de power-up sabe aplicarse, esperar/monitorear
// su propia duración, y devolver al jugador a la normalidad. PowerUpManager no
// conoce estos detalles, solo pide "activate" y espera a que termine.
public interface IPowerUpStrategy
{
    IEnumerator Activate(PlayerModel model, PowerUpManager.Entry entry);
}

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    [System.Serializable]
    public class Entry
    {
        public PowerUpType type;

        [Header("Sonidos")]
        public AudioSource pickUpSource;
        public AudioSource activationSource;
        public AudioSource breakSource;

        [Header("Visuales")]
        public GameObject effectObject;        // Burbuja, estela, etc.
        public ParticleSystem activeParticles; // Partículas en loop
        public ParticleSystem breakParticles;  // Explosión al terminar

        [Header("Configuración")]
        public float duration = 5f;

        [Header("Boost Settings")]
        public float moveSpeedMultiplier = 1.5f;
        public int extraJumps = 1;
        public float dashCooldownMultiplier = 0.5f;

        [HideInInspector] public bool active;

        // --- NUEVO: Guardamos los stats originales acá adentro, sin depender de la UI ---
        [HideInInspector] public float originalMoveSpeed;
        [HideInInspector] public int originalMaxJumps;
        [HideInInspector] public float originalDashCooldown;
    }

    [Header("Asigná tus Power-Ups")]
    [SerializeField] private List<Entry> entries;

    // Una estrategia concreta por tipo. Agregar un power-up nuevo es: sumar el valor
    // al enum, escribir una clase que implemente IPowerUpStrategy, y una línea acá —
    // no hay que tocar ActivateRoutine.
    private Dictionary<PowerUpType, IPowerUpStrategy> strategies;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        strategies = new Dictionary<PowerUpType, IPowerUpStrategy>
        {
            { PowerUpType.Boost, new BoostPowerUpStrategy() },
            { PowerUpType.Shield, new ShieldPowerUpStrategy() },
            { PowerUpType.CameraZoom, new CameraZoomPowerUpStrategy() },
        };

        foreach (var e in entries)
        {
            if (e.effectObject != null) e.effectObject.SetActive(false);
            if (e.activeParticles != null) e.activeParticles.Stop();
            e.active = false;
        }
    }

    // ¡CHAU AL METODO UPDATE! Ya no escuchamos el teclado.

    public void ActivatePowerUp(PowerUpType type)
    {
        var e = entries.Find(x => x.type == type);
        if (e == null) return;

        // Si el poder ya está activo, no hacemos nada (evita que se superpongan bugs)
        if (e.active) return;

        e.pickUpSource?.Play(); // Sonido al tocarlo
        StartCoroutine(ActivateRoutine(e)); // ¡Se activa solo al instante!
    }

    private IEnumerator ActivateRoutine(Entry e)
    {
        e.active = true;

        // Asumo que tu PlayerController tiene el PlayerModel, ajustá esto si lo buscás distinto
        var model = PlayerController.Instance.GetComponent<PlayerModel>();

        // 1. Feedback de INICIO
        e.activationSource?.Play();
        if (e.effectObject != null) e.effectObject.SetActive(true);
        if (e.activeParticles != null) e.activeParticles.Play();

        // 2. Lógica según TIPO — delegada a la Strategy correspondiente
        if (strategies.TryGetValue(e.type, out var strategy))
            yield return strategy.Activate(model, e);

        // 3. Feedback de FINAL
        e.breakSource?.Play();
        if (e.breakParticles != null) e.breakParticles.Play();

        // 4. Limpieza
        if (e.activeParticles != null) e.activeParticles.Stop();
        if (e.effectObject != null) e.effectObject.SetActive(false);

        e.active = false;
    }
}