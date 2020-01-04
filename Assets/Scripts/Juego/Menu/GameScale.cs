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
		SetWidth();
		SetHeight();
	}

	private void SetWidth()
	{
		boardWidth = (int)Math.Round(Screen.width * 0.95f);
	}

	private void SetHeight()
	{
		boardHeight = (int)Math.Round((Screen.height - spriteArriba.rect.height - spriteAbajo.rect.height) * 0.90f);
	}

	public float GetWidth() {
		SetWidth();
		return boardWidth;
	}

	public float GetHeight() {
		SetHeight();
		return boardHeight;
	}

	public float CubosSpriteArriba(int pixelesPorUnidad) {
		return spriteArriba.rect.height / pixelesPorUnidad;
	}
	public float CubosSpriteAbajo(int pixelesPorUnidad) {
		return spriteAbajo.rect.height / pixelesPorUnidad;
	}
}
