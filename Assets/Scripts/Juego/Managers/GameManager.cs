﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

    LectorTxt lectorTxt;
    BoardManager boardManager = null;
    InputManager inputManager = null;

    DatosJugador datosJugador;
    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        LoadPlayer();
       
        //TEMPORAL
        lectorTxt = GetComponent<LectorTxt>();
        lectorTxt.CargaTodosLosNiveles();

        LevelInfo prueba = lectorTxt.CargaNivel(1);
        Debug.Log("Lenght layout"  + prueba.layout.Length);
        Debug.Log("Lenght path" + prueba.path.Length);
        
    }

    public void addTest()
    {
        datosJugador.test = datosJugador.test+1;
        ProgressManager.Save(datosJugador);
        Debug.Log("Saving this: " + datosJugador.test);
    }

    private void LoadPlayer()
    {
        datosJugador = ProgressManager.Load();
        if(datosJugador == null)
        {
            datosJugador = new DatosJugador(0);
        }
        Debug.Log(datosJugador.test);
    }

    #region Set and gets
    public void SetBoardManager(BoardManager instance)
    {
        boardManager = instance;
    }

    public BoardManager GetBoardManager()
    {
      return boardManager;
    }

    public void SetInputManager(InputManager instance)
    {
        inputManager = instance;
    }

    public InputManager GetInputManager()
    {
        return inputManager;
    }
    #endregion
}
