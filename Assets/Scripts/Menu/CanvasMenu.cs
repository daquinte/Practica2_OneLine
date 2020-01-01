using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMenu : MonoBehaviour {


	public Text textoMonedas;
	public Text textoMedallas;

	public Text [] textoProgresoNiveles;

	[Tooltip("Panel de Challenge.")]
	public GameObject challengePanel;


	[Tooltip("Panel de Daily Login.")]
	public GameObject loginPanel;

	/// <summary>
	/// Numero de niveles de dificultad que vamos a tener.
	/// No tiene en cuenta los Challenge, que van aparte.
	/// </summary>
	private int nDificultades;

	/// <summary>
	/// Copia de solo lectura de los datos del jugador
	/// </summary>
	private DatosJugador datosJugador;


	private const int precioChallenge = 25; 


	// Use this for initialization
	void Start () {
		nDificultades = 5;
		textoProgresoNiveles = new Text[nDificultades];

		datosJugador = GameManager.instance.GetDatosJugador();
		textoMonedas.text = datosJugador._monedas.ToString();
	}
	
	// Update is called once per frame
	void Update () {

		//Actualiza en el menú, porque puedes obtener monedas del login diario
		ActualizaMonedas();
	}

	/// <summary>
	/// Callbacks del panel de challenge
	/// </summary>
	#region Challenge Callbacks

	public void ShowChallengePanel() { challengePanel.SetActive(true); }
	public void HideChallengePanel() { challengePanel.SetActive(false); }

	/// <summary>
	/// Empieza un desafío. Tiene en cuenta si el usuario ha pagado o no.
	/// </summary>
	/// <param name="paid">¿Has pagado por este anuncio?</param>
	public void StartChallenge(bool paid) {
		if (paid)
		{
			GameManager.instance.RestaMonedas(precioChallenge);
		}
		//???????
	}
	#endregion

	/// <summary>
	/// Callbacks del panel de Login diario
	/// </summary>
	#region Login Callbacks

	public void ShowLoginPanel() { loginPanel.SetActive(true); }
	public void HideLoginPanel() { loginPanel.SetActive(false); }

	#endregion


	/// <summary>
	/// Analiza cuantos niveles ha resuelto el jugador 
	/// en cada dificultad del juego, y actualiza el texto
	/// </summary>
	private void AnalizaProgreso()
	{

	}

	/// <summary>
	/// Actualiza las monedas del menú
	/// </summary>
	private void ActualizaMonedas()
	{
		textoMonedas.text = GameManager.instance.GetDatosJugador()._monedas.ToString();
	}
}
