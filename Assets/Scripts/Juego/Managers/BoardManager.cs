using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {


    public GameObject prefabTile;

    //Atributos privados

    /// <summary>
    /// Array con los tiles
    /// </summary>
    private List<Tile> tiles;

    /// <summary>
    /// Array de booleanos de si está pulsado o no.
    /// </summary>
    private List<bool> boolTiles;

    /// <summary>
    /// El camino de las pistas, que creo que está en ints.
    /// </summary>
    private List<int> caminoPistas;


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
}
