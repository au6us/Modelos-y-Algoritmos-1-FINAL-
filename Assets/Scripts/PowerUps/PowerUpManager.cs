using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    [System.Serializable]
    public class Entry
    {
        public PowerUpType type;

        [Header("UI & Pickup")]
        public Image uiIcon;
        public AudioSource pickUpSource;

        [Header("Activación (Start)")]
        public AudioSource activationSource;
        public float duration = 5f;
        public GameObject effectObject;        // La burbuja visual
        public ParticleSystem activeParticles; // Partículas en loop

        [Header("Finalización (End/Break)")]
        public AudioSource breakSource;       // Sonido al romperse
        public ParticleSystem breakParticles; // Explosión final

        [Header("Boost Settings")]
        public float moveSpeedMultiplier = 1.5f;
        public int extraJumps = 1;
        public float dashCooldownMultiplier = 0.5f;

        [HideInInspector] public bool equipped;
        [HideInInspector] public bool active;
        [HideInInspector] public Color originalColor;
    }

    [Header("Asigná tus Power‑Ups")]
    [SerializeField] private List<Entry> entries;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        foreach (var e in entries)
        {
            if (e.uiIcon != null)
            {
                e.originalColor = e.uiIcon.color;
                e.uiIcon.color = e.originalColor * 0.2f;
            }
            if (e.effectObject != null) e.effectObject.SetActive(false);
            if (e.activeParticles != null) e.activeParticles.Stop();

            e.equipped = e.active = false;
        }
    }

    private void Update()
    {
        // Seguimos escuchando teclas, PERO como el escudo nunca se marca como "equipped",
        // apretar el 3 no va a hacer nada. (Lo cual está perfecto).
        for (int i = 0; i < entries.Count; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                TryActivate(entries[i]);
    }

    public void Equip(PowerUpType type)
    {
        var e = entries.Find(x => x.type == type);
        if (e == null) return;

        // --- CAMBIO CLAVE AQUI ---
        // Si es el ESCUDO, se activa automáticamente al tocarlo.
        if (e.type == PowerUpType.Shield)
        {
            // Si ya está activo, no hacemos nada (o podrías reiniciar el tiempo si quisieras)
            if (e.active) return;

            e.pickUpSource?.Play(); // Sonido de agarrar
            StartCoroutine(ActivateRoutine(e)); // ¡Se activa solo!
        }
        else
        {
            // Lógica vieja para los otros poderes (Boost, Zoom) que SI requieren tecla
            if (e.equipped) return;

            e.equipped = true;
            if (e.uiIcon != null) e.uiIcon.color = Color.white;
            e.pickUpSource?.Play();
        }
    }

    private void TryActivate(Entry e)
    {
        // Esta función ahora solo sirve para los poderes manuales.
        if (!e.equipped || e.active) return;
        StartCoroutine(ActivateRoutine(e));
    }

    private IEnumerator ActivateRoutine(Entry e)
    {
        e.active = true;
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
            model.GrantShield(); // Dar escudo

            // Esperar mientras dure el tiempo Y siga teniendo el escudo intacto
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

            // Si salió del while y TODAVÍA tiene escudo (o sea, se acabó el tiempo),
            // lo rompemos manualmente para gatillar la invencibilidad.
            if (model.HasShield)
            {
                model.BreakShield();
            }
        }
        else
        {
            yield return new WaitForSeconds(e.duration);
        }

        // 3. Feedback de FINAL
        e.breakSource?.Play();
        if (e.breakParticles != null) e.breakParticles.Play();

        // 4. Limpieza
        if (e.activeParticles != null) e.activeParticles.Stop();
        if (e.effectObject != null) e.effectObject.SetActive(false);

        e.active = false;

        // Solo des-equipamos si era un item equipable. 
        // Como el escudo nunca se marcó como "equipped", esto no afecta, pero limpia prolijo.
        e.equipped = false;
        if (e.uiIcon != null) e.uiIcon.color = e.originalColor * 0.2f;
    }

    private void ApplyBoost(Entry e, PlayerModel model)
    {
        e.uiIcon.GetComponent<BoostHolder>()?.StoreOriginals(
            model.MoveSpeed, model.MaxJumps, model.DashCooldown
        );
        model.MoveSpeed *= e.moveSpeedMultiplier;
        model.MaxJumps += e.extraJumps;
        model.DashCooldown *= e.dashCooldownMultiplier;
    }

    private void RemoveBoost(Entry e, PlayerModel model)
    {
        var holder = e.uiIcon.GetComponent<BoostHolder>();
        if (holder != null)
        {
            model.MoveSpeed = holder.OriginalMoveSpeed;
            model.MaxJumps = holder.OriginalMaxJumps;
            model.DashCooldown = holder.OriginalDashCooldown;
        }
    }
}