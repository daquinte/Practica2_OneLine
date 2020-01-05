using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase que funciona como manager de un nivel. 
/// Controla la logica de los tiles del nivel
/// Gestiona el input en la aplicación
/// </summary>
public class BoardManager : MonoBehaviour
{
    //Atributos publicos
    [Tooltip("Canvas de la escena de juego. Debe tener los componentes GameScale y CanvasJuego.")]
    public GameObject _CanvasJuego;

    [Tooltip("Prefab of a Tile")]
    public Tile prefabTile;

    [Tooltip("Objeto cursor de tu escena")]
    public Cursor cursor;

    [Space]
    [Tooltip("Si quieres una skin en particular, añadela aquí.")]
    public TileSkin preferedSkin;

    [Tooltip("Array de ScriptableObjects para las skins.")]
    public List<TileSkin> tileSkins;

    //Atributos privados//


    private int TARGET_WIDTH;
    private int TARGET_HEIGHT;
    private int PIXELS_TO_UNITS = 110;



    /// <summary>
    /// Array con los tiles
    /// </summary>
    private Tile[,] tiles;


    /// <summary>
    /// Recorrido efectuado en forma de pila.
    /// </summary>
    private List<Tile> caminoTiles;


    /// <summary>
    /// Camino a la meta
    /// </summary>
    private int[,] pistas;


    private TileSkin currentTileSkin;   //TileSkin del nivel actual

    private GameScale gameScale;        //componente GameScale del objeto canvas de tu escena.
    private CanvasJuego canvasJuego;    //componente Canvas Juego del objeto canvas de tu escena.

    private int nTotalTiles;            //Número total de tiles 
    private int nFils;                  //numero filas del tablero
    private int nCols;                  //numero columnas del tablero

    private int screenWidth;
    private int screenHeight;

    bool init = false; //TEMPORAL, QUITAR CUANDO SE CARGE EL NIVEL DESDE EL MENU
    bool isChallenge = false;           //¿Estamos jugando un nivel Challenge?

    // Use this for initialization
    void Start() { 
        caminoTiles = new List<Tile>();

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
    void Update() {
        //QUITAR ESTE IF CUANDO EL OBJETO SE CARGE DESDE EL MENU
        if (init) {
            if (GameManager.instance.GetInputManager().getInputInfo().pulsado) {
                coordsDentroMatriz((int)Math.Round(GameManager.instance.GetInputManager().getInputInfo().position.x),
                                    (int)Math.Round(GameManager.instance.GetInputManager().getInputInfo().position.y));
            }
        }
        if (screenWidth != Screen.width || screenHeight != Screen.height) {
           ResizeCamera();
        }
    }

    /// <summary>
    /// Métodos de inicialización del Board Manager.
    /// </summary>
    #region Start Methods

    /// <summary>
    /// Ponemos este mapa a tipo challenge.
    /// Es decir: no se guarda el progreso "general", y el mapa puede acabar abruptamente.
    /// </summary>
    public void SetCurrentMapAsChallenge()
    {
        isChallenge = true;
    }

    public void ResetMap()
    {
        caminoTiles = new List<Tile>();
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
    public void InitMap(InfoNivel infoNivel) {
        nFils = infoNivel.layout.Length;
        nCols = infoNivel.layout[0].Length;
        pistas = infoNivel.path;

        tiles = new Tile[nCols, nFils];
        nTotalTiles = 0;
        //Situamos la cámara
        ResizeCamera();

        for (int filas = 0; filas < tiles.GetLength(1); filas++) {
            string infoFila = infoNivel.layout[filas];
            for (int cols = 0; cols < tiles.GetLength(0); cols++) {
                char tipoTile = infoFila[cols];
                if (tipoTile != '0') {
                    ///Si en el futuro hay que escalar los bloques, si quitas que sean hijos de boardManager se pueden escalar!
                    Tile tile = Instantiate(prefabTile, new Vector3(cols, -filas, 0), Quaternion.identity, transform);
                    tile.gameObject.name = "Bloque" + cols + filas;
                    tile.SetTileSkin(currentTileSkin);
                    tiles[cols, filas] = tile;
                    nTotalTiles++;
                    if (tipoTile == '2') { //Si es inicial...
                        tile.SetTileInicial();
                        caminoTiles.Add(tiles[cols, filas]);
                    }

                }
            }
        }
        init = true;
    }

 

    private void GetRandomSkin()
    {
        if (preferedSkin == null) {
            int rnd = UnityEngine.Random.Range(0, tileSkins.Count);
            currentTileSkin = tileSkins[rnd];
        }
        else currentTileSkin = preferedSkin;
    }

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
        if (tiles[x, y].GetPulsado()) {
            DeshacerCamino(tiles[x, y]);
        }
        else {
            Vector3 posicion = new Vector3(0, 0, 0);
            Vector3 sentido = new Vector3(0, 0, 0);
            if (esCandidato(caminoTiles[caminoTiles.Count-1], tiles[x, y], ref posicion, ref sentido)) {
                tiles[x, y].Pulsar();
                tiles[x, y].MarcarCamino(false, caminoTiles[caminoTiles.Count - 1], posicion, sentido);
                caminoTiles.Add(tiles[x, y]);

                if (NivelCompletado()) {

                    if (isChallenge){
                         canvasJuego.ShowChallengeCompletado();
                    }
                    else canvasJuego.ShowSiguienteNivelPanel();
                }
            }
        }
    }

    public void coordsDentroMatriz(int x, int y)
    {
        if (y <= 0) {
            y = Math.Abs(y);
            if ((x >= 0 && x < tiles.GetLength(0)) && (y < tiles.GetLength(1)) && (tiles[x, y] != null)) {
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
        if (!NivelCompletado()) {
            bool flag = false;
            int fila = 0;
            Tile ultimoCorrecto = null;
            while(!flag && fila < caminoTiles.Count)
            {
                if (tiles[pistas[fila, 1], pistas[fila, 0]] == caminoTiles[fila])
                {
                    fila++;
                    ultimoCorrecto = caminoTiles[fila-1];
                    Debug.Log("ultimo correcto: " + ultimoCorrecto.gameObject.name);
                }
                else
                {
                    flag = true;
                }
            }

            if (flag) {
                DeshacerCamino(ultimoCorrecto);
            }
            MarcarCaminoPistas(fila);
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
        if (nTotalTiles - caminoTiles.Count >= 5) {
            final = comienzo + 5;
        }
        else {
            final = nTotalTiles;
        }
        Debug.Log("FINAL: " + final);
        for (int fils = 1; fils < final; fils++) {
            Debug.Log("Fila: " + fils);
            Debug.Log("X: " + pistas[fils, 1] + ", Y: " + pistas[fils, 0]);
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
        if (Math.Abs(diferenciaX) == 1 && diferenciaY == 0) {
            if (diferenciaX < 0) {
                posicion = new Vector3(-0.5f, 0, 0);
            }
            else {
                posicion = new Vector3(0.5f, 0, 0);
            }
            sentido = new Vector3(0, 0, 0);
            return true;
        }
        else if (diferenciaX == 0 && Math.Abs(diferenciaY) == 1) {
            if (diferenciaY < 0) {
                posicion = new Vector3(0, -0.5f, 0);
            }
            else {
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
        while (caminoTiles[caminoTiles.Count - 1] != bloquePulsado) {
            caminoTiles[caminoTiles.Count - 1].Despulsar();
            caminoTiles.Remove(caminoTiles[caminoTiles.Count - 1]);
            caminoTiles[caminoTiles.Count - 1].DesmarcarCamino();
        }
    }

    /// <summary>
    /// Encargado de determinar si el nivel se ha completado con éxito o no.
    /// </summary>
    /// <returns>-True si se han marcado todas las casillas, false en caso contrario</returns>
    private bool NivelCompletado() {
        return (caminoTiles.Count == nTotalTiles);
    }
    #endregion

}
