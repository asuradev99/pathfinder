using UnityEngine;
using System.Collections;

public static class Constants
{
	public const int STARTNODE = 0;
	public const int WALLNODE = 2;
	public const int NODE = 1;
}
public class test : MonoBehaviour {
	public int destination;
	public GameObject myNode;
	public GameObject wallNode;
	public GameObject startNode;
	public GameObject refSprite;
	int cX = 0;
	int cY = 0;
	int junk = 0; 
	int  comparator = 0;
	int comparator2 = 0;
	int left, right, up, down;
	public int gridUnits = 10;//grid 
	public bool running = false; 
	string s;
	float time = 0;
	public float seconds;
	public float minutes;
	bool timeron = false;
	void Start () {
		//run main function
		//StartCoroutine(Spec());
		Spec();
	}
	//main function in an enumerator func so we can yield to frame count. 
	public all_cells refscript; 
	void Spec(){
		//initialize reference script for Arraylist containing cell's status
		refscript = refSprite.GetComponent<all_cells> ();
		Closed refvar = refSprite.GetComponent<Closed> ();
		//creates the starting node and adds it to the arraylist
		refscript.cellref.Add (Instantiate (startNode, new Vector3 (this.transform.position.x, this.transform.position.y, 0), Quaternion.identity));
		refscript.cell_types.Add (Constants.STARTNODE);
		refscript.cell_id.Add (0, new Vector2 (cX, cY));
		refscript.cell_num.Add (0, cX * gridUnits + cY);
		cX += 1;
		transform.position = new Vector3 (transform.position.x + 6f, transform.position.y, 0);

		//creates the cells and the obstacles in grid. 
		for (int i = 0; i < (gridUnits * gridUnits) - 1; i++) {
			Vector3 myPos = new Vector3 (this.transform.position.x, this.transform.position.y, 0);
			if (Random.Range (1, 10) > 2) {
				//clones regular cell and adds its atatus to the list
				refscript.cellref.Add (Instantiate (myNode, myPos, Quaternion.identity));
				refscript.cell_types.Add (Constants.NODE);


			} else {
				//clones wall cell and adds its status to the list
				refscript.cellref.Add (Instantiate (wallNode, myPos, Quaternion.identity));
				refscript.cell_types.Add (Constants.WALLNODE);
			}

			//adds cell position to list
			refscript.cell_id.Add (i + 1, new Vector2 (cX, cY));
			refscript.cell_num.Add (i + 1, (int)cY * gridUnits + cX);
			cX += 1;

			//moves 6 pixels after each clone 
			transform.position = new Vector3 (transform.position.x + 6f, transform.position.y, 0);


			//if my x-position is greater than 54, start new row
			if (this.transform.position.x > (gridUnits - 1) * 6) {
				cY += 1;
				cX = 0;
				transform.position = new Vector3 (0, transform.position.y - 6f, 0); 
			}


		}
		destination = Random.Range(0, gridUnits * gridUnits - 1);
		refscript.cellref [destination].name = "destination";

		//BFS ();
		//StartCoroutine(BestFirst());
		//StartCoroutine(djikstra());
		//StartCoroutine(AStar());
		//StartCoroutine(BestFirst());
	}
		
	

	// Update is called once per frame

	IEnumerator BFS(){
		//breadth-first-search 

		running = true;
		//initializing status list
		for (int i = 0; i < (gridUnits * gridUnits); i++) {
			refscript.cell_status.Add(false);
			refscript.parents.Add (-1);
		}
		//adds first node to queue
		refscript.BreadthCoords.Enqueue (0);
		bool done = false;
		while (!done) {
			timeron = true;
			//deletes first element from queue
			Debug.Log(seconds);
			refscript.currentNode = refscript.BreadthCoords.Dequeue ();
			//refscript.cellref [1].GetComponent<SpriteRenderer> ().color = Color.red; 
			done = (refscript.currentNode == destination);
			refscript.cellref [refscript.currentNode].name = "Boogy";
			refscript.cell_status [refscript.currentNode] = true;

			//checks and adds neighbors
			if (refscript.currentNode % gridUnits != 0) {
				left = refscript.currentNode - 1; 
				if (Constants.WALLNODE != (int) refscript.cell_types[left] && 
					(refscript.cell_status[left] != true) ) {
					//Debug.Log ("LEFT: Inserting" + left + "," + refscript.cell_status[left]);
					refscript.BreadthCoords.Enqueue (left);
					refscript.cell_status [left] = true;
					refscript.parents [left] = refscript.currentNode;
				}
			}

			if (refscript.currentNode % gridUnits != gridUnits - 1) {
				right = refscript.currentNode + 1;
				if (Constants.WALLNODE != (int) refscript.cell_types[right] && 
					(refscript.cell_status[right] != true) ) {
					//Debug.Log ("RIGHT: Inserting" + right + "," + refscript.cell_status[right]);
					refscript.BreadthCoords.Enqueue (right);
					refscript.cell_status [right] = true;
					refscript.parents [right] = refscript.currentNode;
				}
			}

			if (refscript.currentNode >= gridUnits) {
				up = refscript.currentNode - gridUnits;
				if (Constants.WALLNODE != (int) refscript.cell_types[up] && 
					(refscript.cell_status[up] != true) ) {
					//Debug.Log ("UP: Inserting" + up + "," + refscript.cell_status[up]);
					refscript.BreadthCoords.Enqueue (up);
					refscript.cell_status [up] = true;
					refscript.parents [up] = refscript.currentNode;
				}
			}

			if (refscript.currentNode < gridUnits * (gridUnits - 1)) {
				down = refscript.currentNode + gridUnits;
				if (Constants.WALLNODE != (int) refscript.cell_types[down] && 
					(refscript.cell_status[down]!= true)) {
					//Debug.Log ("DOWN: Inserting" + down + "," + refscript.cell_status[down]);
					refscript.BreadthCoords.Enqueue (down);
					refscript.cell_status [down] = true;
					refscript.parents [down] = refscript.currentNode;
				}
			}
			yield return new WaitForSeconds(0.05f) ;

		}
		Debug.Log ("| Time in seconds| " + minutes.ToString () + ":" + seconds.ToString ()); 
		timeron = false;
		trailRender(destination);
		running = false;
	}
	IEnumerator BestFirst(){
		//finds destination(random)
		//int destination = Random.Range(0, gridUnits * gridUnits - 1); 


		for (int i = 0; i < (gridUnits * gridUnits); i++) {
			refscript.cell_status.Add(false);
			refscript.parents.Add (-1);
		}

		bool done = false;
		//adds first node to queue
		refscript.g.Add(heuristic(0,destination), 0);
		while (!done) {
			//deletes first element from list
			refscript.currentNode = refscript.g.Values[0];
			refscript.g.RemoveAt (0);
			done = (refscript.currentNode == destination);
			refscript.cellref [refscript.currentNode].name = "Boogy";
			refscript.cell_status [refscript.currentNode] = true;

			//checks and adds neighbors
			if (refscript.currentNode % gridUnits != 0) {
				left = refscript.currentNode - 1; 
				if (Constants.WALLNODE != (int) refscript.cell_types[left] && 
					(refscript.cell_status[left] != true) ) {
					//Debug.Log ("LEFT: Inserting" + left + "," + refscript.cell_status[left]);
					refscript.g.Add (heuristic(left, destination),left);
					refscript.parents [left] = refscript.currentNode;
				}
			}

			if (refscript.currentNode % gridUnits != gridUnits - 1) {
				right = refscript.currentNode + 1;
				if (Constants.WALLNODE != (int) refscript.cell_types[right] && 
					(refscript.cell_status[right] != true) ) {
					//Debug.Log ("RIGHT: Inserting" + right + "," + refscript.cell_status[right]);
					refscript.g.Add(heuristic(right, destination), right);
					refscript.cell_status [right] = true;
					refscript.parents [right] = refscript.currentNode;
				}
			}

			if (refscript.currentNode >= gridUnits) {
				up = refscript.currentNode - gridUnits;
				if (Constants.WALLNODE != (int) refscript.cell_types[up] && 
					(refscript.cell_status[up] != true) ) {
					//Debug.Log ("UP: Inserting" + up + "," + refscript.cell_status[up]);
					refscript.g.Add(heuristic(up, destination), up);
					refscript.cell_status [up] = true;
					refscript.parents [up] = refscript.currentNode;
				}
			}

			if (refscript.currentNode < gridUnits * (gridUnits - 1)) {
				down = refscript.currentNode + gridUnits;
				if (Constants.WALLNODE != (int) refscript.cell_types[down] && 
					(refscript.cell_status[down]!= true)) {
					//Debug.Log ("DOWN: Inserting" + down + "," + refscript.cell_status[down]);
					refscript.g.Add (heuristic(down, destination), down);
					refscript.cell_status [down] = true;
					refscript.parents [down] = refscript.currentNode;
				}
			}
			yield return new WaitForSeconds(0.01f) ;

		}
		trailRender(destination);

	}
	IEnumerator djikstra(){
		//finds destination(random)
		//int destination = Random.Range(0, gridUnits * gridUnits - 1); 
		//while (Constants.WALLNODE == (int)refscript.cell_types [destination]) {
		//	destination = Random.Range(0, gridUnits * gridUnits - 1); 
		//}
		//refscript.cellref [destination].name = "destination";
		for (int i = 0; i < (gridUnits * gridUnits); i++) {
			refscript.cell_status.Add(false);
			refscript.parents.Add (-1);
		}

		bool done = false;
		//adds first node to queue
		refscript.g.Add(heuristic(0,destination), 0);
		while (!done) {
			//deletes first element from list
			refscript.currentNode = refscript.g.Values[0];
			refscript.currentDist = refscript.g.Keys [0];
			refscript.g.RemoveAt (0);
			done = (refscript.currentNode == destination);
			refscript.cellref [refscript.currentNode].name = "Boogy";
			refscript.cell_status [refscript.currentNode] = true;

			//checks and adds neighbors
			if (refscript.currentNode % gridUnits != 0) {
				left = refscript.currentNode - 1; 
				if (Constants.WALLNODE != (int) refscript.cell_types[left] && 
					(refscript.cell_status[left] != true) ) {
					//Debug.Log ("LEFT: Inserting" + left + "," + refscript.cell_status[left]);
					refscript.g.Add (refscript.currentDist + 1,left);
					refscript.parents [left] = refscript.currentNode;
				}
			}

			if (refscript.currentNode % gridUnits != gridUnits - 1) {
				right = refscript.currentNode + 1;
				if (Constants.WALLNODE != (int) refscript.cell_types[right] && 
					(refscript.cell_status[right] != true) ) {
					//Debug.Log ("RIGHT: Inserting" + right + "," + refscript.cell_status[right]);
					refscript.g.Add(refscript.currentDist + 1, right);
					refscript.cell_status [right] = true;
					refscript.parents [right] = refscript.currentNode;
				}
			}

			if (refscript.currentNode >= gridUnits) {
				up = refscript.currentNode - gridUnits;
				if (Constants.WALLNODE != (int) refscript.cell_types[up] && 
					(refscript.cell_status[up] != true) ) {
					//Debug.Log ("UP: Inserting" + up + "," + refscript.cell_status[up]);
					refscript.g.Add(refscript.currentDist + 1, up);
					refscript.cell_status [up] = true;
					refscript.parents [up] = refscript.currentNode;
				}
			}

			if (refscript.currentNode < gridUnits * (gridUnits - 1)) {
				down = refscript.currentNode + gridUnits;
				if (Constants.WALLNODE != (int) refscript.cell_types[down] && 
					(refscript.cell_status[down]!= true)) {
					//Debug.Log ("DOWN: Inserting" + down + "," + refscript.cell_status[down]);
					refscript.g.Add (refscript.currentDist + 1, down);
					refscript.cell_status [down] = true;
					refscript.parents [down] = refscript.currentNode;
				}
			}
			yield return new WaitForSeconds(0.001f) ;

		}
		trailRender(destination);
	}
	IEnumerator AStar(){
		//finds destination(random)
		for (int i = 0; i < (gridUnits * gridUnits); i++) {
			refscript.cell_status.Add(false);
			refscript.parents.Add (-1);
		}

		bool done = false;
		//adds first node to queue
		refscript.g.Add(heuristic(0,destination), 0);
		while (!done) {
			//deletes first element from list
			refscript.currentNode = refscript.g.Values[0];
			refscript.currentDist = refscript.g.Keys [0];
			refscript.g.RemoveAt (0);
			done = (refscript.currentNode == destination);
			if (refscript.cellref [refscript.currentNode].name != "destination") {
				refscript.cellref [refscript.currentNode].name = "Boogy";
			}
			refscript.cell_status [refscript.currentNode] = true;

			//checks and adds neighbors
			if (refscript.currentNode % gridUnits != 0) {
				left = refscript.currentNode - 1; 
				if (Constants.WALLNODE != (int) refscript.cell_types[left] && 
					(refscript.cell_status[left] != true) ) {
					//Debug.Log ("LEFT: Inserting" + left + "," + refscript.cell_status[left]);
					refscript.g.Add (refscript.currentDist + heuristic(left, destination),left);
					refscript.parents [left] = refscript.currentNode;
				}
			}

			if (refscript.currentNode % gridUnits != gridUnits - 1) {
				right = refscript.currentNode + 1;
				if (Constants.WALLNODE != (int) refscript.cell_types[right] && 
					(refscript.cell_status[right] != true) ) {
					//Debug.Log ("RIGHT: Inserting" + right + "," + refscript.cell_status[right]);
					refscript.g.Add(refscript.currentDist + heuristic(right, destination), right);
					refscript.cell_status [right] = true;
					refscript.parents [right] = refscript.currentNode;
				}
			}

			if (refscript.currentNode >= gridUnits) {
				up = refscript.currentNode - gridUnits;
				if (Constants.WALLNODE != (int) refscript.cell_types[up] && 
					(refscript.cell_status[up] != true) ) {
					//Debug.Log ("UP: Inserting" + up + "," + refscript.cell_status[up]);
					refscript.g.Add(refscript.currentDist + heuristic(up, destination), up);
					refscript.cell_status [up] = true;
					refscript.parents [up] = refscript.currentNode;
				}
			}

			if (refscript.currentNode < gridUnits * (gridUnits - 1)) {
				down = refscript.currentNode + gridUnits;
				if (Constants.WALLNODE != (int) refscript.cell_types[down] && 
					(refscript.cell_status[down]!= true)) {
					//Debug.Log ("DOWN: Inserting" + down + "," + refscript.cell_status[down]);
					refscript.g.Add (refscript.currentDist + heuristic(down, destination), down);
					refscript.cell_status [down] = true;
					refscript.parents [down] = refscript.currentNode;
				}
			}
			yield return new WaitForSeconds(0.00001f) ;

		}
		//while (refscript.parents [destination ] != 0) {
			//refscript.cellref [refscript.parents [destination]].name = "Googy";
			//destination = refscript.parents [destination];
		//}
		trailRender(destination);
	}
	int heuristic(int c1, int c2){
		int x1 = c1 / 10;
		int y1 = c1 % 10;
		int x2 = c2 / 10;
		int y2 = c2 % 10;
		return(Mathf.Abs (x1 - x2) + Mathf.Abs (y1 - y2));
	}
	void trailRender(int comparator){
		while (refscript.parents [comparator ] != 0) {
			if (refscript.cellref [refscript.parents [comparator]].name != "destination") {
				refscript.cellref [refscript.parents [comparator]].name = "Googy";
			}
			comparator = refscript.parents [comparator];
		}
	}
	void clear(){
		
		refscript.cell_status.Clear ();
		refscript.parents.Clear ();
		refscript.BreadthCoords.Clear ();
		refscript.g.Clear ();

		for (int i = 0; i < refscript.cellref.Count; i++) {
			if (refscript.cellref [i].name == "Boogy" || refscript.cellref [i].name == "Googy" ) {
				refscript.cellref [i].name = "Oog";
			}
		}
		refscript.cellref [destination].name = "destination";

	}

		void startTick(){
			
		time += Time.deltaTime;
		seconds = time % 60;
		minutes = Mathf.Floor (time / 60);

		}
		void stopTick(){
			
			
			seconds = 0;
			minutes = 0;
			time = 0;
		}
		void Update(){
		if (timeron) {
			startTick ();
		} else {
			stopTick ();
		}
		}
}
