using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener //nuevo de esta versión de los ads
{
    private int RecompensaAnuncio = 20;

    //ID del juego
    private string gameID = "123456";

    //Id de los tipos (placement)
    private string placementIdNormal = "video";
    private string placementIdRewarded = "rewardedVideo";

    void Awake()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, true);
    }

    void Start()
    {
        GameManager.instance.SetAdsManager(this);
    }

    //TODO: separar anuncios de dameMonedas de los de DuplicaMonedas
    public void ShowAd()
    {
        if (Advertisement.IsReady(placementIdNormal))
        {
            Advertisement.Show(placementIdNormal);
        }
        else Debug.Log("not ready");

    }


    ///Callback de la interfaz  IUnityAdsListener
    #region Unity Ads Callback

    /// <summary>
    /// Cuando el anuncio esté listo, se llamará a esta función
    /// </summary>
    /// <param name="placementId"></param>
    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == placementIdRewarded) Debug.Log("AD READY");
    }

    /// <summary>
    /// En caso de querer acciones aparte cuando se inicie el anuncio, irán aquí
    /// </summary>
    /// <param name="placementId">placement ID actual</param>
    public void OnUnityAdsDidStart(string placementId)
    {
        //No queremos hacer nada
    }


    /// <summary>
    /// Callback de anuncio completado.
    /// </summary>
    /// <param name="placementId">Placement ID</param>
    /// <param name="showResult">Información adicional de lo que ha hecho el jugador</param>
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {

        switch (showResult)
        {
            //El usuario ha visto todo el anuncio -> Reward
            case ShowResult.Finished:
                Debug.Log("MUY BIEN CAMPEON");
                break;

            case ShowResult.Skipped:
                Debug.Log("TE LO HAS SALTADO >:C");

                break;

            case ShowResult.Failed:
                throw new System.Exception("Ha fallado el anuncio.");

        }

    }


    public void OnUnityAdsDidError(string message)
    {
        throw new System.Exception("Error en el anuncio: " + message);
    }


    #endregion

}