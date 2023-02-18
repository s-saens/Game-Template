using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Mediation;
using UnityEngine;
#pragma warning disable 4014

public class MediationController : MonoBehaviour, IDisposable
{
    [SerializeField] Event showAdEvent_;

    void OnEnable()
    {
        showAdEvent_.callback += ShowAd;
    }
    void OnDisable()
    {
        showAdEvent_.callback -= ShowAd;
    }


    IRewardedAd ad;
    [SerializeField] string adUnitIdSuffix_Android = "_Android";
    [SerializeField] string adUnitIdSuffix_iOS = "_iOS";
    string adUnitId = "Rewarded";
    [SerializeField] string gameId_Android = "";
    [SerializeField] string gameId_iOS = "";
    string gameId = "";


    [Header("Events")]
    [SerializeField] Event adCompletedEvent;
    [SerializeField] Event adShowFailEvent;


    // Static Flags
    static Data<bool> isAdInitialized = new Data<bool>(false);
    static Data<bool> isAdLoaded = new Data<bool>(false);

    void Start()
    {
        InitGameID();
        InitServices();
    }

    void InitGameID()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            gameId = gameId_Android;
            adUnitId += adUnitIdSuffix_Android;
            return;
        }
        
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameId = gameId_iOS;
            adUnitId += adUnitIdSuffix_iOS;
            return;
        }
    }

    async Task InitServices()
    {
        try
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetGameId(gameId);
            await UnityServices.InitializeAsync(initializationOptions);
            InitializationComplete();
        }
        catch (Exception e)
        {
            InitializationFailed(e);

            await Task.Delay(1000);
            await InitServices();
        }
    }

    void SetupAd()
    {
        //Create
        ad = MediationService.Instance.CreateRewardedAd(adUnitId);

        //Subscribe to events
        ad.OnClosed += AdClosed;
        ad.OnClicked += AdClicked;
        ad.OnLoaded += AdLoaded;
        ad.OnFailedLoad += AdFailedLoad;
        ad.OnUserRewarded += UserRewarded;

        // Impression Event
        MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;
    }

    public void Dispose() => ad?.Dispose();

    async void ShowAd()
    {
        if (!isAdLoaded.value) adShowFailEvent.Invoke();

        else
            try
            {
                RewardedAdShowOptions showOptions = new RewardedAdShowOptions();
                showOptions.AutoReload = true;
                await ad.ShowAsync(showOptions);
                AdShown();
            }
            catch (ShowFailedException e)
            {
                AdFailedShow(e);
            }
    }

    void InitializationComplete()
    {
        isAdInitialized.value = true;
        SetupAd();
        LoadAd();
    }

    async Task LoadAd()
    {
        try
        {
            await ad.LoadAsync();
        }
        catch (LoadFailedException)
        {
            await Task.Delay(1000);
            await LoadAd();
        }
    }

    void InitializationFailed(Exception e)
    {
        isAdInitialized.value = false;
        Debug.Log("Failed to Initialize ad");
    }

    void AdLoaded(object sender, EventArgs e)
    {
        isAdLoaded.value = true;
        Debug.Log("Ad loaded YEAH~");
    }

    void AdFailedLoad(object sender, LoadErrorEventArgs e)
    {
        isAdLoaded.value = false;
        Debug.Log("Failed to load ad");
        Debug.Log(e.Message);
    }

    void AdShown()
    {
        Debug.Log("Ad shown!");
    }

    void AdClosed(object sender, EventArgs e)
    {
        isAdLoaded.value = ad.AdState == AdState.Loaded;
    }

    void AdClicked(object sender, EventArgs e)
    {
        Debug.Log("Ad has been clicked");
    }

    void AdFailedShow(ShowFailedException e)
    {
        Debug.Log(e.Message);
    }

    void ImpressionEvent(object sender, ImpressionEventArgs args)
    {
        var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
        Debug.Log("Impression event from ad unit id " + args.AdUnitId + " " + impressionData);
    }

    void UserRewarded(object sender, RewardEventArgs e)
    {
        Debug.Log($"Received reward: type:{e.Type}; amount:{e.Amount}");

        adCompletedEvent.Invoke();
    }
}