using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{

    public Button prefab;

    public int numberToCreate = 100;

    private void Start() {
        Populate();
    }

    void Populate() {
        Button aux;
        GameManager.instance.NuevoNivelSerializable(GameManager.instance.infoNivel.numNivelActual);
        for (int i = 1; i <= numberToCreate; i++) {
            aux = Instantiate(prefab, transform);
            aux.GetComponent<BotonNivel>().AsignarNivel(i);
        }
    }

}
