using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Necesitamos el lector de niveles para cargarlos desde cualquier escena
[RequireComponent(typeof(LectorNiveles))]

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

    LectorNiveles lectorNiveles;
    AdsManager adsManager = null;
    BoardManager boardManager = null;
    InputManager inputManager = null;

    DatosJugador datosJugador;

    /// <summary>
    /// Struct con la información persistente entre escenas
    /// </summary>
    public struct InfoEleccionJugador
    {
        public string tipoDificultadActual;
        public int numNivelActual;
        public bool isChallenge;
    }

    public InfoEleccionJugador infoNivel;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Loads the player, if there is any
        LoadPlayer();

        lectorNiveles = GetComponent<LectorNiveles>();
        lectorNiveles.CargaTodosLosNiveles();

        //TEMPORAL
        infoNivel.tipoDificultadActual = "DEBUG";
        infoNivel.numNivelActual = 0;
        infoNivel.numNivelActual = 203;
    }

    /// <summary>
    /// Actualmente, todos los cambios de escena tienen efecto en el *siguiente frame*
    /// Debido a que usamos LoadScene en lugar de su versión asíncrona
    /// </summary>
    #region SceneManagement

    public void CargaEscenaTitulo()
    {
        SceneManager.LoadScene(0);
    }

    public void CargaSeleccionNivel(int dificultad)
    {
        if (dificultad >= 0 && dificultad <= 4)
        {
            switch (dificultad)
            {

                case 0:
                    infoNivel.tipoDificultadActual = "BEGINNER";
                    break;

                case 1:
                    infoNivel.tipoDificultadActual = "REGULAR";
                    break;
                case 2:
                    infoNivel.tipoDificultadActual = "ADVANCED";
                    break;
                case 3:
                    infoNivel.tipoDificultadActual = "EXPERT";
                    break;
                case 4:
                    infoNivel.tipoDificultadActual = "MASTER";
                    break;

                    //ETC ETC
            }

        }
        SceneManager.LoadScene(1);
    }


    public void CargaEscenaJuego(int nivel, bool isChallenge)
    {
        infoNivel.numNivelActual = nivel;
        infoNivel.isChallenge = isChallenge;
        SceneManager.LoadScene(2);
    }
    #endregion


    public void JuegaNivel(int nivel)
    {
        boardManager.InitMap(lectorNiveles.CargaNivel(nivel));
    }
    public InfoNivel GetInfoNivel(int nivel)
    {
        //PRIMERO: CAMBIO ESCENA A ESCENA JUEGO
        return lectorNiveles.CargaNivel(nivel);
    }

    public void LanzaAnuncio()
    {
        adsManager.ShowAd();
    }

    public void SumaMonedas(int cantidad)
    {
        datosJugador._monedas += cantidad;
        SavePlayer();
    }

    public void RestaMonedas(int cantidad)
    {
        if (datosJugador._monedas - cantidad >= 0)
            datosJugador._monedas -= cantidad;
        else datosJugador._monedas = 0;

        SavePlayer();
    }

    #region Save and Load
    /// <summary>
    /// Guarda el estado del jugador
    /// </summary>
    public void SavePlayer()
    {
        ProgressManager.Save(datosJugador);
    }
    private void LoadPlayer()
    {
        datosJugador = ProgressManager.Load();
        if (datosJugador == null)
        {
            datosJugador = new DatosJugador(100, 0);
        }
        Debug.Log(datosJugador._monedas);
    }
    #endregion

    #region Set and gets

    /// <summary>
    /// Asigna el boardManager que se va a usar.
    /// De esta manera, todos accederán a la misma instancia a traves del getter correspondiente
    /// </summary>
    /// <param name="instance">Instancia de BoardManager</param>
    public void SetBoardManager(BoardManager instance)
    {
        boardManager = instance;
    }

    /// <summary>
    /// Asigna el Input Manager que se va a usar.
    /// De esta manera, todos accederán a la misma instancia a traves del getter correspondiente.
    /// </summary>
    /// <param name="instance">Instancia de InputManager</param>
    public void SetInputManager(InputManager instance)
    {
        inputManager = instance;
    }

    /// <summary>
    /// Asigna el Ads Manager que se va a usar.
    /// De esta manera, GameManager se encargará de lanzar anuncios cuando sea necesario
    /// Y gestionará las recompensas de los mismos.
    /// </summary>
    /// <param name="instance">Instancia de AdsManager</param>
    public void SetAdsManager(AdsManager instance)
    {
        adsManager = instance;
    }

    public BoardManager GetBoardManager()
    {
        return boardManager;
    }


    public InputManager GetInputManager()
    {
        return inputManager;
    }

    /// <summary>
    /// Devuelve el estado del jugador
    /// En forma de clase que no puede modificarse desde fuera.
    /// </summary>
    /// <returns>Datos del jugador</returns>
    public DatosJugador GetDatosJugador()
    {
        return datosJugador;
    }

    #endregion
}
