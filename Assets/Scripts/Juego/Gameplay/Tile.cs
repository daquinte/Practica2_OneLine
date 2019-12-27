﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script que controla la infraestructura de un Tile. 
 * Prefab. El prefab se marca automaticamente :D
 * TODO: Cambiar el Sprite de ConCamino según la skin, ya llegará.
*/
public class Tile : MonoBehaviour
{
    [Tooltip("Skin de tile no pulsado.")]
    public Sprite spriteNoPulsado;      //Sprite de tile no pulsado

    [Tooltip("Guia de camino")]
    public GameObject spriteDireccionCamino;      //Sprite de tile no pulsado

    private Sprite spritePulsado;        //Sprite de tile pulsado


    /// <summary>
    /// Bool interno que controla si el tile está pulsado o no
    /// </summary>
    private bool _pulsado = false;

    /// <summary>
    /// Si es el tile inicial, va a estar siempre marcado
    /// Se añadirá al camino y nunca deberá ser retirado aunque se pulse en él
    /// </summary>
    private bool _tileInicial = false;


    private void Start()
    {
        if(!_tileInicial)
            GetComponent<SpriteRenderer>().sprite = spriteNoPulsado;
    }

    /// <summary>
    /// Marca este Tile como tile principal
    /// Un tile inicial no podrá ser desmarcado, aunque se pulse sobre él.
    /// </summary>
    public void SetTileInicial()
    {
        _tileInicial = true;
        _pulsado = true;
        GetComponent<SpriteRenderer>().sprite = spritePulsado;
    }

    public void SetTileSkin(TileSkin tileSkin)
    {
        spritePulsado = tileSkin.spriteTilePulsado;
    }

    /// <summary>
    /// Informa al tile de que ha sido pulsado.
    /// Manipula el sprite del GameObject, y el bool interno.
    /// </summary>
    public void Pulsar()
    {
        _pulsado = true;
        GetComponent<SpriteRenderer>().sprite = spritePulsado;
    }

    /// <summary>
    /// Informa al tile de que ha sido despulsado.
    /// Manipula el sprite del GameObject, y el bool interno.
    /// Solo cambiará el sprite si *no* es el primero.
    /// </summary>
    public void Despulsar()
    {
        if (!_tileInicial)
        {
            _pulsado = false;
            GetComponent<SpriteRenderer>().sprite = spriteNoPulsado;
        }
    }
    public void DesmarcarCamino()
    {
        if (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.parent = null;
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Devuelve si el tile está pulsado o no
    /// </summary>
    /// <returns>Estado del bool "Pulsado" interno</returns>
    public bool GetPulsado() { return _pulsado; }

    void OnMouseDown()
    {
        GameManager.instance.GetBoardManager().SetTilePulsado((int)this.transform.position.x, (int)this.transform.position.y);
    }

    public void marcarCamino(Tile tile, Vector3 posicion, Vector3 sentido)
    {
        GameObject child = Instantiate(spriteDireccionCamino, tile.transform);
        // Es muy importante hacer esto porque si no, empieza en el origen de coordenadas
        child.transform.position = tile.transform.position + posicion;
        child.transform.eulerAngles = sentido;
    }

}