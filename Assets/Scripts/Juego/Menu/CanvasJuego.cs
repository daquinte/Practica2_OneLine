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
    public Text textoDificultad;

    //Elementos de la HUD de una partida normal
    public Button botonReset;
    public Button botonAnuncio;
    public Button botonPista;

    //Elementos de la HUD para partida de tipo challenge
    //Debe estar inactivo por defecto.
    public CountDown countDown;

    // Start is called before the first frame update
    void Start()
    {
        textoMonedas.text = GameManager.instance.GetDatosJugador()._monedas.ToString();

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

    /// <summary>
    /// Callback para informar a la instancia del nivel que queremos volver a seleccionar nivel
    /// </summary>
    public void GoToSeleccionNivel()
    {
        GameManager.instance.CargaSeleccionNivel(GameManager.instance.infoNivel.tipoDificultadActual);
    }

    /// <summary>
    /// Callback para informar a la instancia del nivel que queremos volver al titulo
    /// </summary>
    public void GoToTitulo()
    {
        GameManager.instance.CargaEscenaTitulo();
    }

    /// <summary>
    /// Callback para informar a la instancia del nivel que queremos volver a seleccionar nivel
    /// </summary>
    public void GoToSiguienteNivel()
    {
        //TODO: Cambiar el sorting layer del Canvas a "BackGround" Cuando aparezca el panel de Siguiente Nivel
        //NIVEL ++
        //GameManager.instance.CargaEscenaJuego(GameManager.instance.infoNivel.numNivelActual++, false);
    }

    public void DameAnuncio()
    {
        GameManager.instance.LanzaAnuncio();
    }
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

}
