// PowerUpManager.cs
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

        [Header("Activación")]
        public AudioSource activationSource;
        public float duration = 5f;
        public GameObject effectObject;    // cámara extra, etc.

        [Header("Boost Settings (solo para Boost)")]
        public float moveSpeedMultiplier = 1.5f;
        public int extraJumps = 1;
        public float dashCooldownMultiplier = 0.5f;
        public ParticleSystem boostParticles;

        [HideInInspector] public bool equipped;
        [HideInInspector] public bool active;
        [HideInInspector] public Color originalColor;
    }

    [Header("Asigná tus Power‑Ups (1 = Alpha1, 2 = Alpha2, ...)")]
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
            if (e.boostParticles != null) e.boostParticles.Stop();
            e.equipped = e.active = false;
        }
    }

    private void Update()
    {
        for (int i = 0; i < entries.Count; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                TryActivate(entries[i]);
    }

    public void Equip(PowerUpType type)
    {
        var e = entries.Find(x => x.type == type);
        if (e == null || e.equipped) return;

        e.equipped = true;
        if (e.uiIcon != null) e.uiIcon.color = Color.white;
        e.pickUpSource?.Play();
    }

    private void TryActivate(Entry e)
    {
        if (!e.equipped || e.active) return;
        StartCoroutine(ActivateRoutine(e));
    }

    private IEnumerator ActivateRoutine(Entry e)
    {
        e.active = true;

        // feedback de activación
        e.activationSource?.Play();
        if (e.effectObject != null) e.effectObject.SetActive(true);
        if (e.boostParticles != null) e.boostParticles.Play();

        if (e.type == PowerUpType.Boost)
            ApplyBoost(e);

        yield return new WaitForSeconds(e.duration);

        // revertir
        if (e.type == PowerUpType.Boost)
            RemoveBoost(e);

        if (e.boostParticles != null) e.boostParticles.Stop();
        if (e.effectObject != null) e.effectObject.SetActive(false);

        e.active = false;
        e.equipped = false;
        if (e.uiIcon != null) e.uiIcon.color = e.originalColor * 0.2f;
    }

    private void ApplyBoost(Entry e)
    {
        var model = PlayerController.Instance.GetComponent<PlayerModel>();

        // guardo originales en el BoostHolder del icono
        e.uiIcon.GetComponent<BoostHolder>()?.StoreOriginals(
            model.MoveSpeed,
            model.MaxJumps,
            model.DashCooldown
        );

        model.MoveSpeed *= e.moveSpeedMultiplier;
        model.MaxJumps += e.extraJumps;
        model.DashCooldown *= e.dashCooldownMultiplier;
    }

    private void RemoveBoost(Entry e)
    {
        var model = PlayerController.Instance.GetComponent<PlayerModel>();
        var holder = e.uiIcon.GetComponent<BoostHolder>();
        if (holder != null)
        {
            model.MoveSpeed = holder.OriginalMoveSpeed;
            model.MaxJumps = holder.OriginalMaxJumps;
            model.DashCooldown = holder.OriginalDashCooldown;
        }
    }
}
