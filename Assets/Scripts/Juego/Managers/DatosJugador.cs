using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase serializable con los datos del jugador
/// </summary>
[System.Serializable]
public class DatosJugador {
    public int _monedas;                            //Monedas del jugador
    public int _medallas;                           //Medallas del jugador

    public Dictionary<int, bool> playedLevels;      //Diccionario con los niveles que el jugador puede jugar

    //Serializacion y encriptado    
    public int hash;                                //Clave hash
    public int salt;                                //Sal que le aplicaremos a la clave hash

    public float timerChallenge;                    //Tiempo restante al desafío
    public float timerDaily;                        //Tiempo restante al login diario

    //Constructora del objeto serializable con datos por defecto
    //Se llamará cuando se cree un nuevo jugador en el dispositivo
    public DatosJugador(int monedas, int medallas)
    {
        playedLevels = new Dictionary<int, bool>();
        _monedas = monedas;
        _medallas = medallas;
        timerChallenge = 0;
        timerDaily = 0;
    }

    /// <summary>
    /// Asigna el nivel por parámetro como jugable en el diccionario
    /// </summary>
    /// <param name="nivel">indice del nivel jugable</param>
    public void AsignaNivel(int nivel) {
        if (!playedLevels.ContainsKey(nivel)) {
            playedLevels.Add(nivel, true);
        }
        else {
            playedLevels[nivel] = true;
        }
    }

    /// <summary>
    /// Obtiene el número de niveles desbloqueados por dificultad
    /// En base a los topes pasados por parámetro
    /// </summary>
    /// <param name="topeInferior">Tope inferior</param>
    /// <param name="topeSuperior">Tope superior</param>
    /// <returns></returns>
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
