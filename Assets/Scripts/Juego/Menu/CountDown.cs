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


    //timeLeft -= TimeManager.instance.CheckDate(); // Esto en el init
    //timeLeft -= Time.deltaTime; // Esto en el Update

    void ResetClock(float time)
    {
        TimeManager.instance.SaveDate();
        timeLeft = time;
        timeLeft -= TimeManager.instance.CheckDate();
    }


    public Text text;

    private void OnEnable()
    {
        StartTimer(timeLeft);
    }

    public void StartTimer(float from)
    {
        stop = false;
        timeLeft = from;
        Update();
        StartCoroutine(updateCoroutine());
    }

    public void StopTimer()
    {
        stop = true;
    }

    void Update()
    {
        if (stop) return;
        timeLeft -= Time.deltaTime;

        minutes = Mathf.Floor(timeLeft / 60);
        seconds = timeLeft % 60;
        if (seconds > 59) seconds = 59;
        if (minutes < 0)
        {
            stop = true;
            minutes = 0;
            seconds = 0;

            canvasJuego.ChallengeFallido();
        }
        //        fraction = (timeLeft * 100) % 100;
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