using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase de CountDown para el timer de Challenge.
/// </summary>
public class CountDown : MonoBehaviour
{

    public CanvasJuego canvasJuego;

    public Timer timer;
    public float time;

    private float minutes;
    private float seconds;

    public bool stop = true;

    public Text text;

    private void OnEnable() {
        StartTimer();
    }

    public void StartTimer() {
        stop = false;
        timer.InitClock(time);
    }

    public void StopTimer() {
        stop = true;
        timer.ResetClock(time);
    }

    void Update() {
        if (stop) return;
        minutes = Mathf.Floor(timer.timeLeft / 60);
        seconds = Mathf.Round(timer.timeLeft % 60);
        text.text = string.Format("{00:0}:{1:00}", minutes, seconds);
        if (minutes < 0) {
            stop = true;
            minutes = 0;
            seconds = 0;
            canvasJuego.ChallengeFallido();
        }
    }
}