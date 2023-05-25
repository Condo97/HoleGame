using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAdManager: MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    public static RewardedAdManager instance;

    [Header(" Settings ")]
    [SerializeField] private string androidGameID;
    [SerializeField] private string iosGameID;
    [SerializeField] private string androidRewardedID;
    [SerializeField] private string iosRewardedID;
    [SerializeField] private bool testMode;
    private string gameID = null;
    private string rewardedID = null;
    private Action<bool> showAdSuccessCompletionHandler;


    /***
     * Initialize Ad
     */

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

#if UNITY_ANDROID
        gameID = androidGameID;
        rewardedID = androidRewardedID;
#elif UNITY_IOS
        gameID = iosGameID;
        rewardedID = iosRewardedID;
#elif UNITY_EDITOR
        gameID = androidGameID;
        rewardedID = androidRewardedID;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
            Advertisement.Initialize(gameID, testMode, this);

    }

    private void Start()
    {
        
    }

    public void OnInitializationComplete()
    {
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Error Initializing Ads: " + error + "\n" + message);
    }

    /***
     * Load Ad
     */

    public void LoadAd()
    {
        Advertisement.Load(rewardedID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(rewardedID))
        {
            // This ad successfully loaded
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Error Loading Ads: " + error + "\n" + placementId + "\n" + message);
    }

    /***
     * Show Ad
     */

    public void ShowAd(Action<bool> onSuccess)
    {
        Advertisement.Show(rewardedID, this);

        showAdSuccessCompletionHandler = onSuccess;
    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        bool success = false;
        if (placementId.Equals(rewardedID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // Rewarded ad complete... grant a reward
            success = true;
        }

        showAdSuccessCompletionHandler?.Invoke(success);

        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        // Maybe try to load another ad, check UnityAdsShowError conditions
        if (error != UnityAdsShowError.ALREADY_SHOWING)
        {
            LoadAd();
        }
    }

}
