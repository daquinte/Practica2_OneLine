using UnityEngine;

//Serialization
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Clase encargada de mantener el progreso del jugador. 
/// </summary>
public static class ProgressManager {


    static int customSalt;
    static private int baseSalt = 1234;
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

        datosJugador.hash = 0;  //primero se pone a 0
        datosJugador.salt = baseSalt + datosJugador._monedas;
        
        datosJugador.hash = Crypto(bf, datosJugador);

        //Serializamos data y lo guardamos en file
        bf.Serialize(file, datosJugador);

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
            int hashLeida = data.hash;
            int salLeida = data.salt;

            data.hash = 0;
            int miHash = Crypto(bf, data);
            int miSal = baseSalt + data._monedas;   //Si se han modificado de forma externa, la sal cambiará.
            file.Close();

            Debug.Log("mi Hash" + miHash);
            Debug.Log("mi Sal" + miSal);
            if (miHash == hashLeida && miSal == salLeida)
            {
                return data;
            }
            else { 
                return null;
            }
        }
        else return null;

    }

    private static int Crypto (BinaryFormatter bf, DatosJugador datos)
    {
        MemoryStream memoryStream = new MemoryStream();

        bf.Serialize(memoryStream, datos);

        // This resets the memory stream position for the following read operation
        memoryStream.Seek(0, SeekOrigin.Begin);

        // Get the bytes
        var bytes = new byte[memoryStream.Length];
        return memoryStream.Read(bytes, 0, (int)memoryStream.Length).GetHashCode();

    }
}
