﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonNivel : MonoBehaviour
{

    [HideInInspector] public int nivel = 0;
    public Sprite nivelBloqueado;

    public Text numeroNivel;
    public Image star;

    public void AsignarNivel(int level) {
        //Comprobar si este nivel esta desbloqueado
        //En funcion de esto, cambiar imagen y stats
        if (!GameManager.instance.GetDatosJugador().playedLevels.ContainsKey(level)) {
            GetComponent<Button>().enabled = false; //No carga nivel 
            GetComponent<Image>().sprite = nivelBloqueado;
            numeroNivel.enabled = false;
            star.enabled = false;
        }
        else {
            nivel = level;
            numeroNivel.enabled = true;
            numeroNivel.text = nivel.ToString();
        }
    }

    public void CargarNivel() {
        GameManager.instance.CargaEscenaJuego(nivel, false);
    }
}
