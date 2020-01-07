using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    DateTime currentDate;
    DateTime oldTime;

    private string saveLocation;
    public static TimeManager instance;
    
    // Start is called before the first frame update
    void Awake() {
        instance = this;
        saveLocation = "lastSavedDate1";
    }

    public float CheckDate() {
        // Guardamos el tiempo actual cuando empieza
        currentDate = System.DateTime.Now;
        string tempString = PlayerPrefs.GetString(saveLocation, "1");
        // Guardamos el tiempo antiguo de PlayerPrefs como long
        long tempLong = Convert.ToInt64(tempString);

        //Convertimos el antiguo de binario a DateTime
        DateTime oldTime = DateTime.FromBinary(tempLong);

        // Usamos la diferencia de tiempos y lo guardamos como timespan
        TimeSpan diferencia = currentDate.Subtract(oldTime);
        return (float)diferencia.TotalSeconds;
    }

    /// <summary>
    /// Guarda el tiempo actual, es necesario para comprobar la diferencia de tiempo
    /// </summary>
    public void SaveDate() {
        PlayerPrefs.SetString(saveLocation, System.DateTime.Now.ToBinary().ToString());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
