using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Necesitamos el lector de niveles para cargarlos desde cualquier escena
[RequireComponent(typeof(LectorNiveles))]

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

    LectorNiveles lectorNiveles;
    AdsManager   adsManager = null;
    BoardManager boardManager = null;
    InputManager inputManager = null;

    DatosJugador datosJugador;
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

    }

    void Start()
    { 
        lectorNiveles = GetComponent<LectorNiveles>();
        lectorNiveles.CargaTodosLosNiveles();
    }

    public void JuegaNivel(int nivel) //bool isChallenge??
    {
        //PRIMERO: CAMBIO ESCENA A ESCENA JUEGO
        boardManager.InitMap(lectorNiveles.CargaNivel(nivel));
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
        if(datosJugador == null)
        {
            datosJugador = new DatosJugador(100, 0);
        }
        Debug.Log(datosJugador._monedas);
    }

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
