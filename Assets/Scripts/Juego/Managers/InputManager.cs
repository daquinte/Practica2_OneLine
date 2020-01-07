using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que gestiona el input de la aplicacion
/// </summary>
public class InputManager : MonoBehaviour {

	/// <summary>
	/// Contiene información del input para ser consultado
	/// </summary>
	public struct inputInfo
	{
		public Vector3 position;
		public bool pulsado;
	}

	private inputInfo infoInput;

	// Use this for initialization
	void Start () {
		infoInput.pulsado = false;
		GameManager.instance.SetInputManager(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0))
		{
			infoInput.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			infoInput.position.z = 0;
			infoInput.pulsado = true;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			infoInput.pulsado = false;
		}
	}

	/// <summary>
	/// Devuelve la información del input
	/// </summary>
	/// <returns>Struct con información</returns>
	public inputInfo getInputInfo()
	{
		return infoInput;
	}
}
