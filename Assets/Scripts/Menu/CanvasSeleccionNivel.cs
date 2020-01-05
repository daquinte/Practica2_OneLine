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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void JuegaNivel()
    {
        //PILLO EL NUMERO DEL NIVEL Y SE LO DOY AL GM
        GameManager.instance.CargaEscenaJuego(Random.Range(1, 500), false);
    }
    public void GoBackToMenu() { GameManager.instance.CargaEscenaTitulo(); }
    private void AnalizaDificultad()
    {
        textoDificultad.text = GameManager.instance.infoNivel.tipoDificultadActual;    
    }
}
