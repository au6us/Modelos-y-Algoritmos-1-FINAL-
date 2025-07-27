// PowerUpManager.cs
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

        [Header("UI & Pickup")]
        public GameObject uiIcon;        // Icono UI (inactive por defecto)
        public AudioSource pickUpSource; // AudioSource al equipar

        [Header("Activación")]
        public AudioSource activationSource; // AudioSource al activar
        public float duration = 5f;          // Duración configurable
        public GameObject effectObject;      // Tu segunda cámara u otro efecto

        [HideInInspector] public bool equipped;
        [HideInInspector] public bool active;
    }

    [Header("Configura tus Power‑Ups aquí (orden = tecla 1,2,3...)")]
    [SerializeField] private List<Entry> entries;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        foreach (var e in entries)
        {
            if (e.uiIcon != null) e.uiIcon.SetActive(false);
            if (e.effectObject != null) e.effectObject.SetActive(false);
            e.equipped = false;
            e.active = false;
        }
    }

    private void Update()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                TryActivate(entries[i]);
        }
    }

    public void Equip(PowerUpType type)
    {
        var e = entries.Find(x => x.type == type);
        if (e == null || e.equipped) return;

        e.equipped = true;
        if (e.uiIcon != null) e.uiIcon.SetActive(true);
        if (e.pickUpSource != null) e.pickUpSource.Play();
    }

    private void TryActivate(Entry e)
    {
        if (!e.equipped || e.active) return;
        StartCoroutine(ActivateRoutine(e));
    }

    private IEnumerator ActivateRoutine(Entry e)
    {
        e.active = true;

        if (e.activationSource != null) e.activationSource.Play();
        if (e.effectObject != null) e.effectObject.SetActive(true);

        yield return new WaitForSeconds(e.duration);

        if (e.effectObject != null) e.effectObject.SetActive(false);
        e.active = false;
    }
}
