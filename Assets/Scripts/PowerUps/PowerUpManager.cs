using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

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

        // 2. Lógica según TIPO
        if (e.type == PowerUpType.Boost)
        {
            ApplyBoost(e, model);
            yield return new WaitForSeconds(e.duration);
            RemoveBoost(e, model);
        }
        else if (e.type == PowerUpType.Shield)
        {
            model.GrantShield();

            float timer = e.duration;
            while (timer > 0 && model.HasShield)
            {
                timer -= Time.deltaTime;

                // Parpadeo final (últimos 2 seg)
                if (timer < 2f && e.effectObject != null)
                {
                    e.effectObject.SetActive((Time.time * 10) % 2 > 1);
                }

                yield return null;
            }

            // Si se acabó el tiempo y todavía tiene escudo, lo rompemos
            if (model.HasShield)
            {
                model.BreakShield();
            }
        }
        else
        {
            // Para el CameraZoom u otros poderes que agregues
            yield return new WaitForSeconds(e.duration);
        }

        // 3. Feedback de FINAL
        e.breakSource?.Play();
        if (e.breakParticles != null) e.breakParticles.Play();

        // 4. Limpieza
        if (e.activeParticles != null) e.activeParticles.Stop();
        if (e.effectObject != null) e.effectObject.SetActive(false);

        e.active = false;
    }

    private void ApplyBoost(Entry e, PlayerModel model)
    {
        // Guardamos las estadísticas reales antes de modificarlas
        e.originalMoveSpeed = model.MoveSpeed;
        e.originalMaxJumps = model.MaxJumps;
        e.originalDashCooldown = model.DashCooldown;

        // Aplicamos la mejora
        model.MoveSpeed *= e.moveSpeedMultiplier;
        model.MaxJumps += e.extraJumps;
        model.DashCooldown *= e.dashCooldownMultiplier;
    }

    private void RemoveBoost(Entry e, PlayerModel model)
    {
        // Devolvemos a la normalidad usando los datos que guardamos
        model.MoveSpeed = e.originalMoveSpeed;
        model.MaxJumps = e.originalMaxJumps;
        model.DashCooldown = e.originalDashCooldown;
    }
}