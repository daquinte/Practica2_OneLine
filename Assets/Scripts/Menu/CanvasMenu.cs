using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMenu : MonoBehaviour {


	public Text textoMonedas;
	public Text textoMedallas;

	public Text [] textoProgresoNiveles;

	public RewardedAdsButton duplicaLogin;
	public RewardedAdsButton freeChallenge;

	[Tooltip("Panel de Challenge.")]
	public GameObject challengePanel;


	[Tooltip("Panel de Daily Login.")]
	public GameObject loginPanel;

	[Tooltip("Panel de Tiempo de Challenge.")]
	public GameObject panelTiempoChallenge;

	/// <summary>
	/// Clase encargada de gestionar si se ve o no el login diario.
	/// </summary>
	private SeePresent seePresent;

	/// <summary>
	/// Numero de niveles de dificultad que vamos a tener.
	/// No tiene en cuenta los Challenge, que van aparte.
	/// </summary>
	private int nDificultades;

	/// <summary>
	/// Copia de solo lectura de los datos del jugador
	/// </summary>
	private DatosJugador datosJugador;


	


	// Use this for initialization
	void Start () {
		nDificultades = 5;

		duplicaLogin.SetCallbackRecompensa(DuplicaRecompensaDiaria);
		freeChallenge.SetCallbackRecompensa(StartFreeChallenge);

		datosJugador = GameManager.instance.GetDatosJugador();
		textoMonedas.text = datosJugador._monedas.ToString();
		seePresent = GetComponent<SeePresent>();
		AnalizaProgreso();
	}
	
	// Update is called once per frame
	void Update () {

		//Actualiza en el menú, porque puedes obtener monedas del login diario
		ActualizaMonedas();
	}


	public void GoToSeleccionNiveles(int dificultad)
	{
		GameManager.instance.CargaSeleccionNivel(dificultad);
	}


	/// <summary>
	/// Callbacks del panel de challenge
	/// </summary>
	#region Challenge Callbacks

	public void ShowChallengePanel() { challengePanel.SetActive(true); }
	

	public void HideChallengePanel() { challengePanel.SetActive(false); }

	/// <summary>
	/// Empieza un desafío pagando la cuota de entrada
	/// </summary>
	public void StartPaidChallenge()
	{ 
		
		GameManager.instance.OnChallengeStart(false);
	}

	public void StartFreeChallenge()
	{
		GameManager.instance.OnChallengeStart(true);
	}
	#endregion

	/// <summary>
	/// Callbacks del panel de Login diario
	/// </summary>
	#region Login Callbacks

	public void ShowLoginPanel()		  { loginPanel.SetActive(true); }
	public void SumaRecompensaDiaria()    { GameManager.instance.OnDailyLoginReward(false); }
	public void DuplicaRecompensaDiaria() { 
		GameManager.instance.OnDailyLoginReward(true);
		loginPanel.SetActive(false);
	}
	public void HideLoginPanel() { 
		loginPanel.SetActive(false);
		seePresent.InitTimer();
	}

	#endregion


	/// <summary>
	/// Analiza cuantos niveles ha resuelto el jugador 
	/// en cada dificultad del juego, y actualiza el texto
	/// </summary>
	private void AnalizaProgreso()
	{
		int [] npd = GameManager.instance.GetNivelesPorDificultad();
		for(int i = 0; i < 5; i++)
		{
			string numtostr = npd[i].ToString();
			textoProgresoNiveles[i].text = numtostr + "/100";
		}
	}

	/// <summary>
	/// Actualiza las monedas del menú
	/// </summary>
	private void ActualizaMonedas()
	{
		textoMonedas.text = GameManager.instance.GetDatosJugador()._monedas.ToString();
	}

	public void PosibleJugarChallenge(GameObject deactivateObjects, GameObject[] activateObjects, Button challengeButton)
	{
		deactivateObjects.SetActive(false);
		foreach (GameObject uiMember in activateObjects) {
			uiMember.SetActive(true);
		}
		challengeButton.interactable = true;
	}
}
