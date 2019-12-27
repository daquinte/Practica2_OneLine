using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase serializable con los datos del jugador
/// </summary>
[System.Serializable]
public class DatosJugador {
    public int test;
    public bool _noAds;
    public int _monedas;
    public string hash;


    /// <summary>
    /// Constructora vacia, necesaria para el hash
    /// </summary>
    public DatosJugador() { }

    //Constructora del objeto serializable con datos por defecto
    public DatosJugador(int t)
    {
        _monedas = t;
        _noAds = false;
    }
}
