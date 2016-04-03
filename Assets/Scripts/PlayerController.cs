using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public struct Coords{
        public int x, y;

        public Coords(int x, int y){
            this.x = x;
            this.y = y;
        }
    }

    private int[] primes = new int[] {1,2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97,101,103,107,109,113,127,131,137,139,149}; 

    private bool isSolving = false;
    private bool startPurgatory = false;

	private List<int> nodes = new List<int>();
	private List<Vector3> nodePoints = new List<Vector3>();
	private List<Coords> nodeCoords = new List<Coords>();

	private List<int> edges = new List<int>();
	private List<int> winningEdges = new List<int>();

	private int[] edgesIn;

	public GridMaker GM;

	public Material line_mat;
	public float line_width;

	private RuntimePlatform platform = Application.platform;

	private float box_width, box_height;

	private int startNode, endNode;

	public GameObject encryptorClearButton, finishButton, lock1, lock2, lock3, decryptorClearButton, timer, timerBackground, startButton;

	private Vector3 topLeft, bottomRight;

	// Use this for initialization
	void Start () {	
		edgesIn = new int[GM.columnCount * GM.columnCount];
	}
	
	void Update () {

		
		if(Input.touches.Length > 0){
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

			CheckPosition(hit);
		}
		
		
		if (Input.GetMouseButton(0)){
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint,Vector2.zero);

			CheckPosition(hit);
		}
		
	}

	void CheckPosition(RaycastHit2D hit){
		if(!startPurgatory){
			if(hit.collider != null && hit.collider.gameObject.tag == "node"){
				Node node = hit.collider.gameObject.GetComponent<Node>();
				if(nodes.Count == 0){
                    LineSegment.RemoveAllLines();//This line resolves the drawing a line from the endpoint problem
                    if (isSolving && node.number == startNode){
                        
						AddNode(node);
					}
					else if(!isSolving){
						AddNode(node);
					}
				}
				else{
					if(nodes[nodes.Count - 1] != node.number){//the new node is not the same as the last node
						if(Mathf.Abs(node.row - nodeCoords[nodeCoords.Count - 1].x) <= 1 && Mathf.Abs(node.column - nodeCoords[nodeCoords.Count - 1].y) <= 1){//the node is not adjacent
							if(!edges.Contains(primes[node.number] * primes[nodes[nodes.Count-1]])){//if the edge doesn't already exist
								if(isSolving){
									if(node.edgesIn > 0){
										if(node.number == endNode){
											if(node.edgesIn > 1){
												AddNode(node);
												CheckNonzeroNodeNumbers();
											}else{
												if(LastOneStanding()){
													AddNode(node);
													CheckNonzeroNodeNumbers();
												}
											}
										}else{
											AddNode(node);
											CheckNonzeroNodeNumbers();
										}									

									}
								}
								else{
									AddNode(node);
								}
							}
						}
					}
				}
			}
		}
	}

	bool LastOneStanding(){
		int total = 0;
		GameObject[] nodeObjs = GameObject.FindGameObjectsWithTag("node");
		for(int k = 0; k < nodeObjs.Length; k ++){
			Node thisNode = nodeObjs[k].GetComponent<Node>();
			total += thisNode.edgesIn;
		}
		return (total == 1);
	}

	void AddNode(Node node){

		if(nodes.Count > 0){
			int edge = primes[nodes[nodes.Count-1]]*primes[node.number];
			edges.Add(edge);
		}

		if(!isSolving){//the puzzle maker is playing
			node.edgesIn++;
			edgesIn[node.number]++;
		}

		if(isSolving){
			node.edgesIn--;
			node.texMexsh.text = node.edgesIn.ToString();
		}

		nodes.Add(node.number);
		nodePoints.Add(node.transform.position);
		nodeCoords.Add(new Coords(node.row, node.column));


        //stops current line segment
        if(LineSegment.activeSegment != null)
        {
            LineSegment.activeSegment.FinishDrawing(node.transform.position);
        }

        node.DrawSegment(line_mat, line_width, GM.topLeft.transform.position, GM.bottomRight.transform.position);//Draws a new line segment


	}

	public void OnStartButtonClicked(){
		MusicMiddleware.loopSound("Locksmith", true);

		startButton.SetActive(false);
		startPurgatory = false;

		PopulateEdgesIn();
		nodes.Clear();
		nodePoints.Clear();
		nodeCoords.Clear();
		edges.Clear();

		isSolving = true;

		lock1.SetActive(true);
		lock2.SetActive(true);
		lock3.SetActive(true);
		decryptorClearButton.SetActive(true);
		timer.SetActive(true);
		timerBackground.SetActive(true);

		DecryptionManager.StartTimer();
	}

	public void OnFinishButtonClicked(){
		startNode = nodes[0];
		endNode = nodes[nodes.Count - 1];

		for(int k = 0; k < edges.Count; k++){
			winningEdges.Add(edges[k]);
		}

		winningEdges.Sort();

        if (isSolving)
        {
            LineSegment.RemoveAllLines();
        }
        else
        {
            LineSegment.HideAllLines();
        }

		finishButton.SetActive(false);
		encryptorClearButton.SetActive(false);

		startButton.SetActive(true);

		startPurgatory = true;
	}

	public void OnEncryptorClearButtonClicked(){
		LineSegment.RemoveAllLines();
		nodes.Clear();
		nodePoints.Clear();
		nodeCoords.Clear();
		edges.Clear();
		/*for(int k = 0; k < edgesIn.Length; k++){
			edgesIn[k] = 0;
		} */

		GameObject[] nodeObjs = GameObject.FindGameObjectsWithTag("node");

		for(int k = 0; k < nodeObjs.Length; k ++){
			Node thisNode = nodeObjs[k].GetComponent<Node>();
			thisNode.edgesIn = 0;
		}
	}

	public void OnDecryptorClearButtonClicked(){
		
		nodes.Clear();
		nodePoints.Clear();
		nodeCoords.Clear();
		edges.Clear();
		PopulateEdgesIn();
        LineSegment.RemoveAllLines();
    }

	public void RestartLevel(){
		Application.LoadLevel(Application.loadedLevel);
	}


	void CheckNonzeroNodeNumbers(){
		GameObject[] nodeObjs = GameObject.FindGameObjectsWithTag("node");
		bool validShape = true;
		for(int k = 0; k < nodeObjs.Length; k++){
			Node thisNode = nodeObjs[k].GetComponent<Node>();
			if(thisNode.edgesIn != 0){
				validShape = false;
			}
		}
		if(validShape){
			CheckShape();
		}
	}

	void CheckShape(){
        int hits = 0;
		edges.Sort();
		bool same = true;
		for(int k = 0; k < edges.Count; k ++){
            if (edges[k] != winningEdges[k])
            {
                same = false;
            }
            else
                hits++;
		}
        int wrong = edges.Count - hits;
		if(same){
			Debug.Log("You Got It!");
			DecryptionManager.StopTimer();
		}else{
            string count = "Correct egdes: " + hits + " Incorrect egdes: " + wrong;
			Debug.Log(count);
			// DecryptionManager.BreakLock();
			OnDecryptorClearButtonClicked();
		}
	}

	void PopulateEdgesIn(){

		GameObject[] nodeObjs = GameObject.FindGameObjectsWithTag("node");

		for(int k = 0; k < nodeObjs.Length; k ++){
			Node thisNode = nodeObjs[k].GetComponent<Node>();
			thisNode.texMexsh.text = edgesIn[thisNode.number].ToString();
			if(!isSolving){
				if(thisNode.number == nodes[0]){
					thisNode.texMexsh.text += "S";
				}
				if(thisNode.number == nodes[nodes.Count - 1]){
					thisNode.texMexsh.text += "E";
				}
			}else{
				thisNode.edgesIn = edgesIn[thisNode.number];
				if(thisNode.number == startNode){
					thisNode.texMexsh.text += "S";
				}
				if(thisNode.number == endNode){
					thisNode.texMexsh.text += "E";
				}
			}
		}
	}

	public void DisableControls(){
		startPurgatory = true;
	}

}