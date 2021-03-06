﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

/// <summary>
/// Lee los niveles de fichero
/// Devuelve la información del nivel solicitado
/// </summary>
public class LectorNiveles : MonoBehaviour
{
    private Dictionary<int, InfoNivel> _niveles;

    //Carga un nivel y guarda su informacion en "level info"
    private void CargaNivel(JSONNode level)
    {
        try
        {
            //Inicializando
            InfoNivel levelInfo = new InfoNivel();
            levelInfo.layout = new string[level["layout"].Count];
            levelInfo.path = new int[level["path"].Count, level["path"][0].Count];

            for (int i = 0; i < levelInfo.layout.Length; i++)
            {
                levelInfo.layout[i] = level["layout"][i];
                //Debug.Log("layout[i]" + levelInfo.layout[i]);

            }

            for (int i = 0; i < levelInfo.path.GetLength(0); i++)
            {
                for (int j = 0; j < levelInfo.path.GetLength(1); j++)
                {
                    levelInfo.path[i, j] = level["path"][i][j];
                    //Debug.Log( "path["+i +","+j+"]"+levelInfo.path[i, j]);
                }
            }

            _niveles.Add(level["index"], levelInfo);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
        }
    }


    /// <summary>
    /// Cargamos todos los niveles
    /// </summary>
    public void CargaTodosLosNiveles()
    {
        _niveles = new Dictionary<int, InfoNivel>();
        string json;
#if !UNITY_EDITOR && UNITY_ANDROID
        var reader = new WWW("jar:file://" + Application.dataPath + "!/assets/niveles.json");
        while (!reader.isDone) { }
        json = reader.text;
#else
        json = File.ReadAllText(Application.streamingAssetsPath + "/niveles.json");
#endif
        JSONNode niveles = JSON.Parse(json);

        //Itera sobre la etiqueta "levels"
        for (int i = 0; i < niveles["levels"].Count; i++) {
            CargaNivel(niveles["levels"][i]);
        }
    }

    /// <summary>
    /// Devuelve la información del nivel indicado
    /// </summary>
    /// <param name="nNivel">indice del nivel</param>
    /// <returns></returns>
    public InfoNivel CargaNivel(int nNivel)
    {
        return _niveles[nNivel];
    }

    /// <summary>
    /// Devuelve el numero de niveles actual
    /// </summary>
    /// <returns></returns>
    public int GetNumNiveles()
    {
        return _niveles.Count;
    }
}

/// <summary>
/// Clase en la que almacenamos lo que se va leyendo de los JSON
/// </summary>
public class InfoNivel
{
    public string[] layout { get; set; }
    public int[,] path { get; set; }
}
