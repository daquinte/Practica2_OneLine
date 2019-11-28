using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script que controla la infraestructura de un Tile. 
 * Prefab. El prefab se marca automaticamente :D
 * TODO: Cambiar el Sprite de ConCamino según la skin, ya llegará.
*/
public class Tile : MonoBehaviour {

    [Tooltip ("Sprite para cuando no haya camino")]
    public SpriteRenderer SinCamino;

    [Tooltip("Sprite para cuando haya camino")]
    public SpriteRenderer ConCamino;

    private bool _pulsado = false;

    // Use this for initialization
    void Start () {
		
	}

    /// <summary>
    /// Manipula el sprite del GameObject.
    /// </summary>
    /// <param name="value"></param>
    public void SetPulsado(bool value) { _pulsado = value; }
    public bool GetPulsado() { return _pulsado; }

}
