﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cursor : MonoBehaviour {

	//private Sprite fingerSprite;
	private SpriteRenderer renderer;
	public InputManager inputManager;

	void Awake()
	{
		Debug.Log("Antes");
		renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (inputManager.getInputInfo().pulsado)
		{
			transform.position = inputManager.getInputInfo().position;
			if (!renderer.enabled)
				renderer.enabled = true;
		}
		else if (renderer.enabled)
				renderer.enabled = false;
	}

	public void SetSprite(Sprite fingerSprite)
	{
		renderer.sprite = fingerSprite;
	}
}
