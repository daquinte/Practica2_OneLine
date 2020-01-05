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




    //Constantes
    private const int precioChallenge = 25;
    private const int recompensaAnuncio = 25;
    private const int recompensaChallenge = 50;
    private const int recompensaLogin = 100;

    //ultimo anuncio
    private int tipoUltimoAnuncio;
    private int cantidadADuplicar;

    public int numberToCreate = 100;

    /// <summary>
    /// Struct con la información persistente entre escenas
    /// </summary>
    public struct InfoEleccionJugador
    {
        public string tipoDificultadActual;
        public int numNivelActual;
        public bool isChallenge;
        public int dificultad;
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
            infoNivel.dificultad = dificultad;
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


 
    public InfoNivel GetInfoNivel(int nivel)
    {
        //PRIMERO: CAMBIO ESCENA A ESCENA JUEGO
        return lectorNiveles.CargaNivel(nivel);
    }

    public void LanzaAnuncio(int tipoAnuncio)
    {
        tipoUltimoAnuncio = tipoAnuncio;
        adsManager.ShowAd();
    }

    public void OnChallengeStart(bool pagadoConMonedas)
    {
        if (pagadoConMonedas)
        {
            RestaMonedas(precioChallenge);
            CargaEscenaJuego(1, true);

        }
        else LanzaAnuncio(0);

    }

    public void OnChallengeCompleted(bool duplicado)
    {
        if (duplicado)
        {

            cantidadADuplicar = recompensaChallenge;
            LanzaAnuncio(2);

        }
        else SumaMonedas(recompensaChallenge);
    }


    public void OnDailyLoginReward(bool duplicar)
    {
       
        if (duplicar)
        {
            cantidadADuplicar = recompensaLogin;
            LanzaAnuncio(2);
        }
        else SumaMonedas(recompensaLogin);
    }

    public void RecompensaJugador()
    {
        switch (tipoUltimoAnuncio)
        {
            //0 = Normal, ir gratis a la escena de Challenge
            case 0:
                CargaEscenaJuego(1, true);
                break;
            //1 = +25 monedas
            case 1:
                SumaMonedas(recompensaAnuncio);
                break;
            //2 = monedas*2
            case 2:
                SumaMonedas(cantidadADuplicar * 2);
                break;
        }

    }

    private void SumaMonedas(int cantidad)
    {
        datosJugador._monedas += cantidad;
        SavePlayer();
    }

    private void RestaMonedas(int cantidad)
    {
        if (datosJugador._monedas - cantidad >= 0)
            datosJugador._monedas -= cantidad;
        else datosJugador._monedas = 0;

        SavePlayer();
    }

    public void NuevoNivelSerializable(int nivel) {
        GameManager.instance.infoNivel.numNivelActual = nivel;
        // Serializacion        
        if (!datosJugador.playedLevels.ContainsKey(GameManager.instance.infoNivel.numNivelActual)) {
            datosJugador.playedLevels.Add(GameManager.instance.infoNivel.numNivelActual, true);
            ProgressManager.Save(datosJugador);
        }
        
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
        if (datosJugador == null) {
            datosJugador = new DatosJugador(100, 0);
        }
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

    public int [] GetNumberToCreate()
    {

        int numNivelesTotales = lectorNiveles.GetNumNiveles();
        int topeInferior = numberToCreate * infoNivel.dificultad + 1;
        int topeSuperior = numberToCreate * infoNivel.dificultad + numberToCreate;
        topeSuperior = (topeSuperior < numNivelesTotales) ? topeSuperior : numNivelesTotales;
        int[] array = { topeInferior, topeSuperior };
        return array;
    }

    #endregion
}
