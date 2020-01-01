using UnityEngine;

//Serialization
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Clase encargada de mantener el progreso del jugador. 
/// </summary>
public static class ProgressManager { 

    /// <summary>
    /// Guarda el estado del juego en un .dat
    /// crea el archivo y lo guarda.
    /// Application.persistentDataPath es independiente del dispositivo
    /// 
    /// </summary>
    public static void Save(DatosJugador datosJugador)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/ProgresoJugador.dat");

        DatosJugador datosAux = new DatosJugador();
        string objHash = datosAux.GetHashCode().ToString();
        datosAux = datosJugador;
        datosAux.hash = objHash;
        Debug.Log(datosAux.hash);

        //Serializamos data y lo guardamos en file
        bf.Serialize(file, datosAux);
        //Cerramos file
        file.Close();
    }

    /// <summary>
    /// Carga el estado del archivo .dat previamente creado
    /// si este existiera.
    /// </summary>
    public static DatosJugador Load()
    {
        Debug.Log(Application.persistentDataPath);
        //Comprobamos si el archivo existe antes de abrirlo
        if (File.Exists(Application.persistentDataPath + "/ProgresoJugador.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ProgresoJugador.dat", FileMode.Open);

            //Tenemos que castear la deserializacion que ha leido a Datos Jugador
            DatosJugador data = bf.Deserialize(file) as DatosJugador;
            file.Close();

            return data;
        }
        else return null;

    }
}
