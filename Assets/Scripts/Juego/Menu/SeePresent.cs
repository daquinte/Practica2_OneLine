using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePresent : MonoBehaviour
{

    public Timer timer;
    public GameObject regaloLogin;

    public float time;
    public bool regaloUsado = false;
    
    public void Start() {
        if (GameManager.instance.GetDatosJugador().timerDaily > 0) {
            ContinueTimer();
        }
        else {
            regaloUsado = false;
            regaloLogin.SetActive(true);
        }
    }
    public void ContinueTimer()
    {
        time = GameManager.instance.GetDatosJugador().timerDaily;
        timer.InitClock(time);
        regaloUsado = true;
    }
    public void InitTimer() {
        regaloUsado = true;
        regaloLogin.SetActive(false);
        timer.ResetClock(time);
    }

    // Update is called once per frame
    void Update() {
        if (regaloUsado) {
            GameManager.instance.GetDatosJugador().timerDaily = timer.timeLeft;
            if (timer.timeLeft < 0) {
                StopTimer();
                regaloLogin.SetActive(true);
            }
        }
    }

    private void StopTimer() {
        timer.ResetClock(0);
        regaloUsado = false;
    }
}
