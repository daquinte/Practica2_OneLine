using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase serializable con los datos del jugador
/// </summary>
[System.Serializable]
public class DatosJugador {
    public int _monedas;
    public int _medallas;

    public Dictionary<int, bool> playedLevels;
    public bool _noAds;


    //Serializacion y encriptado
    public int hash;
    public int salt;


    /// <summary>
    /// Constructora vacia, necesaria para el hash
    /// </summary>
    public DatosJugador() { }

    //Constructora del objeto serializable con datos por defecto
    public DatosJugador(int monedas, int medallas)
    {
        playedLevels = new Dictionary<int, bool>();
        _monedas = monedas;
        _medallas = medallas;
        _noAds = false;
    }
}
