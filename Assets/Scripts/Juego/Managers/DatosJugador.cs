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

    public void AsignaNivel(int nivel) {
        if (!playedLevels.ContainsKey(nivel)) {
            playedLevels.Add(nivel, true);
        }
        else {
            playedLevels[nivel] = true;
        }
    }

    public int GetNumLevels(int topeInferior, int topeSuperior) {
        int infAux = -1;
        int supAux = 600;
        if (playedLevels.Count == 0) return 1;
        foreach (KeyValuePair<int, bool> entry in playedLevels) {
            if (infAux <= topeInferior && entry.Key >= topeInferior) {
                infAux = entry.Key;
            }
            if (entry.Key <= topeSuperior) {
                supAux = entry.Key;
            }
            else {
                break;
            } 
        }
        return supAux - infAux;
    }
}
