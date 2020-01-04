﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScale : MonoBehaviour
{

	[Tooltip("Borde de arriba")]
	public RectTransform spriteArriba;        //Sprite del camino seguido

	[Tooltip("Borde de abajo")]
	public RectTransform spriteAbajo;

	private float boardWidth;
	private float boardHeight;

	private float margenLateral = 0.80f;
	private float margenSuperior = 0.95f;

	// Use this for initialization
	void Start()
	{
		SetWidth();
		SetHeight();
	}

	private void SetWidth()
	{
		boardWidth = (int)Math.Round(Screen.width * margenLateral);
	}

	private void SetHeight()
	{
		boardHeight = (int)Math.Round((Screen.height - spriteArriba.rect.height - spriteAbajo.rect.height) * margenSuperior);
	}

	public float GetWidth()
	{
		SetWidth();
		return boardWidth;
	}

	public float GetHeight()
	{
		SetHeight();
		return boardHeight;
	}
	public float GetMargenLateral()
	{
		return margenLateral;
	}

	public float GetMargenSuperior()
	{
		return margenSuperior;
	}

	public float CubosSpriteArriba(int pixelesPorUnidad)
	{
		return spriteArriba.rect.height / pixelesPorUnidad;
	}
	public float CubosSpriteAbajo(int pixelesPorUnidad)
	{
		return spriteAbajo.rect.height / pixelesPorUnidad;
	}

	public float CubosLaterales(int pixelesPorUnidad)
	{
		float anchoTablero = GetWidth() / pixelesPorUnidad;
		float diferenciaP = Math.Abs(7.0f * pixelesPorUnidad - GetWidth());
		float diferenciaC = diferenciaP / pixelesPorUnidad;
		if (diferenciaC < 1.0f)
		{
			return 1.0f;
		}
		return 0;

	}
}
