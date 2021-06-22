using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public partial class Basics : MonoBehaviour, IUnityAdsListener
{
    public void RunAd()
    {
        if(Advertisement.IsReady("rewardedVideo"))
            Advertisement.Show("rewardedVideo");
        else
        {
            adErrorMenu.gameObject.SetActive(true);
            idleMenu.gameObject.SetActive(false);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Ads ready.");
    }
 
    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Ad error: " + message);
    }
 
    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ad started.");
    }
 
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(placementId == "rewardedVideo" && showResult == ShowResult.Finished)
        {
            Debug.Log("ran");
            numGold += idleGold;
            doubleIdleUses++;
            PlayerPrefs.SetInt("doubleIdleUses", doubleIdleUses);
        }
    }
}
