using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Queremos que se puedan crear desde el editor
[CreateAssetMenu]

/// <summary>
/// Clase scriptable con la información necesaria para una skin
/// </summary>
public class TileSkin : ScriptableObject {

    public Sprite spriteTilePulsado;            //Sprite al que cambia cuando está pulsado.
    public Sprite spriteTilePista;              //Sprite de la pista, en función del color del tile.

    public Sprite spriteDedo;                   //Sprite para el dedo.
	
}
