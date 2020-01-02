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
    public Text textoMonedas;

    //Elementos de la HUD de una partida normal
    public Button botonReset;
    public Button botonAnuncio;
    public Button botonPista;

    //Elementos de la HUD para partida de tipo challenge
    //Debe estar inactivo por defecto.
    public CountDown countDown;


    private bool isChallenge;

    // Start is called before the first frame update
    void Start()
    {
        isChallenge = false;
        textoMonedas.text = GameManager.instance.GetDatosJugador()._monedas.ToString();

        //Temporal:SetStandardHUD
        SetStandardHUD();
    }

    // Update is called once per frame
    void Update()
    {
        textoMonedas.text = GameManager.instance.GetDatosJugador()._monedas.ToString();

    }

    public void SetChallengeHUD()
    {
        botonReset.gameObject.SetActive(false);
        botonAnuncio.gameObject.SetActive(false);
        botonPista.gameObject.SetActive(false);

        countDown.gameObject.SetActive(true);
    }

    public void SetStandardHUD()
    {
        botonReset.gameObject.SetActive(true);
        botonAnuncio.gameObject.SetActive(true);
        botonPista.gameObject.SetActive(true);

        countDown.gameObject.SetActive(false);
    }

}
