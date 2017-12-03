using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class all_cells : MonoBehaviour {
	public List<GameObject> cellref = new List <GameObject>(); 
	public ArrayList cell_types = new ArrayList ();
	public Hashtable cell_id = new Hashtable();
	public Hashtable cell_num = new Hashtable();
	public List<bool> cell_status = new List <bool> ();
	public Queue<int> BreadthCoords = new Queue<int> ();
	public int currentNode = 0; 
	public int tX;
	public int tY;
	// Use this for initialization
	void Start () {
		





	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
