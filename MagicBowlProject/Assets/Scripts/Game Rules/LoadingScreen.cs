using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class LoadingScreen : MonoBehaviour {

	public GameObject network;
	private CustomNetworkManager net;
	public GameObject mainCamera;
	public GameObject loadingCamera;
	public GameObject HUDCanvas;
	public GameObject loadingCanvas;
	
	void Start(){
		net = network.GetComponent<CustomNetworkManager> ();


	}


	void Update () {
		
		if (net.startGame) {
			
			//shows hudcanvas
			loadingCanvas.SetActive(false);
			CanvasGroup group = HUDCanvas.GetComponent<CanvasGroup> ();
			group.alpha = 1;
			group.interactable = true;

			//changes camera
			loadingCamera.SetActive(false);
			mainCamera.SetActive (true);





		}

	}


}
