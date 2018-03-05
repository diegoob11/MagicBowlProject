using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //important per les imatges!
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class Temps : MonoBehaviour
{

    //aquest script va dins de Time
    //nota: els valors es tornen a reiniciar cada vegada que es carrega l'escena

    private float mins_inicials = 02; //2
    private float segons_inicials = 59;//59
    private bool reinicia = true;
    private float timerreiniciat = 0;
    private CustomNetworkManager custom_net;  //script
    private CountdownFinished count_fin; //script
    private bool animacio_en_proces = false;
    private bool animacio_acabada = false;
    public bool allow_move_player = false; //accessed by PlayerController.cs
    private bool comptatemps = false;
    public GameObject net_manag; //rep NetworkManager per llegir startGame de l'script
    public GameObject countdown;
    public GameObject score;
    public GameObject directional_light;

    void Start()
    {
        //startGameNet = net_manag.GetComponent<CustomNetworkManager> ().startGame;
        custom_net = net_manag.GetComponent<CustomNetworkManager>();
        count_fin = countdown.GetComponent<CountdownFinished>();

        countdown.SetActive(false);
    }

    // Update is called once per frame. Nota: si un script crida l'update, no crida cap més event. L'update només es cridat en l'escena de l'script
    void Update()
    {
        if (reinicia && custom_net.startGame && !animacio_en_proces)
        { //startGame es troba a CustomNetworkManager.cs. Inicia animacio countdown

            //animacio 3,2,1,start https://www.youtube.com/watch?v=ZEP3lxsA-FY
            countdown.SetActive(true); //starts animation
            animacio_en_proces = true;
        }

        if (animacio_en_proces)
        {
            if (count_fin.countdown_acabat)
            {//has to be accessed now because before was inactive
                animacio_acabada = true;
                animacio_en_proces = false;
            }
        }
        if (animacio_acabada)
        {
            reinicia = false;
            animacio_acabada = false;
            comptatemps = true;
            timerreiniciat = Time.time;
            allow_move_player = true;
        }

        if (comptatemps)
        {
            //canvia el temps
            float timer = Time.time;
            float minutes = mins_inicials - Mathf.Floor((timer - timerreiniciat) / 60);
            float seconds = segons_inicials - Mathf.Floor((timer - timerreiniciat) % 60);
            string minutes2 = minutes.ToString("00");
            string seconds2 = seconds.ToString("00");
            var temps = this.GetComponent<Text>();
            temps.text = minutes2 + ":" + seconds2;

            //acaba partida si passen 3 minuts
            if (minutes2 == "00" && seconds2 == "00")
            { //canviar per 00 00
                allow_move_player = false;
                comptatemps = false;
                directional_light.GetComponent<Light>().intensity = 0.1f;
                score.GetComponent<Animator>().Play("show_results");
                //game is ended by ScoreAnimationFinished.cs
                //reinicia els valors "private" degut a que canvia d'escena
            }
        }
    }
}
