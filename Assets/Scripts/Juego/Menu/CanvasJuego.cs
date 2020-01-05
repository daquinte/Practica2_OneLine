using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase encargada de gestionar el canvas en la escena de juego
/// Sabe cambiar el canvas para las escenas de juego normales, y adaptarla
/// a las escenas de tipo "challenge".
/// </summary>
public class CanvasJuego : MonoBehaviour
{
    [Tooltip("Panel de siguiente nivel para un nivel normal.")]
    public GameObject panelSiguienteNivel;

    [Tooltip("Panel de nivel completado para challenge.")]
    public GameObject panelChallengeWin;

    [Tooltip("Panel de nivel no superado para challenge.")]
    public GameObject panelChallengeLost;

    //Elementos del banner constantes
    public Text textoMonedas;
    public Text textoDificultad;

    //Elementos de la HUD de una partida normal
    public Button botonReset;
    public Button botonAnuncio;
    public Button botonPista;

    //Elementos de la HUD para partida de tipo challenge
    public CountDown countDown;

    public Text TextoSigNivelDif; 
    public Text TextoSigNivelNum;

    // Start is called before the first frame update
    void Start()
    {
        textoMonedas.text = GameManager.instance.GetDatosJugador()._monedas.ToString();
        textoDificultad.text = GameManager.instance.infoNivel.tipoDificultadActual + "  " + GameManager.instance.infoNivel.numNivelActual.ToString();


        if (GameManager.instance.infoNivel.isChallenge)
        {
            SetChallengeHUD();
        }
        else SetStandardHUD();
    }

    // Update is called once per frame
    void Update()
    {
        textoMonedas.text = GameManager.instance.GetDatosJugador()._monedas.ToString();

    }

    public void ShowSiguienteNivelPanel() {
        this.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        TextoSigNivelDif.text = GameManager.instance.infoNivel.tipoDificultadActual;
        TextoSigNivelNum.text = GameManager.instance.infoNivel.numNivelActual.ToString();
        panelSiguienteNivel.SetActive(true); 
    
    }
    public void HideSiguienteNivelPanel() { panelSiguienteNivel.SetActive(false); }

    public void ShowChallengeCompletado() {
        this.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        countDown.StopTimer();
        panelChallengeWin.SetActive(true); 
    }

    public void ChallengeFallido() {
        this.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        panelChallengeLost.SetActive(true); 
    }

    public void HideChallengeFallido() { 
        
        panelChallengeLost.SetActive(false); 
    }

    ///Callbacks de la escena
    #region Callbacks
    /// <summary>
    /// Callback para informar a la instancia del nivel que queremos volver a seleccionar nivel
    /// </summary>
    public void GoToSeleccionNivel()
    {
        GameManager.instance.CargaSeleccionNivel(GameManager.instance.infoNivel.dificultad);
    }

    /// <summary>
    /// Callback para informar a la instancia del nivel que queremos volver al titulo
    /// </summary>
    public void GoToTitulo() {
        GameManager.instance.CargaEscenaTitulo();
    }

    /// <summary>
    /// Callback para informar a la instancia del nivel que queremos volver a seleccionar nivel
    /// </summary>
    public void GoToSiguienteNivel() {
        GameManager.instance.cargaSiguienteNivel();
        this.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        this.GetComponent<Canvas>().worldCamera = Camera.main;
        HideSiguienteNivelPanel();
    }

    public void AnuncioPorMonedas() {
        GameManager.instance.LanzaAnuncio(1);
    }

    public void CompletarChallenge()
    {
        GameManager.instance.OnChallengeCompleted(false);
    }
    public void DuplicarChallenge()
    {
        GameManager.instance.OnChallengeCompleted(true); 
    }
    #endregion


    //Cambios privados en la interfaz
    #region HUD Change
    private void SetChallengeHUD()
    {
        botonReset.gameObject.SetActive(false);
        botonAnuncio.gameObject.SetActive(false);
        botonPista.gameObject.SetActive(false);

        countDown.gameObject.SetActive(true);
    }

    private void SetStandardHUD()
    {
        botonReset.gameObject.SetActive(true);
        botonAnuncio.gameObject.SetActive(true);
        botonPista.gameObject.SetActive(true);

        countDown.gameObject.SetActive(false);
    }
    #endregion
}
