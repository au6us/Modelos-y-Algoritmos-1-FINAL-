using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONSaveHandler : MonoBehaviour
{
    private string filePath;
    private string savePath;
    private const string DashKey = "DashUnlocked";

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
    }

    void Start()
    {
        savePath = Application.persistentDataPath + "/level_data.json";

        Debug.Log($"Ruta de datos del jugador: {filePath}");
        Debug.Log($"Ruta de datos de estrellas: {savePath}");
    }

    public void SaveData(int coins)
    {
        SaveData data = new SaveData { coins = coins };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    public int LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data.coins;
        }
        else
        {
            return 0; // Devuelve 0 si no existe un archivo
        }
    }

    public void DeleteData()
    {
        Debug.Log("Intentando eliminar datos...");

        if (File.Exists(filePath))
        {
            Debug.Log("Archivo de jugador encontrado. Eliminando...");
            File.Delete(filePath);
        }
        else
        {
            Debug.Log("No hay datos del jugador para eliminar.");
        }

        if (File.Exists(savePath))
        {
            Debug.Log("Archivo de estrellas encontrado. Eliminando...");
            File.Delete(savePath);
        }
        else
        {
            Debug.Log("No hay datos de estrellas para eliminar.");
        }

        if (PlayerPrefs.HasKey(DashKey))
        {
            Debug.Log("Eliminando estado del dash...");
            PlayerPrefs.DeleteKey(DashKey);
        }

        PlayerPrefs.Save();
        Debug.Log("Proceso de eliminación completado.");
    }

    public void SaveDashState(bool isUnlocked)
    {
        PlayerPrefs.SetInt(DashKey, isUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool LoadDashState()
    {
        return PlayerPrefs.GetInt(DashKey, 0) == 1;
    }

    public void SaveStars(int levelIndex, int stars)
    {
        Dictionary<int, int> levelStars = LoadAllStars();
        levelStars[levelIndex] = stars;
        string json = JsonUtility.ToJson(new LevelDataWrapper(levelStars));

        File.WriteAllText(savePath, json);

        // Debug para verificar que los datos se están guardando correctamente
        Debug.Log($"Guardando estrellas: Nivel {levelIndex}, Estrellas {stars}");
        Debug.Log("Datos de estrellas guardados en: " + savePath);
        Debug.Log("Contenido del JSON guardado: " + json);
    }

    public int LoadStars(int levelIndex)
    {
        Dictionary<int, int> levelStars = LoadAllStars();

        // Debug para ver qué datos se cargan
        Debug.Log($"Cargando estrellas del nivel {levelIndex}");
        Debug.Log("Datos de estrellas actuales: " + JsonUtility.ToJson(new LevelDataWrapper(levelStars)));

        return levelStars.ContainsKey(levelIndex) ? levelStars[levelIndex] : 0;
    }

    private Dictionary<int, int> LoadAllStars()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            LevelDataWrapper dataWrapper = JsonUtility.FromJson<LevelDataWrapper>(json);
            return dataWrapper.ToDictionary();
        }
        return new Dictionary<int, int>();
    }

    [System.Serializable]
    private class LevelDataWrapper
    {
        public List<int> levelIndices;
        public List<int> starCounts;

        public LevelDataWrapper(Dictionary<int, int> levelStars)
        {
            levelIndices = new List<int>(levelStars.Keys);
            starCounts = new List<int>(levelStars.Values);
        }

        public Dictionary<int, int> ToDictionary()
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int i = 0; i < levelIndices.Count; i++)
            {
                dictionary[levelIndices[i]] = starCounts[i];
            }
            return dictionary;
        }
    }
}
