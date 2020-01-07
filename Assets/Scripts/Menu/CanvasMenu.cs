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


	// Use this for initialization
	void Start () {
		seePresent = GetComponent<SeePresent>();

		duplicaLogin.SetCallbackRecompensa(DuplicaRecompensaDiaria);
		freeChallenge.SetCallbackRecompensa(StartFreeChallenge);

		GameManager.instance.SetNumDificultades(textoProgresoNiveles.Length);

		ActualizaCanvas();
		AnalizaProgreso();

	}
	
	// Update is called once per frame
	void Update () {

		//Actualiza en el menú, porque puedes obtener monedas del login diario
		ActualizaCanvas();
	}

	/// <summary>
	/// Informa al GameManager de que el usuario quiere cerrar la aplicación
	/// </summary>
	public void CierraApp()
	{
		GameManager.instance.CierraJuego();
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
		HideLoginPanel();
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
		for(int i = 0; i < textoProgresoNiveles.Length; i++)
		{
			string numtostr = npd[i].ToString();
			textoProgresoNiveles[i].text = numtostr + "/100";
		}
	}

	/// <summary>
	/// Actualiza las monedas del menú
	/// </summary>
	private void ActualizaCanvas()
	{
		textoMonedas.text = GameManager.instance.GetDatosJugador()._monedas.ToString();
		textoMedallas.text = GameManager.instance.GetDatosJugador()._medallas.ToString();
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
