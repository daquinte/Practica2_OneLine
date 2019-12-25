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
    [Tooltip("Prefab of a Tile")]
    public Tile prefabTile;

    [Tooltip("Objeto cursor de tu escena")]
    public Cursor cursor;

    [Space]
    [Tooltip("Si quieres una skin en particular, añadela aquí.")]
    public TileSkin preferedSkin;
    
    [Tooltip("Array de ScriptableObjects para las skins.")]
    public List<TileSkin> tileSkins;


    public float TARGET_WIDTH = 1080;
    public float TARGET_HEIGHT = 1920;
    public float PIXELS_TO_UNITS = 110;


    //Atributos privados
    private InputManager inputManager;


    /// <summary>
    /// Array con los tiles
    /// </summary>
    private Tile[,] tiles;

    /// <summary>
    /// El camino de las pistas, que creo que está en ints.
    /// </summary>
    private List<int> caminoPistas;

    /// <summary>
    /// Recorrido efectuado en forma de pila.
    /// </summary>
    private Stack<Tile> caminoTiles;

    private TileSkin currentTileSkin;   //TileSkin del nivel actual


    private int nTotalTiles;            //Número total de tiles 
    private int nFils;                  //numero filas del tablero
    private int nCols;                  //numero columnas del tablero

    private int _anchoTile;
    private int _altoTile;

    // Use this for initialization
    void Start()
    {

        //Todo: actualizar con los valores del txt

        nCols = 3;
        nFils = 6;
        tiles = new Tile[nCols, nFils];
        nTotalTiles = nCols * nFils;

        caminoTiles = new Stack<Tile>();
        Camera.main.transform.position = new Vector3((nCols / 2.0f) - 0.5f, (nFils / 2.0f)-0.5f, Camera.main.transform.position.z);
        ResizeCamera(nCols, nFils);

        GetRandomSkin();
        cursor.SetSprite(currentTileSkin.spriteDedo);
        InitTiles();

        GameManager.instance.SetBoardManager(this);
        //inputManager = GameManager.instance.GetInputManager();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetInputManager().getInputInfo().pulsado)
        {
            coordsDentroMatriz((int)Math.Round(GameManager.instance.GetInputManager().getInputInfo().position.x), 
                                (int)Math.Round(GameManager.instance.GetInputManager().getInputInfo().position.y));
        }
    }

    /// <summary>
    /// Métodos de inicialización del Board Manager.
    /// </summary>
    #region Start Methods
    private void InitTiles()
    {
        for (int filas = 0; filas < tiles.GetLength(1); filas++)
        {
            for (int cols = 0; cols < tiles.GetLength(0); cols++)
            {
                Tile tile = Instantiate(prefabTile, new Vector3(cols, filas, 0), Quaternion.identity, transform);
                //tile.transform.SetParent(gameObject.transform);
                tile.gameObject.name = "Bloque" + cols + filas;
                tile.SetTileSkin(currentTileSkin);
                tiles[cols, filas] = tile;

                if (cols == 0 && filas == 0)
                {
                    tile.SetTileInicial();
                    caminoTiles.Push(tiles[cols, filas]);
                }
            }
        }
    }

    private void GetRandomSkin()
    {
        if (preferedSkin == null)
        {
            int rnd = UnityEngine.Random.Range(0, tileSkins.Count);
            currentTileSkin = tileSkins[rnd];
        }
        else currentTileSkin = preferedSkin;
    }

    void ResizeCamera(int cols, int fils)
    {
        float desiredRatio = TARGET_WIDTH / TARGET_HEIGHT;
        float currentRatio = (float)Screen.width / (float)Screen.height;

        if (currentRatio >= desiredRatio)
        {
            Camera.main.orthographicSize = TARGET_WIDTH / PIXELS_TO_UNITS;
        }
        else
        {
            float differenceInSize = desiredRatio / currentRatio;
            Camera.main.orthographicSize = TARGET_WIDTH / PIXELS_TO_UNITS * differenceInSize;
        }
    }
    #endregion

    /// <summary>
    /// Métodos de lógica de juego y comprobaciones del mismo.
    /// </summary>
    #region Logic Methods

    // TODO: Comentar
    private bool esCandidato(Tile tileCandidato, ref Vector3 posicion, ref Vector3 sentido)
    {
        Tile top = caminoTiles.Peek();
        int diferenciaX = (int)(tileCandidato.gameObject.transform.position.x - top.gameObject.transform.position.x);
        int diferenciaY = (int)(tileCandidato.gameObject.transform.position.y - top.gameObject.transform.position.y);
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
            if (esCandidato(tiles[x, y], ref posicion, ref sentido))
            {
                tiles[x, y].Pulsar();
                tiles[x, y].marcarCamino(caminoTiles.Peek(), posicion, sentido);
                caminoTiles.Push(tiles[x, y]);

                if (NivelCompletado())
                {
                    Debug.Log("Nivel completado :DD:D");
                }
            }
        }
    }

    public void coordsDentroMatriz(int x, int y)
    {
        if ((x >= 0 && x < tiles.GetLength(0)) && (y >= 0 && y < tiles.GetLength(1)))
        {
            SetTilePulsado(x, y);
        }
    }

    /// <summary>
    /// Reinicia el camino del nivel al inicio del mismo.
    /// Sólo se respeta el tile inicial
    /// </summary>
    public void ReiniciaCamino()
    {
        while (caminoTiles.Count != 1)
        {
            caminoTiles.Peek().Despulsar();
            caminoTiles.Pop();
            caminoTiles.Peek().DesmarcarCamino();
        }
    }

    /// <summary>
    /// Deshace el camino hasta que el top del stack 
    /// es el bloque que queremos
    /// </summary>
    /// <param name="bloquePulsado"></param>
    private void DeshacerCamino(Tile bloquePulsado)
    {
        while (caminoTiles.Peek() != bloquePulsado)
        {
            caminoTiles.Peek().Despulsar();
            caminoTiles.Pop();
            caminoTiles.Peek().DesmarcarCamino();
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
