using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase que funciona como manager de un nivel. 
/// Controla la logica de los tiles del nivel
/// Gestiona el input en la aplicación
/// </summary>
public class BoardManager : MonoBehaviour {
    //Atributos publicos
    [Tooltip("Prefab of a Tile")]
    public Tile prefabTile;

    [Tooltip("Array de ScriptableObjects para las skins.")]
    public List<TileSkin> tileSkins;

    public InputManager inputManager;


    //Atributos privados

    /// <summary>
    /// Array con los tiles
    /// </summary>
    private Tile[,] tiles;

    /// <summary>
    /// El camino de las pistas, que creo que está en ints.
    /// </summary>
    private List<int> caminoPistas;


    private int nFils; //numero filas del tablero
    private int nCols; //numero columnas del tablero
    //Se supone que el juego tiene dos anchos de tile distintos (enunciado)
    //Los de Beginner son más anchos, creo.
    private int _anchoTile;     
    private int _altoTile;

	// Use this for initialization
	void Start () {


        tiles = new Tile[3,1];
        Camera.main.transform.position = new Vector3(tiles.GetLength(0) / 2, tiles.GetLength(1) / 2, Camera.main.transform.position.z);
        for (int filas = 0; filas < tiles.GetLength(1); filas++)
        {
            for (int cols = 0; cols < tiles.GetLength(0); cols++)
            {
                Tile tile = Instantiate(prefabTile, new Vector3(cols, filas, 0), Quaternion.identity, transform);
                tile.gameObject.name = "Bloque" + cols + filas;

                if (cols == 0 && filas == 0) tile.SetTileInicial();
                tiles[cols, filas] = tile;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 click = inputManager.getInputPosition();
        
    }

    
    private bool EsAdyacente()
    {
        //Comprobamos que sean adyacentes
        /*
        int verticalDist = Mathf.Abs(tile1.y_ - tile2.y_);
        int horizontalDist = Mathf.Abs(tile1.x_ - tile2.x_);

        return (verticalDist + horizontalDist == 1);
        */
        return false;
    }
}
