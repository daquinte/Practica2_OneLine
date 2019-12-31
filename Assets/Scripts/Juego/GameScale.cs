using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScale : MonoBehaviour {

	[Tooltip("Borde de arriba")]
	public RectTransform spriteArriba;        //Sprite del camino seguido

	[Tooltip("Borde de abajo")]
	public RectTransform spriteAbajo;

	private float boardWidth;
	private float boardHeight;

	// Use this for initialization
	void Start () {
		boardWidth = (int)Math.Round(Screen.width * 0.95f);
		boardHeight = (int)Math.Round((Screen.height - spriteArriba.rect.height - spriteAbajo.rect.height) * 0.95f);
	}

	public float GetWidth() {
		return boardWidth;
	}

	public float GetHeight() {
		return boardHeight;
	}
}
