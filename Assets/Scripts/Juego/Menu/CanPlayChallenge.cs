using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanPlayChallenge : MonoBehaviour
{

    public Timer timer;
    public Text timeText;
    public GameObject[] gameObjectsToDeactivate;
    public CanvasMenu canvasMenu;
    public Button challengeButton;

    public float time;

    private float minutes;
    private float seconds;

    public bool actualiza = false;


    void Start() {
        
        if (GameManager.instance.GetDatosJugador().timerChallenge > 0) {
            InitTimer();
        }
        else if (GameManager.instance.infoNivel.isChallenge) {
            actualiza = true;
            timer.ResetClock(time);
            DisableObjects();
        }
        else {
            actualiza = false;
            StopTimer();
            canvasMenu.PosibleJugarChallenge(this.gameObject, gameObjectsToDeactivate, challengeButton);
        }
    }

    public void InitTimer() {
        actualiza = true;
        time = GameManager.instance.GetDatosJugador().timerChallenge;
        timer.InitClock(time);
        DisableObjects();
    }

    // Update is called once per frame
    void Update() {
        if (!actualiza) return;
        Debug.Log("Challenge: " + timer.timeLeft);
        GameManager.instance.GetDatosJugador().timerChallenge = timer.timeLeft;
        minutes = Mathf.Floor(timer.timeLeft / 60);
        seconds = Mathf.Round(timer.timeLeft % 60);
        if (minutes < 0)
        {
            StopTimer();
            minutes = 0;
            seconds = 0;
            canvasMenu.PosibleJugarChallenge(this.gameObject, gameObjectsToDeactivate, challengeButton);
        }
        else
        {
            timeText.text = string.Format("{00:0}:{1:00}", minutes, seconds);
        }
    }

    private void StopTimer()
    {
        actualiza = false;
        timer.ResetClock(0);
    }

    private void DisableObjects() {
        challengeButton.interactable = false;
        foreach (GameObject uiMember in gameObjectsToDeactivate) {
            uiMember.SetActive(false);
        }
    }
}
