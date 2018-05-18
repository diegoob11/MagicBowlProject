using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{

    public NetworkManager manager;
    public GameObject btnPause;
    public GameObject menuPause;
    public bool server;

    // Use this for initialization

    void Start()
    {
        menuPause.SetActive(false); 
		//note: the object has to be active in the viewport
		//manager = m.GetComponent<CustomNetworkManager>().manager;
		//COJER EL MANAGER DEL SCRIPT DE CUSTOMNETWORKMANAGER
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void mostraMenuPausa()
    { 
		//important to be public to see it in the onclick() from PauseButton
        btnPause.SetActive(false);
        menuPause.SetActive(true);
    }

    public void clickContinue()
    {
        menuPause.SetActive(false);
        btnPause.SetActive(true);
    }

    public void OnDestroyMatch(bool success, string extendedInfo)
    {
        if (server)
        {
            Debug.Log("SERVER PLAYER DESTROYED");
            NetworkManager.singleton.StopHost();
            NetworkManager.singleton.StopMatchMaker();
            NetworkManager.Shutdown();
            Destroy(GameObject.Find("NetworkManager"));
            NetworkTransport.Shutdown();
        }
        else
        {
            Debug.Log("CLIENT PLAYER DESTROYED");
            NetworkManager.singleton.StopMatchMaker();
            NetworkManager.Shutdown();
            Destroy(GameObject.Find("NetworkManager"));
        }
    }
    public void clickExit(NetworkManager m)
    {
        manager = m.GetComponent<CustomNetworkManager>().manager;
        manager.matchMaker.DestroyMatch(manager.matches[0].networkId, 0, OnDestroyMatch);
        Destroy(GameObject.Find("mc"));
        SceneManager.LoadScene("MainMenu");
    }



}
