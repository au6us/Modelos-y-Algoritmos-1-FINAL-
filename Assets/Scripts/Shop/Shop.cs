using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [SerializeField] EventTrigger backMenuButton;

    [SerializeField] ItemUI itemPrefab;
    [SerializeField] Transform shopParent;

    [SerializeField] ItemDTO[] myItems = new ItemDTO[0];
    [SerializeField] JSONSaveHandler saveHandler;

    private int currentCoins;
    void Start()
    {
        // Busca automáticamente JSONSaveHandler si no está asignado
        if (saveHandler == null)
        {
            saveHandler = FindObjectOfType<JSONSaveHandler>();
            if (saveHandler == null)
            {
                Debug.LogError("JSONSaveHandler no encontrado en la escena. Asegúrate de que existe.");
                return;
            }
        }

        // Carga las monedas
        currentCoins = saveHandler.LoadData();
        Debug.Log("Monedas actuales: " + currentCoins);

        for (int i = 0; i < myItems.Length; i++)
        {
            var newItem = Instantiate(itemPrefab, shopParent);
            newItem.InitializeButton(myItems[i]);
            newItem.onItemClicked += OnSellItem;
        }
    }

    void OnSellItem(ItemDTO itemToSell)
    {
        if (currentCoins >= itemToSell.itemCost)
        {
            currentCoins -= itemToSell.itemCost;
            saveHandler.SaveData(currentCoins);
            Debug.Log("Compraste " + itemToSell.itemName);
            Debug.Log("Monedas restantes: " + currentCoins);

            if (itemToSell.itemName == "Dash")
            {
                saveHandler.SaveDashState(true); // Guardar estado de desbloqueo del dash
                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    player.UnlockDash(); // Actualizar el estado del jugador en la escena actual
                }
            }
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar " + itemToSell.itemName);
        }
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }
}
