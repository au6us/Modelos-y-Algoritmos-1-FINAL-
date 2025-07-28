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
        public Image uiIcon;           // Tu Image en la UI
        public AudioSource pickUpSource;

        [Header("Activación")]
        public AudioSource activationSource;
        public float duration = 5f;
        public GameObject effectObject;

        [HideInInspector] public bool equipped;
        [HideInInspector] public bool active;
        [HideInInspector] public Color originalColor;
    }

    [Header("Configura Power‑Ups aca (orden = tecla 1,2,3...) se charla")]
    [SerializeField] private List<Entry> entries;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // Guardar color original y oscurecer items
        foreach (var e in entries)
        {
            if (e.uiIcon != null)
            {
                e.originalColor = e.uiIcon.color;
                e.uiIcon.color = e.originalColor * 0.5f; //Se baja el Alpha a la mitad;
            }
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

    /// <summary>Equipar al recoger.</summary>
    public void Equip(PowerUpType type)
    {
        var e = entries.Find(x => x.type == type);
        if (e == null || e.equipped) return;

        e.equipped = true;
        if (e.uiIcon != null)
        {
            e.uiIcon.color = Color.white;
        }
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
        e.activationSource?.Play();
        if (e.effectObject != null)
            e.effectObject.SetActive(true);

        yield return new WaitForSeconds(e.duration);

        // Desactivar efecto
        if (e.effectObject != null)
            e.effectObject.SetActive(false);

        // Consumir el power‑up y restaurar color UI
        e.active = false;
        e.equipped = false;
        if (e.uiIcon != null)
        {
            e.uiIcon.color = e.originalColor * 0.5f;
        }
    }
}
