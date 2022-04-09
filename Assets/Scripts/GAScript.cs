using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
//using Unity.Advertisement.IosSupport;

public class GAScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static GAScript Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        GameAnalytics.Initialize();

        RequestTrackingPermission();
    }

    public void LevelStart(string levelname)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelname);
        //FaceBookScript.instance.LevelStarted(levelname);
       
    }

    public void LevelFail(string levelname)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelname);
       // FaceBookScript.instance.LevelFailed(levelname);
       
    }

    public void LevelCompleted(string levelname)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelname);
       // FaceBookScript.instance.LevelCompleted(levelname);
       
    }

    public void RequestTrackingPermission()
    {
#if UNITY_IOS
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif
    }
}
