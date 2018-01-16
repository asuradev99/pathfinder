using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closed : MonoBehaviour {
	public int type = 1; 
	public int index;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (type == 1) {
			//GetComponent<SpriteRenderer>().color = Color.red;
		}

		if (name == "Oog") {
		    GetComponent<SpriteRenderer>().color = Color.white;
		}
		if (name == "Boogy") {
			GetComponent<SpriteRenderer>().color = Color.red;
		}
		if (name == "Googy"){
			GetComponent<SpriteRenderer> ().color = Color.yellow;
		}
		if (name == "destination") {
			GetComponent<SpriteRenderer> ().color = Color.cyan;
		}
	}
		
	void OnMouseDown (){
		name = "Wall";

	}
}
