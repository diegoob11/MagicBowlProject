using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //important per les imatges!
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class TimeController : MonoBehaviour
{
    //aquest script va dins de Time
    //nota: els valors es tornen a reiniciar cada vegada que es carrega l'escena
    private float initMinutes;
    private float initSseconds;
    private bool restart;
    private float timeRestarted;
    private CustomNetworkManager customNetwork;  //script
    private CountdownFinished countEnd; //script
    private bool animationInProgress;
    private bool animationFinished;
    public bool allowPlayerMovement; //accessed by PlayerController.cs
    private bool timeCounter;
    public GameObject networkManager; //rep NetworkManager per llegir startGame de l'script
    public GameObject countdown;
    public GameObject score;
    public GameObject directional_light;

    void Start()
    {
        customNetwork = networkManager.GetComponent<CustomNetworkManager>();
        countEnd = countdown.GetComponent<CountdownFinished>();

        restart = true;

        animationInProgress = false;
        animationFinished = false;
        allowPlayerMovement = false;
        timeCounter = false;

        timeRestarted = 0;
        initSseconds = 59;
        initMinutes = 02; 

        countdown.SetActive(false);
    }

    // Update is called once per frame. Nota: si un script crida l'update, no crida cap més event. L'update només es cridat en l'escena de l'script
    void Update()
    {
        if (restart && customNetwork.startGame && !animationInProgress)
        { //startGame es troba a CustomNetworkManager.cs. Inicia animacio countdown
            //animacio 3,2,1,start https://www.youtube.com/watch?v=ZEP3lxsA-FY
            countdown.SetActive(true); //starts animation
            animationInProgress = true;
        }

        if (animationInProgress)
        {
            print("end: " + countEnd.countdownEnd);
            if (countEnd.countdownEnd)
            { //has to be accessed now because before was inactive
                animationFinished = true;
                animationInProgress = false;
            }
        }
        
        if (animationFinished)
        {
            restart = false;
            animationFinished = false;
            timeCounter = true;
            timeRestarted = Time.time;
            allowPlayerMovement = true;
        }

        if (timeCounter)
        {
            //canvia el temps
            float timer = Time.time;
            float minutes = initMinutes - Mathf.Floor((timer - timeRestarted) / 60);
            float seconds = initSseconds - Mathf.Floor((timer - timeRestarted) % 60);
            string minutes2 = minutes.ToString("00");
            string seconds2 = seconds.ToString("00");
            var temps = this.GetComponent<Text>();
            temps.text = minutes2 + ":" + seconds2;

            //acaba partida si passen 3 minuts
            if (minutes2 == "00" && seconds2 == "00")
            { //canviar per 00 00
                allowPlayerMovement = false;
                timeCounter = false;
                directional_light.GetComponent<Light>().intensity = 0.1f;
                score.GetComponent<Animator>().Play("show_results");
                //game is ended by ScoreAnimationFinished.cs
                //reinicia els valors "private" degut a que canvia d'escena
            }
        }
    }
}
