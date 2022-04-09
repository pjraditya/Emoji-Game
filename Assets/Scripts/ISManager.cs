using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISManager : MonoBehaviour
{
    public static ISManager instance;

    public bool canShowAds, validateAdNetworks; // please set validation adnetwork it back to false
    public bool isRewardedVideoAvaliable, isInterstitialAdsAvaliable;

    public float timeGap;
    public string appKey, admobID;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        Init();
    }

    void Start()
    {
        canShowAds = Connectivity();

        if (validateAdNetworks) 
            IronSource.Agent.validateIntegration();
        
        if(ConnectedToInternet())
            Invoke("LoadAds", 0.25f);
    }

    void Init()
    {
        //For Rewarded Video
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO);
        //For Interstitial
        IronSource.Agent.init(appKey, IronSourceAdUnits.INTERSTITIAL);
        //For Banners
        IronSource.Agent.init(appKey, IronSourceAdUnits.BANNER);
    }

    void LoadAds()
    {
        LoadBannerAds();
        LoadInterstitialAds();
        ShowBannerAds();
    }

    void Update()
    {
        canShowAds = Connectivity();
        if (!canShowAds)
            return;

        isInterstitialAdsAvaliable = IronSource.Agent.isInterstitialReady();
        isRewardedVideoAvaliable = IronSource.Agent.isRewardedVideoAvailable();

    }

    public bool ConnectedToInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return false;

        return true;
    }

    public bool TimeGapFinished()
    {
        timeGap -= Time.deltaTime;
        if (timeGap <= 0)
        {
            return true;
        }

        return false;
    }

    bool Connectivity()
    {
        return ConnectedToInternet() && TimeGapFinished();
    }

    public void ShowRewadedVideo()
    {
        if (!canShowAds && !isRewardedVideoAvaliable)
            return;

        PrintOut("CanShowAds @ ShowRewadedVideo");

        HideBannerAds();
        IronSource.Agent.showRewardedVideo();
    }


    public void LoadInterstitialAds()
    {
        if (!canShowAds)
            return;

        PrintOut("CanShowAds @ LoadInterstitialAds");

        IronSource.Agent.loadInterstitial();
    }

    public void ShowInterstitialAds()
    {
        if (!canShowAds && !isInterstitialAdsAvaliable)
            return;

        PrintOut("CanShowAds @ ShowInterstitialAds");

        HideBannerAds();
        IronSource.Agent.showInterstitial();
    }


    public void LoadBannerAds()
    {
        if (!ConnectedToInternet())
            return;

        PrintOut("CanShowAds @ LoadBannerAds");
        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
    }

    public void ShowBannerAds()
    {
        if (!ConnectedToInternet())
            return;

        IronSource.Agent.displayBanner();
    }

    public void HideBannerAds()
    {
        if (!ConnectedToInternet())
            return;

        IronSource.Agent.hideBanner();
    }

    public void RewardCallBacks()
    {
        // please place callbacks here
        if(!UIManager.instance)
            return;
        if(!UIManager.instance.isSkipped)
            return;
        
        UIManager.instance.isSkipped = false;
        UIManager.instance.NextLevel();

    }

    public void PrintOut(string txt)
    {
        print(txt);
    }


}
