using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DuplicateKeyComparer: IComparer<int> 
{
	

	public int Compare(int x, int y)
	{
		int result = x.CompareTo(y);

		if (result == 0)
			return 1;   // Handle equality as beeing greater
		else
			return result;
	}


}
public class all_cells : MonoBehaviour {
	
	public List<GameObject> cellref = new List <GameObject>(); 
	public ArrayList cell_types = new ArrayList ();
	public Hashtable cell_id = new Hashtable();
	public Hashtable cell_num = new Hashtable();
	public List<bool> cell_status = new List <bool> ();
	public List<int> parents = new List <int> ();
	public Queue<int> BreadthCoords = new Queue<int> ();
	public int currentNode = 0; 
	public int tX;
	public int tY;
	public SortedList<int, int> g = new SortedList <int, int> (new DuplicateKeyComparer());
	// Use this for initialization
	void Start () {
		int k;
		Debug.Log ("Start");
		g.Add (-1, 5);
		g.Add (-2, 2);
		g.Add (0, 1);
		g.Add (-10, 4);
		k = g.Keys[0];
		Debug.Log (k);

		g.RemoveAt (0);
		k = g.Keys[0];
		Debug.Log (k);
		g.Add (-3, 6);
		k = g.Keys[0];
		Debug.Log (k);


	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
