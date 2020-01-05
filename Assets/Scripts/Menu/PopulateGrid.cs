using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{

    public Button prefab;

    private void Start() {
        Populate();
    }

    void Populate() {
        Button aux;
        GameManager.instance.NuevoNivelSerializable(GameManager.instance.infoNivel.numNivelActual);
        int [] margenes = GameManager.instance.GetNumberToCreate();
        for (int i = margenes[0]; i <= margenes[1]; i++) {
            aux = Instantiate(prefab, transform);
            aux.GetComponent<BotonNivel>().AsignarNivel(i);
        }
    }

}
