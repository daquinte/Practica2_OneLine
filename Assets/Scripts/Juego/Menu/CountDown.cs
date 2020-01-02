using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase de CountDown para el timer de Challenge.
/// </summary>
public class CountDown : MonoBehaviour
{

    public float timeLeft = 300.0f;

    private float minutes;
    private float seconds;

    public bool stop = true;


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

            //TODO: CALLBACK DE "SE ACABÓ EL TIEMPO"
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