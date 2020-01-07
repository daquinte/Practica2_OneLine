using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Necesitamos el lector de niveles para cargarlos desde cualquier escena
[RequireComponent(typeof(LectorNiveles))]

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

    [Tooltip("Define el numero de niveles por dificultad")]
    public int numberToCreate = 100;

    LectorNiveles lectorNiveles;
    BoardManager boardManager = null;
    InputManager inputManager = null;
    DatosJugador datosJugador;


    //Constantes
    private const int precioChallenge = 25;
    private const int precioPista = 25;
    private const int recompensaAnuncio = 25;
    private const int recompensaMonedasChallenge = 50;
    private const int recompensaMedallasChallenge = 1;
    private const int recompensaLogin = 35;


    private int nDificultades;
    private int[] nivelesPorDificultad;

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

        //Iniciamos por defecto
        infoNivel.tipoDificultadActual = "BEGINNER";
        infoNivel.numNivelActual = 1;
    }

    /// <summary>
    /// Actualmente, todos los cambios de escena tienen efecto en el *siguiente frame*
    /// Debido a que usamos LoadScene en lugar de su versión asíncrona
    /// </summary>
    #region SceneManagement


    //Cierra la aplicación
    public void CierraJuego()
    {
        Application.Quit();
    }
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
        return lectorNiveles.CargaNivel(nivel);
    }


    private void CalculaNivelesPorDificultad()
    {
        nivelesPorDificultad = new int[nDificultades];
        for (int i = 0; i < nivelesPorDificultad.Length; i++)
        {
            nivelesPorDificultad[i] = datosJugador.GetNumLevels(numberToCreate * i + 1, numberToCreate * i + numberToCreate);
        }
    }

    //Estados

    /// <summary>
    /// Empieza el nivel de challenge. 
    /// Tiene en cuenta si el jugador ha decidido pagar o ver un anuncio.
    /// Recibe "true" si es gratis (anuncio), false en caso contrario.
    /// </summary>
    /// <param name="free">¿Has pagado para jugar?</param>
    public void OnChallengeStart(bool free)
    {
        if (!free)
        {
            RestaMonedas(precioChallenge);
        }
        CargaEscenaJuego(Random.Range(100, 400), true);
    }

    /// <summary>
    /// Método que es llamado al completar un desafio 
    /// para obtener las recompensas pertinentes
    /// </summary>
    /// <param name="duplicado">Notifica si el usuario ha decidido ver un anuncio para duplicar</param>
    public void OnChallengeCompleted(bool duplicado)
    {
        int mult = 1;
        if (duplicado)
        {
            mult = 2;
        }
        SumaMonedas(recompensaMonedasChallenge   * mult);
        SumaMedallas(recompensaMedallasChallenge * mult);

    }

    /// <summary>
    /// Gestiona las recompensas del login diario
    /// </summary>
    /// <param name="duplicar"></param>
    public void OnDailyLoginReward(bool duplicar)
    {
        int mult = 1;
        if (duplicar)
        {
            mult = 2;
        }
        SumaMonedas(recompensaLogin * mult);
    }

    /// <summary>
    /// Llamado cuando el usuario ha visto un anuncio a cambio de monedas.
    /// </summary>
    public void OnRewardedAdWatched()
    {
        SumaMonedas(recompensaAnuncio);
    }
    /// <summary>
    /// Llamado cuando el usuario ha visto un anuncio para pasar al challenge gratis.
    /// </summary>
    public void OnFreeChallengeAdWatched()
    {
        CargaEscenaJuego(1, true);
    }

    /// <summary>
    /// Cobra al jugador el precio de una pista.
    /// </summary>
    public void CobraPista()
    {
        RestaMonedas(precioPista);
    }

    //--Niveles--//

    public void NuevoNivelSerializable(int nivel)
    {
        // Serializacion
        datosJugador.AsignaNivel(GameManager.instance.infoNivel.numNivelActual);
        ProgressManager.Save(datosJugador);
    }

    public void DesBloqueaSiguienteNivel()
    {
        infoNivel.numNivelActual++;
        NuevoNivelSerializable(infoNivel.numNivelActual);
    }
    public void CargaSiguienteNivel()
    {
        if (infoNivel.numNivelActual > (infoNivel.dificultad * numberToCreate + numberToCreate))
        {
            CargaSeleccionNivel(infoNivel.dificultad);
        }
        boardManager.InitMap(lectorNiveles.CargaNivel(infoNivel.numNivelActual));
    }


    //Métodos privados de suma y resta de monedas

    private void SumaMonedas(int cantidad)
    {
        if (datosJugador._monedas + cantidad <= 999)
        {
            datosJugador._monedas += cantidad;
        }
        else datosJugador._monedas = 999;
        SavePlayer();
    }

    private void RestaMonedas(int cantidad)
    {
        if (datosJugador._monedas - cantidad >= 0)
            datosJugador._monedas -= cantidad;
        else datosJugador._monedas = 0;

        SavePlayer();
    }

    private void SumaMedallas(int cantidad)
    {
        if (datosJugador._medallas + cantidad <= 999)
        {
            datosJugador._medallas += cantidad;
        }
        else datosJugador._medallas = 999;
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
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SavePlayer();
        }
    }
    void OnApplicationQuit()
    {
        SavePlayer();
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


    public void SetNumDificultades(int numDif)
    {
        nDificultades = numDif;
        for (int i = 0; i < nDificultades; i++)
        {
            datosJugador.AsignaNivel(numberToCreate * i + 1);
        }
    }

/// <summary>
/// Asigna la instancia de BoardManager a la que vamos a referenciar.
/// De esta manera siempre tendremos la misma referencia, asegurando la coherencia durante las partidas.
/// Se mantiene la misma instancia hasta
/// </summary>
/// <returns></returns>
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

    public int[] GetNivelesPorDificultad()
    {
        CalculaNivelesPorDificultad();
        return nivelesPorDificultad;
    }

    public int[] GetNumberToCreate()
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
