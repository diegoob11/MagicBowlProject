using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreAnimationFinished : MonoBehaviour {




	public void FinishGame(){
		SceneManager.LoadScene ("MainMenu");

	}
}
