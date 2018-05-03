using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreAnimationFinished : MonoBehaviour {

	//public RuntimeAnimatorController score2;



	void Start(){
		//GetComponent<Animator>().runtimeAnimatorController = score2 as RuntimeAnimatorController;
	}

	public void FinishGame(){
		SceneManager.LoadScene ("MainMenu");

	}
}
