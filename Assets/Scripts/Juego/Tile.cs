using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script que controla la infraestructura de un Tile. 
 * Prefab. El prefab se marca automaticamente :D
 * TODO: Cambiar el Sprite de ConCamino según la skin, ya llegará.
*/
public class Tile : MonoBehaviour
{

    [Tooltip("Sprite para cuando no haya camino")]
    public Sprite pulsado;

    [Tooltip("Sprite para cuando haya camino")]
    public Sprite noPulsado;


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
            GetComponent<SpriteRenderer>().sprite = noPulsado;
    }

    /// <summary>
    /// Marca este Tile como tile principal
    /// Un tile inicial no podrá ser desmarcado, aunque se pulse sobre él.
    /// </summary>
    public void SetTileInicial()
    {
        _tileInicial = true;
        GetComponent<SpriteRenderer>().sprite = pulsado;
    }

    /// <summary>
    /// Informa al tile de que ha sido pulsado.
    /// Manipula el sprite del GameObject, y el bool interno.
    /// </summary>
    public void Pulsar()
    {
        _pulsado = true;
        GetComponent<SpriteRenderer>().sprite = pulsado;
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
            GetComponent<SpriteRenderer>().sprite = noPulsado;
        }
    }

    /// <summary>
    /// Devuelve si el tile está pulsado o no
    /// </summary>
    /// <returns>Estado del bool "Pulsado" interno</returns>
    public bool GetPulsado() { return _pulsado; }



}
