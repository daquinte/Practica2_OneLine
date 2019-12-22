using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}

	private Vector3 position;

	public Vector3 getInputPosition()
	{
		return position;
	}
}
 