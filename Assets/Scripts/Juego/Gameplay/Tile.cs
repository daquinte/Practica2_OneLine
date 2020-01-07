using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Constrolamos la estructura de cada tile particular que componen el tablero.
/// Cada tile se encarga de saber si ha sido pulsado, y de pintar su camino según el input del usuario.
/// </summary>
public class Tile : MonoBehaviour
{
    [Tooltip("Skin de tile no pulsado.")]
    public Sprite spriteNoPulsado;                      //Sprite de tile no pulsado

    [Tooltip("Guia de camino")]
    public SpriteRenderer spriteDireccionCamino;        //Sprite del camino seguido

    [Tooltip("Pista. El sprite se escoge dinamicamente.")]
    public SpriteRenderer pistaSprite;                  //Sprite de Pista




    private Sprite spritePulsado;                       //Sprite de tile pulsado

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
        pistaSprite.sprite = tileSkin.spriteTilePista;
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

    /// <summary>
    /// Dibuja el camino, de pista o normal, en función de la información del Tile previo.
    /// En base a esa información, se añade el camino correspondiente como hijo y
    /// lo modifica adecuadamente.
    /// </summary>
    /// <param name="pista">¿Es pista o no?</param>
    /// <param name="tile">Tile del que procede</param>
    /// <param name="posicion">posición del anterior tile</param>
    /// <param name="sentido">sentido del camino</param>
    public void MarcarCamino(bool pista, Tile tile, Vector3 posicion, Vector3 sentido)
    {
        SpriteRenderer spriteR = (pista) ? pistaSprite : spriteDireccionCamino;
        SpriteRenderer child = Instantiate(spriteR, tile.transform);
        // Es muy importante hacer esto porque si no, empieza en el origen de coordenadas
        child.transform.position = tile.transform.position + posicion;
        child.transform.eulerAngles = sentido;
    }

    /// <summary>
    /// Desmarca el camino que tuviera este tile. 
    /// Sólo borramos el camino normal, nunca borraremos el camino formado por pistas.
    /// </summary>
    public void DesmarcarCamino()
    {
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<SpriteRenderer>().sprite != pistaSprite.sprite) {
                child.parent = null;
                Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    /// Devuelve si el tile está pulsado o no
    /// </summary>
    /// <returns>Estado del bool "Pulsado" interno</returns>
    public bool GetPulsado() { return _pulsado; }

    /// <summary>
    /// Pregunta si se ha pulsado dentro del Tile
    /// </summary>
    void OnMouseDown() {
        GameManager.instance.GetBoardManager().coordsDentroMatriz((int)this.transform.position.x, Mathf.Abs((int)this.transform.position.y));
    }



}
