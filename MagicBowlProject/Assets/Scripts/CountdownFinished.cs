using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownFinished : MonoBehaviour {

	public bool countdown_acabat = false; //accedit a Temps.cs


	public void CountdownIsFinished(){ //event in Countdown.anim

		countdown_acabat = true;
	}
}
