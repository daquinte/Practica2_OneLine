using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]

/// <summary>
/// Este componente lanzará un anuncio con recompensa.
/// Extraido y adaptado de: https://docs.unity3d.com/Packages/com.unity.ads@3.2/manual/MonetizationBasicIntegrationUnity.html
/// </summary>
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{

#if UNITY_IOS
    private string gameId = "1486551";
#elif UNITY_ANDROID
    private string gameId = "1486550";
#endif

    public delegate void VoidRecompensa();

    private Button myButton;
    private string myPlacementId = "rewardedVideo";
   
    private VoidRecompensa callBackRecompensa; //Callback de este rewarded Ad
    void Start()
    {
        myButton = GetComponent<Button>();

        // Set interactivity to be dependent on the Placement’s status:
        myButton.interactable = Advertisement.IsReady(myPlacementId);

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementId);
    }

    /// <summary>
    /// Asigna al callback privado la funcion que recibe por parámetro.
    /// </summary>
    /// <param name="recompensaCB">Función que queremos llamar como callback del anuncio.</param>
    public void SetCallbackRecompensa(VoidRecompensa recompensaCB)
    {
        callBackRecompensa = recompensaCB;
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            if (callBackRecompensa != null) callBackRecompensa();
            //GameManager.instance.RecompensaJugador();
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}