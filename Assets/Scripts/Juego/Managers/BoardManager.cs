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

    [Tooltip("Input Manager")]
    public InputManager inputManager;

    [Space]
    [Tooltip("Si quieres una skin en particular, añadela aquí.")]
    public TileSkin preferedSkin;

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

    private Sprite fingerSprite;

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
        nCols = 3;
        nFils = 1;
        tiles = new Tile[nCols, nFils];

        caminoTiles = new Stack<Tile>();
        Camera.main.transform.position = new Vector3(nCols / 2, nFils / 2, Camera.main.transform.position.z);

        GetRandomSkin();
        InitTiles();
        GameManager.instance.SetBoardManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 click = inputManager.getInputPosition();
        Debug.Log(click);
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
            int rnd = Random.Range(0, tileSkins.Count);
            currentTileSkin = tileSkins[rnd];
        }
        else currentTileSkin = preferedSkin;
    }
    #endregion

    /// <summary>
    /// Métodos de lógica de juego y comprobaciones del mismo.
    /// </summary>
    #region Logic Methods
    void TilePulsado()
    {

    }

    public void SetTilePulsado(int x, int y) {
        tiles[x, y].Pulsar();
        // Asumimos que es una jugada legal
        caminoTiles.Push(tiles[x, y]);
    }
    #endregion

}
