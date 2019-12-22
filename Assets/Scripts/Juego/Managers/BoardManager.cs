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
    public GameObject prefabTile;

    [Tooltip("Array de ScriptableObjects para las skins.")]
    public List<TileSkin> tileSkins;

    //Atributos privados

    /// <summary>
    /// Array con los tiles
    /// </summary>
    private Tile[,] tiles;

    /// <summary>
    /// Array de booleanos de si está pulsado o no.
    /// </summary>
    private bool[,] boolTiles;

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
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
