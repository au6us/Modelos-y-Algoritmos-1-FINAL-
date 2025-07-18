using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsLoadListener , IUnityAdsInitializationListener, IUnityAdsShowListener
{
    [SerializeField] string gameID = "5728142";

    [SerializeField] string adID = "Rewarded_Android";

    StaminaSystem staminaSystem;

    public int staminaCantityReward = 1;

    public void OnInitializationComplete()
    {
        Advertisement.Load(adID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {

    }

    private void Start()
    {
        Advertisement.Initialize(gameID, true, this);
        staminaSystem = FindObjectOfType<StaminaSystem>();
    }

    public void ShowAD()
    {
        if (!Advertisement.isInitialized) return;

        Advertisement.Show(adID, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {

        if(showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("¡Obtuviste una carga de Stamina!");
            if(staminaSystem != null)
            {
                staminaSystem.RechargeStamina(staminaCantityReward);
            }
        }
        else
        {
            Debug.Log("No obtuviste tu carga de Stamina :(");
        }



    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
    }

    //Estos 2 son para solucionar ese error falso del listener, es para que no moleste boe
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    }
}
