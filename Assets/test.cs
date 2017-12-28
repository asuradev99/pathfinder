using UnityEngine;
using System.Collections;

public static class Constants
{
	public const int STARTNODE = 0;
	public const int WALLNODE = 2;
	public const int NODE = 1;
}
public class test : MonoBehaviour {
	
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
	void Start () {
		//run main function
		StartCoroutine(Spec());
	}
	//main function in an enumerator func so we can yield to frame count. 
	IEnumerator Spec(){
		//initialize reference script for Arraylist containing cell's status
		all_cells refscript = refSprite.GetComponent<all_cells> ();
		Closed refvar = refSprite.GetComponent<Closed> ();
		//creates the starting node and adds it to the arraylist
		refscript.cellref.Add(Instantiate (startNode, new Vector3 (this.transform.position.x, this.transform.position.y, 0), Quaternion.identity));
		refscript.cell_types.Add (Constants.STARTNODE);
		refscript.cell_id.Add (0, new Vector2 (cX, cY));
		refscript.cell_num.Add (0, cX * gridUnits + cY);
		cX += 1;
		transform.position = new Vector3 (transform.position.x + 6f, transform.position.y, 0);

		//creates the cells and the obstacles in grid. 
		for (int i = 0; i < (gridUnits * gridUnits)-1; i++) {
			Vector3 myPos = new Vector3 (this.transform.position.x, this.transform.position.y, 0);
			if (Random.Range (1, 10) > 2) {
				//clones regular cell and adds its atatus to the list
				refscript.cellref.Add(Instantiate (myNode, myPos, Quaternion.identity));
				refscript.cell_types.Add(Constants.NODE );


			} else {
				//clones wall cell and adds its status to the list
				refscript.cellref.Add(Instantiate (wallNode, myPos, Quaternion.identity));
				refscript.cell_types.Add (Constants.WALLNODE );
			}

			//adds cell position to list
			refscript.cell_id.Add(i+1, new Vector2(cX,cY));
			refscript.cell_num.Add (i + 1, (int)cY * gridUnits + cX);
			cX += 1;
			yield return 0;
			//moves 6 pixels after each clone 
			transform.position = new Vector3 (transform.position.x + 6f, transform.position.y, 0);


			//if my x-position is greater than 54, start new row
			if (this.transform.position.x > (gridUnits - 1) * 6) {
				cY += 1;
				cX = 0;
				transform.position = new Vector3 (0, transform.position.y - 6f, 0); 
			}


		}
		//breadth-first-search 

		//initializing status list
		for (int i = 0; i < (gridUnits * gridUnits); i++) {
			refscript.cell_status.Add(false);
			refscript.parents.Add (-1);
		}
		//adds first node to queue
		refscript.BreadthCoords.Enqueue (0);
		while (refscript.BreadthCoords.Count != 0) {
			//deletes first element from queue
			refscript.currentNode = refscript.BreadthCoords.Dequeue ();
			//refscript.cellref [1].GetComponent<SpriteRenderer> ().color = Color.red; 
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

		int destination = 9;//gridUnits * gridUnits - (gridUnits / 2);
		refscript.cellref [destination].name = "destination";
		while (refscript.parents [destination ] != 0) {
			refscript.cellref [refscript.parents [destination]].name = "Googy";
			destination = refscript.parents [destination];
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
