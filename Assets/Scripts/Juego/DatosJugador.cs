using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase serializable con los datos del jugador
/// </summary>
[System.Serializable]
public class DatosJugador {
    public int test;
    public string hash;


    /// <summary>
    /// Constructora vacia, necesaria para el hash
    /// </summary>
    public DatosJugador() { }

    //Constructora del objeto serializable con datos por defecto
    public DatosJugador(int t)
    {
        test = t;
    }
}
