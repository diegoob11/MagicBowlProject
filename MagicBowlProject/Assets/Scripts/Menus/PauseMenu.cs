using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class PauseMenu : MonoBehaviour {
	
	public NetworkManager manager;
	
	// Use this for initialization

	void Start () {
		this.gameObject.SetActive(false); //note: the object has to be active in the viewport
		//manager = m.GetComponent<CustomNetworkManager>().manager;
		//COJER EL MANAGER DEL SCRIPT DE CUSTOMNETWORKMANAGER
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void mostraMenuPausa(){ //important to be public to see it in the onclick() from PauseButton
		if (!this.gameObject.activeInHierarchy) {
			
			this.gameObject.SetActive (true);

		} else {
			
			this.gameObject.SetActive (false);
		}
	} 

	public void clickContinue(){
		this.gameObject.SetActive (false); 
	}

	public void clickExit(NetworkManager m){
		//exits game
		manager = m.GetComponent<CustomNetworkManager>().manager;
		//manager.matchMaker.ListMatches(0, 1, "", true, 0, 0, manager.OnMatchList);
		Debug.Log(Network.connections.Length);
		//FALTA DECLARAR BIEN EL ONDESTRYMATCH
		//HACER TODOS LOS DETROYS DE ABAJO:
		//public static void OnDestroyMatch(BasicResponse response) { Debug.Log("Match Destroyed" + response.ToString()); NetworkManager.singleton.StopHost(); NetworkManager.singleton.StopMatchMaker(); NetworkManager.Shutdown(); Destroy(GameObject.Find("Network Manager")); NetworkTransport.Shutdown(); }
		manager.matchMaker.DestroyMatch(manager.matches[0].networkId, 0, m.GetComponent<CustomNetworkManager>().OnDestroyMatch);
		Debug.Log(Network.connections.Length);
		//Network.CloseConnection(Network.connections[0], true);
		Destroy(GameObject.Find("mc"));
		Destroy(GameObject.Find("NetworkManager"));
		SceneManager.LoadScene("MainMenu");
	}



}
