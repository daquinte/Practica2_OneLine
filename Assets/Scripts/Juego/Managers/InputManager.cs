using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

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

	public inputInfo getInputInfo()
	{
		return infoInput;
	}
}
