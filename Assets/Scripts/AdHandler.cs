using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;


public partial class Basics : MonoBehaviour, IUnityAdsListener
{
    private DateTime adStartTime;
    private DateTime adCurrentTime;
    private TimeSpan adWaitTime;
    private bool adError;
    public GameObject adLoadingMenu;
    
    // private int a;
    // private void ad2()
    // {
    //     Advertisement.Initialize("4176969", true);
    //     Advertisement.AddListener(this);
    // }

    // ad coroutine
    IEnumerator AdTest()
    {
        adStartTime = DateTime.Now;
        if(!Advertisement.IsReady("rewardedVideo"))
        {
            adLoadingMenu.gameObject.SetActive(true);
            // Debug.Log("Loading Ad");
        }
        while(adWaitTime.Seconds < 5 && !Advertisement.IsReady("rewardedVideo") && !adError)
        {
            adCurrentTime = DateTime.Now;
            adWaitTime = adCurrentTime - adStartTime;
            // Debug.Log(Advertisement.IsReady("rewardedVideo"));
            yield return null;
        }
        if(Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo");
            adLoadingMenu.gameObject.SetActive(false);
        }
        else
        {
            adLoadingMenu.gameObject.SetActive(false);
            adErrorMenu.gameObject.SetActive(true);
        }
    }
    
    // handle watch ad button
    public void RunAd()
    {
        // adStartTime = DateTime.Now;
        // while(adWaitTime.Seconds < 2 && !Advertisement.IsReady("rewardedVideo"))
        // {
        //     adCurrentTime = DateTime.Now;
        //     adWaitTime = adCurrentTime - adStartTime;
        // }

        // Invoke("RunAd2", 3f);
        // adLoadingMenu.gameObject.SetActive(true);
        // Debug.Log("clicked");
        StartCoroutine(AdTest());
    }

    // private void RunAd2()
    // {
    //     if(Advertisement.IsReady("rewardedVideo"))
    //     {
    //         Advertisement.Show("rewardedVideo");
    //         adLoadingMenu.gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         adLoadingMenu.gameObject.SetActive(false);
    //         adErrorMenu.gameObject.SetActive(true);
    //     }
    // }

    // handle official methods

    public void OnUnityAdsReady(string placementId)
    {
        // Debug.Log("Ads ready.");
    }
 
    public void OnUnityAdsDidError(string message)
    {
        // Debug.Log("Ad error: " + message);
        adError = true;
    }
 
    public void OnUnityAdsDidStart(string placementId)
    {
        // Debug.Log("Ad started.");
    }
 
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(placementId == "rewardedVideo" && showResult == ShowResult.Finished)
        {
            // Debug.Log("ran");
            numGold += idleGold;
            doubleIdleUses++;
            PlayerPrefs.SetInt("doubleIdleUses", doubleIdleUses);
        }
    }
}
