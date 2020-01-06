using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase de CountDown para el timer de Challenge.
/// </summary>
public class CountDown : MonoBehaviour
{

    public CanvasJuego canvasJuego;

    public float timeLeft = 300.0f;

    private float minutes;
    private float seconds;

    public bool stop = true;

    public Text text;

    private void OnEnable() {
        StartTimer();
    }

    public void StartTimer() {
        stop = false;
        TimeManager.instance.SaveDate();
        timeLeft -= TimeManager.instance.CheckDate();
        Update();
        StartCoroutine(updateCoroutine());
    }

    public void StopTimer() {
        stop = true;
    }

    void Update() {
        if (stop) return;
        timeLeft -= Time.deltaTime;
        Debug.Log("Tiempo: " + timeLeft);
        minutes = Mathf.Floor(timeLeft / 60);
        seconds = Mathf.Round(timeLeft % 60);
        if (minutes < 0) {
            stop = true;
            minutes = 0;
            seconds = 0;
            canvasJuego.ChallengeFallido();
        }
    }

    void ResetClock(float time)
    {
        TimeManager.instance.SaveDate();
        timeLeft = time;
        timeLeft -= TimeManager.instance.CheckDate();
    }

    private IEnumerator updateCoroutine()
    {
        while (!stop)
        {
            text.text = string.Format("{00:0}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(0.2f);
        }
    }
}