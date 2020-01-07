using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]


/// <summary>
/// Clase encargada del cursor que sigue al dedo.
/// </summary>
public class Cursor : MonoBehaviour {

	
	private SpriteRenderer spriteRenderer;			//Renderer para el sprite que vamos a usar.

	void Awake() {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;
	}

	// Update is called once per frame
	void Update() {
		if (GameManager.instance.GetInputManager().getInputInfo().pulsado)
		{
			transform.position = GameManager.instance.GetInputManager().getInputInfo().position;
			if (!spriteRenderer.enabled)
				spriteRenderer.enabled = true;
		}
		else if (spriteRenderer.enabled)
			spriteRenderer.enabled = false;
	}

	/// <summary>
	/// Asignamos al cursor un sprite para el dedo
	/// </summary>
	/// <param name="fingerSprite">Sprite que va a usar este componente</param>
	public void SetSprite(Sprite fingerSprite)
	{
		spriteRenderer.sprite = fingerSprite;
	}
}
