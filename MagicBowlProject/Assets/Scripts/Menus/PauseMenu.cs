using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour {
	
	//public GameObject canvas;
	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false); //note: the object has to be active in the viewport
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

	public void clickExit(){
		//exits game
		SceneManager.LoadScene("MainMenu");
	}
}
