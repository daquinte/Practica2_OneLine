using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanPlayChallenge : MonoBehaviour
{

    public Timer timer;
    public Text timeText;
    public GameObject [] gameObjectsToDeactivate;
    public CanvasMenu canvasMenu;

    public float time;

    private float minutes;
    private float seconds;

    void OnEnable() {
        InitTimer();
        foreach(GameObject uiMember in gameObjectsToDeactivate) {
            uiMember.SetActive(false);
        }
    }
    void OnDisable() {
        StopTimer();
        foreach (GameObject uiMember in gameObjectsToDeactivate)
        {
            uiMember.SetActive(true);
        }
    }

    public void InitTimer() {
        if (GameManager.instance.GetDatosJugador().timerChallenge > 0) {
            time = GameManager.instance.GetDatosJugador().timerChallenge;
            timer.InitClock(time);
        }
        else {
            timer.ResetClock(time);
        }
    }

    // Update is called once per frame
    void Update() {
        Debug.Log("Challenge" + timer.timeLeft);
        GameManager.instance.GetDatosJugador().timerChallenge = timer.timeLeft;
        minutes = Mathf.Floor(timer.timeLeft / 60);
        seconds = Mathf.Round(timer.timeLeft % 60);
        timeText.text = string.Format("{00:0}:{1:00}", minutes, seconds);
        if (minutes < 0) {
            StopTimer();
            minutes = 0;
            seconds = 0;
            canvasMenu.PosibleJugarChallenge(this.gameObject, gameObjectsToDeactivate);
        }
    }

    private void StopTimer() {
        timer.ResetClock(time);
    }
}
