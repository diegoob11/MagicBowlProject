using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CharacterCreation : MonoBehaviour {

	public List<GameObject> models;
	//Model index
	public int selectionIndex;
	void Start () {
		selectionIndex = 0;
		models = new List<GameObject>();
		foreach(Transform t in transform){
			models.Add(t.gameObject);
			t.gameObject.SetActive(false);
		}
		models[selectionIndex].SetActive(true);
	}
	public void Select(int index){
		if(index == selectionIndex){
			return;
		}
		if(index < 0 || index > models.Count){
			return;
		}
		models[selectionIndex].SetActive(false);
		selectionIndex = index;
		models[selectionIndex].SetActive(true);
	}

	public void findMatch(){
		DontDestroyOnLoad(GameObject.Find("mc"));
		
		SceneManager.LoadScene("move_goal");
		models[0].SetActive(false);
		models[1].SetActive(false);

		
		
	}
	public void BackToMain(){
		SceneManager.LoadScene("MainMenu");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			models[selectionIndex].transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X") * -3, 0.0f), Space.Self);
		}
	}
}
