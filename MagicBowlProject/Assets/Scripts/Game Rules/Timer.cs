using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Timer : NetworkBehaviour
{
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
}