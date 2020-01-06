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

    public float timerChallenge;
    public float timerDaily;

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
        timerChallenge = 0;
        timerDaily = 0;
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
        int count = 0;
        if (playedLevels.Count == 0) return 1;
        foreach (KeyValuePair<int, bool> entry in playedLevels) {
            if (entry.Key >= topeInferior && entry.Key <= topeSuperior) {
                count++;
            }
        }
        return count;
    }
}
