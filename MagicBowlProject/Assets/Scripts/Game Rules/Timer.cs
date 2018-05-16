using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Timer : NetworkBehaviour
{
    //nota: els valors es tornen a reiniciar cada vegada que es carrega l'escena
    private CountdownFinished countEnd; //script
    private bool animationStart;
    private bool timeCounter = false;
    public GameObject countdown;
    private GameObject score;
    //public GameObject directional_light;
    public GameObject timerCanvas;

    private float minutes;
    private float seconds;
    [SyncVar]
    public float timeRemaining;

    [SyncVar]
    public bool allowPlayerMovementTimer; //accessed by PlayerController.cs

    private PlayerController playerController;

    void Start()
    {
        score = GameObject.Find("score");
        countEnd = countdown.GetComponent<CountdownFinished>();
        timeRemaining = 180;
        playerController = GetComponent<PlayerController>();
        if (isLocalPlayer)
        {
            timerCanvas.SetActive(true);
        }
    }
    void Update()
    {
        //startGame es troba a CustomNetworkManager.cs. Inicia animacio countdown
        if (playerController.gameStarted == 1 && !animationStart)
        {
            //animacio 3,2,1,start https://www.youtube.com/watch?v=ZEP3lxsA-FY
            countdown.SetActive(true); //starts animation
            animationStart = true;
        }

        if (animationStart)
        {
            //has to be accessed now because before was inactive
            if (countEnd.countdownEnd)
            {
                timeCounter = true;
                countdown.SetActive(false);
            }
        }

        if (isServer && timeCounter)
        {
            timeRemaining -= Time.deltaTime;
            allowPlayerMovementTimer = true;
        }

        if (timeCounter)
        {
            //canvia el temps
            float minutes = Mathf.Floor(timeRemaining / 60);
            float seconds = Mathf.Floor(timeRemaining % 60);
            string minutes2 = minutes.ToString("00");
            string seconds2 = seconds.ToString("00");
            //timerCanvas.transform.GetChild(0).GetComponent<Text>().text = minutes2 + ":" + seconds2;

            //acaba partida si passen 3 minuts
            if (timeRemaining <= 0)
            {
                allowPlayerMovementTimer = false;
                timeCounter = false;
                //directional_light.GetComponent<Light> ().intensity = 0.1f;
                score.GetComponent<Animator>().Play("ShowResults");
                timerCanvas.SetActive(false);

                //game is ended by ScoreAnimationFinished.cs
                //reinicia els valors "private" degut a que canvia d'escena
            }
        }
    }

    /*
    public GameObject timerCanvas;

    private float minutes;
    private float seconds;

    [SyncVar]
    public float timeRemaining;

    private PlayerController playerController;

    void Start()
    {

        minutes = 3;
        seconds = 0;
        timeRemaining = 180;

        playerController = GetComponent<PlayerController>();
        if (isLocalPlayer)
        {
            timerCanvas.SetActive(true);
        }
    }
    void Update()
    {
        if (isServer && playerController.gameStarted == 1)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
                FinishGame();
        }

        minutes = Mathf.Floor(timeRemaining / 60);
        seconds = Mathf.Floor(timeRemaining % 60);
        timerCanvas.transform.GetChild(0).GetComponent<Text>().text = minutes.ToString("00")
             + ":" + seconds.ToString("00");
    }

    public void FinishGame()
    {

    }
    */
}