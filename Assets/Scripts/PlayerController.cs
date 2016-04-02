using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public struct Coords{
        public int x, y;

        public Coords(int x, int y){
            this.x = x;
            this.y = y;
        }
    }

    private int[] primes = new int[] {1,2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97,101,103,107,109,113,127,131,137,139,149}; 

	private List<int> nodes = new List<int>();
	private List<Vector3> nodePoints = new List<Vector3>();
	private List<Coords> nodeCoords = new List<Coords>();

	private List<int> edges = new List<int>();

	private LineRenderer lr;

	public GridMaker GM;

	private RuntimePlatform platform = Application.platform;

	private float box_width, box_height;

	// Use this for initialization
	void Start () {	
		lr = gameObject.GetComponent<LineRenderer>();
	}
	
	void Update () {
		if(platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer){
			//do later
		}
		else if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer){
			if (Input.GetMouseButton(0)){

				Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(worldPoint,Vector2.zero);

				if (nodePoints.Count != 0){
					lr.SetPosition(nodePoints.Count - 1, worldPoint);
				}

				CheckPosition(hit);

			}
		}
	}

	void CheckPosition(RaycastHit2D hit){
		if(hit.collider != null && hit.collider.gameObject.tag == "node"){
			Node node = hit.collider.gameObject.GetComponent<Node>();
			if(nodes.Count == 0){
				AddNode(node);
			}
			else{
				if(nodes[nodes.Count - 1] != node.number){//the new node is not the same as the last node
					if(Mathf.Abs(node.row - nodeCoords[nodeCoords.Count - 1].x) == 1 || Mathf.Abs(node.column - nodeCoords[nodeCoords.Count - 1].y) == 1){//the node is not adjacent
						if(!edges.Contains(primes[node.number] * primes[nodes[nodes.Count-1]])){//if the edge doesn't already exist
							AddNode(node);
						}
					}
				}
			}
		}
	}

	void AddNode(Node node){

		if(nodes.Count > 0){
			int edge = primes[nodes[nodes.Count-1]]*primes[node.number];
			edges.Add(edge);
		}

		nodes.Add(node.number);
		nodePoints.Add(node.transform.position);
		nodeCoords.Add(new Coords(node.row, node.column));

		lr.SetVertexCount(nodePoints.Count + 1);
		for (int k = 0; k < nodePoints.Count; k++){
			lr.SetPosition(k, nodePoints[k]);
		}
	}

	// private int[] GetPrimes(int order){
	// 	int[] primes = new int[order];
	// 	primes[0] = 1;

	// 	int last_product = 1;

	// 	for(int k = 1; k < order; k++){
	// 		primes[k] = last_product + 1;
	// 		last_product *= primes[k];
	// 		Debug.Log("prime: " + primes[k]);
	// 	}
	// 	return primes;
	// }

}