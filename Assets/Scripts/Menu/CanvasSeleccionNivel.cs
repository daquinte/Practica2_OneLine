using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSeleccionNivel : MonoBehaviour
{
    public Text textoDificultad;

    // Start is called before the first frame update
    void Start()
    {
        AnalizaDificultad();
    }

    public void GoBackToMenu() { GameManager.instance.CargaEscenaTitulo(); }
    private void AnalizaDificultad()
    {
        textoDificultad.text = GameManager.instance.infoNivel.tipoDificultadActual;    
    }
}
