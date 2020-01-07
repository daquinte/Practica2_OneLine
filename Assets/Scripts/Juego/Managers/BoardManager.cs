using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase que funciona como manager de un nivel. 
/// Controla la logica de los tiles del nivel
/// Gestiona la carga de niveles en función de sus datos
/// </summary>
public class BoardManager : MonoBehaviour
{
    //---Atributos publicos---//
    [Tooltip("Canvas de la escena de juego. Debe tener los componentes GameScale y CanvasJuego.")]
    public GameObject _CanvasJuego;

    [Tooltip("Prefab of a Tile")]
    public Tile prefabTile;

    [Tooltip("Objeto cursor de tu escena")]
    public Cursor cursor;

    [Space]

    [Tooltip("Array de ScriptableObjects para las skins.")]
    public List<TileSkin> tileSkins;

    //---Atributos privados---//
    private int PIXELS_TO_UNITS = 110;

    /// <summary>
    /// Controla el ultimo tile desde el que se ha preguntado la pista
    /// Así evitamos que el jugador pierda dinero al darle dos veces al botón de pista
    /// </summary>
    private Tile ultimoInicioPista;
    private Tile[,] tiles;                  //Array con los tiles del nivel
    private List<Tile> caminoTiles;         //Recorrido efectuado por el jugador
                                            
    private int[,] pistas;                  //Camino hasta la meta
                                            
                                            
    private TileSkin currentTileSkin;       //TileSkin del nivel actual
    private GameScale gameScale;            //componente GameScale del objeto canvas de tu escena.
    private CanvasJuego canvasJuego;        //componente Canvas Juego del objeto canvas de tu escena.
                                            
    private int nTotalTiles;                //Número total de tiles 
    private int nFils;                      //numero filas del tablero
    private int nCols;                      //numero columnas del tablero
    private int screenWidth;                //ancho de la pantalla en píxeles
    private int screenHeight;               //alto de la pantalla en píxeles

    bool isChallenge = false;               //¿Estamos jugando un nivel Challenge?

    // Use this for initialization
    void Start()
    {
        caminoTiles = new List<Tile>();

        //Tomamos una skin aleatoria
        GetRandomSkin();
        cursor.SetSprite(currentTileSkin.spriteDedo);

        GameManager.instance.SetBoardManager(this);

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        gameScale = _CanvasJuego.GetComponent<GameScale>();
        canvasJuego = _CanvasJuego.GetComponent<CanvasJuego>();

        InitMap(GameManager.instance.GetInfoNivel(GameManager.instance.infoNivel.numNivelActual));
        isChallenge = GameManager.instance.infoNivel.isChallenge;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.GetInputManager().getInputInfo().pulsado)
        {
            coordsDentroMatriz((int)Math.Round(GameManager.instance.GetInputManager().getInputInfo().position.x),
                                (int)Math.Round(GameManager.instance.GetInputManager().getInputInfo().position.y));
        }

        //Si ha cambiado la pantalla, redimensionamos la escena.
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            ResizeCamera();
        }
    }

    /// <summary>
    /// Métodos de inicialización del Board Manager.
    /// </summary>
    #region Start Methods

    /// <summary>
    /// Borra el mapa actual instanciado en la escena
    /// </summary>
    public void ResetMap()
    {
        caminoTiles = new List<Tile>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// Inicializa un mapa en base al infonivel de referencia
    /// </summary>
    /// <param name="infoNivel">Struct con información necesaria para cargar un nivel</param>
    public void InitMap(InfoNivel infoNivel)
    {
        nFils = infoNivel.layout.Length;
        nCols = infoNivel.layout[0].Length;
        pistas = infoNivel.path;

        tiles = new Tile[nCols, nFils];
        nTotalTiles = 0;

        //Escalamos y situamos la cámara
        ResizeCamera();

        for (int filas = 0; filas < tiles.GetLength(1); filas++)
        {
            string infoFila = infoNivel.layout[filas];
            for (int cols = 0; cols < tiles.GetLength(0); cols++)
            {
                char tipoTile = infoFila[cols];
                if (tipoTile != '0')
                {
                    ///Si en el futuro hay que escalar los bloques, si quitas que sean hijos de boardManager se pueden escalar!
                    Tile tile = Instantiate(prefabTile, new Vector3(cols, -filas, 0), Quaternion.identity, transform);
                    tile.gameObject.name = "Bloque" + cols + filas;
                    tile.SetTileSkin(currentTileSkin);
                    tiles[cols, filas] = tile;
                    nTotalTiles++;

                    //Si es inicial...
                    if (tipoTile == '2')
                    { 
                        tile.SetTileInicial();
                        caminoTiles.Add(tiles[cols, filas]);
                    }

                }
            }
        }
    }


    /// <summary>
    /// Elige una skin aleatoria del array de TileSkins.
    /// </summary>
    private void GetRandomSkin()
    {

        int rnd = UnityEngine.Random.Range(0, tileSkins.Count);
        currentTileSkin = tileSkins[rnd];
    }

    /// <summary>
    /// Redimensionamos la cámara en función del tamaño de la pantalla 
    /// y del canvas
    /// </summary>
    private void ResizeCamera()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        float bloquesMargenArriba = gameScale.CubosSpriteArriba(PIXELS_TO_UNITS);
        float bloquesMargenAbajo = gameScale.CubosSpriteAbajo(PIXELS_TO_UNITS);
        float filasNormalizadas = (nFils < 6.0f) ? 5.0f : 8.0f;
        float margenSuperior = 3.0f;
        float margenLateral = gameScale.CubosLaterales(PIXELS_TO_UNITS);
        float margenes = margenSuperior + margenLateral;
        float bloquesTotales = filasNormalizadas + bloquesMargenAbajo + bloquesMargenArriba + margenes;
        Camera.main.orthographicSize = (bloquesTotales / 2.0f) * gameScale.GetMargenSuperior();
        Camera.main.transform.position = new Vector3(nCols / 2.0f - 0.5f,
            -nFils / 2.0f + 0.7f, Camera.main.transform.position.z);
    }
    #endregion

    /// <summary>
    /// Métodos de lógica de juego y comprobaciones del mismo.
    /// </summary>
    #region Logic Methods
    public void SetTilePulsado(int x, int y)
    {
        if (tiles[x, y].GetPulsado())
        {
            DeshacerCamino(tiles[x, y]);
        }
        else
        {
            Vector3 posicion = new Vector3(0, 0, 0);
            Vector3 sentido = new Vector3(0, 0, 0);
            if (esCandidato(caminoTiles[caminoTiles.Count - 1], tiles[x, y], ref posicion, ref sentido))
            {
                tiles[x, y].Pulsar();
                tiles[x, y].MarcarCamino(false, caminoTiles[caminoTiles.Count - 1], posicion, sentido);
                caminoTiles.Add(tiles[x, y]);

                if (NivelCompletado())
                {

                    if (isChallenge)
                    {
                        canvasJuego.ShowChallengeCompletado();
                    }
                    else
                    {
                        //Cuestión de estética
                        StartCoroutine(EndGameAfterDelay(0.5f));
                    }
                }
            }
        }
    }

    IEnumerator EndGameAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetMap();
        canvasJuego.ShowSiguienteNivelPanel();
        GameManager.instance.DesBloqueaSiguienteNivel();
    }

    public void coordsDentroMatriz(int x, int y)
    {
        if (y <= 0)
        {
            y = Math.Abs(y);
            if ((x >= 0 && x < tiles.GetLength(0)) && (y < tiles.GetLength(1)) && (tiles[x, y] != null))
            {
                SetTilePulsado(x, y);
            }
        }
    }

    /// <summary>
    /// Reinicia el camino del nivel al inicio del mismo.
    /// Sólo se respeta el tile inicial
    /// </summary>
    public void ReiniciaCamino()
    {
        DeshacerCamino(caminoTiles[0]);
    }

    /// <summary>
    /// Muestra las pistas según el estado actual de la partida.
    /// </summary>
    public void MostrarPista()
    {
        if (!NivelCompletado() && GameManager.instance.PuedoCobrarPista())
        {
            bool flag = false;
            int fila = 0;
            Tile ultimoCorrecto = null;
            while (!flag && fila < caminoTiles.Count)
            {
                if (tiles[pistas[fila, 1], pistas[fila, 0]] == caminoTiles[fila])
                {
                    fila++;
                    ultimoCorrecto = caminoTiles[fila - 1];
                }
                else
                {
                    flag = true;
                }
            }

            if (flag)
            {
                DeshacerCamino(ultimoCorrecto);
            }

            //Comprobamos que no se pida la pista dos veces desde el mismo sitio.
            if (ultimoInicioPista == null || ultimoInicioPista != ultimoCorrecto) {
                ultimoInicioPista = ultimoCorrecto;
                MarcarCaminoPistas(fila);
                GameManager.instance.CobraPista();
            }
        }

    }

    /// <summary>
    /// Marca el camino de las pistas. Toma, desde el cominezo, las 5 siguientes posiciones
    /// y las señala como camino a seguir.
    /// </summary>
    /// <param name="comienzo">Int de en que posicion debes comenzar a trazar el camino</param>
    private void MarcarCaminoPistas(int comienzo)
    {
        Vector3 posicion = Vector3.zero;
        Vector3 sentido = Vector3.zero;

        Tile anterior = caminoTiles[0];
        int final = 0;
        if (nTotalTiles - caminoTiles.Count >= 5)
        {
            final = comienzo + 5;
        }
        else
        {
            final = nTotalTiles;
        }

        for (int fils = 1; fils < final; fils++)
        {
            bool placebo = esCandidato(anterior, tiles[pistas[fils, 1], pistas[fils, 0]], ref posicion, ref sentido);
            tiles[pistas[fils, 1], pistas[fils, 0]].MarcarCamino(true, anterior, posicion, sentido);
            anterior = tiles[pistas[fils, 1], pistas[fils, 0]];
        }
    }

    // TODO: Comentar
    private bool esCandidato(Tile peek, Tile tileCandidato, ref Vector3 posicion, ref Vector3 sentido)
    {
        //Tile top = caminoTiles.Peek();
        int diferenciaX = (int)(tileCandidato.gameObject.transform.position.x - peek.gameObject.transform.position.x);
        int diferenciaY = (int)(tileCandidato.gameObject.transform.position.y - peek.gameObject.transform.position.y);
        if (Math.Abs(diferenciaX) == 1 && diferenciaY == 0)
        {
            if (diferenciaX < 0)
            {
                posicion = new Vector3(-0.5f, 0, 0);
            }
            else
            {
                posicion = new Vector3(0.5f, 0, 0);
            }
            sentido = new Vector3(0, 0, 0);
            return true;
        }
        else if (diferenciaX == 0 && Math.Abs(diferenciaY) == 1)
        {
            if (diferenciaY < 0)
            {
                posicion = new Vector3(0, -0.5f, 0);
            }
            else
            {
                posicion = new Vector3(0, 0.5f, 0);
            }
            sentido = new Vector3(0, 0, 90);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Deshace el camino hasta que el top del stack 
    /// es el bloque que queremos
    /// </summary>
    /// <param name="bloquePulsado"></param>
    private void DeshacerCamino(Tile bloquePulsado)
    {
        while (caminoTiles[caminoTiles.Count - 1] != bloquePulsado)
        {
            caminoTiles[caminoTiles.Count - 1].Despulsar();
            caminoTiles.Remove(caminoTiles[caminoTiles.Count - 1]);
            caminoTiles[caminoTiles.Count - 1].DesmarcarCamino();
        }
    }

    /// <summary>
    /// Encargado de determinar si el nivel se ha completado con éxito o no.
    /// </summary>
    /// <returns>-True si se han marcado todas las casillas, false en caso contrario</returns>
    private bool NivelCompletado()
    {
        return (caminoTiles.Count == nTotalTiles);
    }
    #endregion

}
