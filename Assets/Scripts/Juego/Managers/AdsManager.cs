using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    private int RecompensaAnuncio = 20;
    private string videoID = "rewardedVideo";

    void Awake()
    {
        Advertisement.Initialize(videoID, true);
    }

    void Start()
    {
        GameManager.instance.SetAdsManager(this);
    }

    public void ShowAd(string zone = "")
    {
#if UNITY_EDITOR
        StartCoroutine(WaitForAd());
#endif

        if (!Advertisement.IsReady(videoID))
            Debug.LogWarning("Video not available");
        else
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = AdCallbackHandler;
            Advertisement.Show(videoID, options);
        }
    }

    void AdCallbackHandler(ShowResult result)
    {
        if(result == ShowResult.Finished)
        {
            GameManager.instance.SumaMonedas(RecompensaAnuncio);
        }
    }

    IEnumerator WaitForAd()
    {
        float currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return null;

        while (Advertisement.isShowing)
            yield return null;

        Time.timeScale = currentTimeScale;
    }
}