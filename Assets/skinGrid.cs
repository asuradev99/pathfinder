using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skinGrid : MonoBehaviour {
	public Sprite mySprite;
	// Use this for initialization
	void Start () {
		
		if (Random.Range (0, 1) < 0.001) {
			
			this.GetComponent<SpriteRenderer> ().sprite = mySprite;
		}
	}
	

}
