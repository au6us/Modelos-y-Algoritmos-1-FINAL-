using System.Collections.Generic;
using UnityEngine;

// Identificador de cada pantalla que el juego puede mostrar. Una misma escena
// solo registra las que le corresponden (el menú registra MainMenu/Credits,
// una escena de gameplay registra Pause/Win/GameOver) — no hace falta que existan todas a la vez.
//
// IMPORTANTE: los valores están fijados a mano a propósito, y todo agregado nuevo
// (como GameOver acá) va SIEMPRE al final. Unity guarda cada entrada de la lista
// "Screens" como el número, no el nombre — si algún día se borra o reordena un
// valor del medio, todo lo que ya estaba configurado en el
// Inspector se corre de lugar en silencio (nos pasó justo con "GameOver"). Al
// numerarlos explícitamente, un valor nuevo solo se agrega al final, nunca pisa
// a los demás.
public enum ScreenId { MainMenu = 0, Credits = 1, Pause = 2, Win = 3, GameOver = 4 }

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [System.Serializable]
    public class ScreenEntry
    {
        public ScreenId id;
        public GameObject panel;
    }

    [Header("Asigná los paneles de ESTA escena")]
    [SerializeField] private List<ScreenEntry> screens;

    // El HUD (vida, estamina, monedas, puntuación) NO es una "pantalla" más: convive con
    // el gameplay, no compite en exclusividad con Pause/Win. Por eso no es un ScreenId,
    // pero el ScreenManager sigue siendo el único responsable de coordinarlo: se oculta
    // mientras haya una pantalla arriba, y vuelve solo cuando se cierra. En la escena
    // de Menú simplemente se deja sin asignar (queda null) y estas líneas no hacen nada.
    [Header("HUD de gameplay (dejar vacío en la escena de Menú)")]
    [SerializeField] private GameObject hud;

    private Dictionary<ScreenId, GameObject> screenLookup;
    private ScreenId? activeScreen;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        screenLookup = new Dictionary<ScreenId, GameObject>();
        foreach (var entry in screens)
        {
            if (entry.panel == null) continue;
            if (!screenLookup.ContainsKey(entry.id)) screenLookup.Add(entry.id, entry.panel);
            entry.panel.SetActive(false);
        }

        if (hud != null) hud.SetActive(true);
    }

    // Muestra una pantalla y oculta la que estuviera activa (nunca hay dos a la vez).
    public void Show(ScreenId id)
    {
        if (!screenLookup.TryGetValue(id, out var panel)) return;

        if (activeScreen.HasValue && screenLookup.TryGetValue(activeScreen.Value, out var current))
            current.SetActive(false);

        panel.SetActive(true);
        activeScreen = id;

        if (hud != null) hud.SetActive(false);
    }

    public void Hide(ScreenId id)
    {
        if (!screenLookup.TryGetValue(id, out var panel)) return;
        panel.SetActive(false);

        if (activeScreen == id)
        {
            activeScreen = null;
            if (hud != null) hud.SetActive(true);
        }
    }

    public void HideAll()
    {
        foreach (var panel in screenLookup.Values) panel.SetActive(false);
        activeScreen = null;
        if (hud != null) hud.SetActive(true);
    }

    public bool IsShowing(ScreenId id) => activeScreen == id;
}
