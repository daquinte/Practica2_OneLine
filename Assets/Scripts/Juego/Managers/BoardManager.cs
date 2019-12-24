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

    [Space]
    [Tooltip("Si quieres una skin en particular, añadela aquí.")]
    public TileSkin preferedSkin;

    public Cursor cursor;

    public InputManager inputManager;

    [Tooltip("Array de ScriptableObjects para las skins.")]
    public List<TileSkin> tileSkins;



    //Atributos privados

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

    private TileSkin currentTileSkin;

    private int nFils; //numero filas del tablero
    private int nCols; //numero columnas del tablero

    //Se supone que el juego tiene dos anchos de tile distintos (enunciado)
    //Los de Beginner son más anchos, creo.
    private int _anchoTile;
    private int _altoTile;

    // Use this for initialization
    void Start()
    {
        //Todo: actualizar con los valores del txt
        nCols = 5;
        nFils = 9;
        tiles = new Tile[nCols, nFils];

        caminoTiles = new Stack<Tile>();
        Camera.main.transform.position = new Vector3(nCols / 2, nFils / 2, Camera.main.transform.position.z);

        GetRandomSkin();
        cursor.SetSprite(currentTileSkin.spriteDedo);
        InitTiles();
        GameManager.instance.SetBoardManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.getInputInfo().pulsado)
        {
            coordsDentroMatriz((int)Math.Round(inputManager.getInputInfo().position.x), (int)Math.Round(inputManager.getInputInfo().position.y));
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
    #endregion

    /// <summary>
    /// Métodos de lógica de juego y comprobaciones del mismo.
    /// </summary>
    #region Logic Methods
    private bool esCandidato(Tile tileCandidato)
    {
        Tile top = caminoTiles.Peek();
        int diferenciaX = Math.Abs((int)(tileCandidato.gameObject.transform.position.x - top.gameObject.transform.position.x));
        int diferenciaY = Math.Abs((int)(tileCandidato.gameObject.transform.position.y - top.gameObject.transform.position.y));
        return (diferenciaX == 1 && diferenciaY == 0) || (diferenciaX == 0 && diferenciaY == 1);
    }

    public void SetTilePulsado(int x, int y)
    {

        // Asumimos que es una jugada legal
        if (tiles[x, y].GetPulsado())
        {
            DeshacerCamino(tiles[x, y]);
        }
        else
        {
            if (esCandidato(tiles[x, y]))
            {
                tiles[x, y].Pulsar();
                caminoTiles.Push(tiles[x, y]);
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
            //caminoTiles.Peek().QuitaCamino(); //<- El caminito entre dos bloques que habrá que hacer
        }
    }
    #endregion

}
