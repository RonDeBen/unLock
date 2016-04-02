using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridMaker : MonoBehaviour {

	public struct Coords{
        public int x, y;

        public Coords(int x, int y){
            this.x = x;
            this.y = y;
        }
    }

    [HideInInspector]
    public int startNode, endNode;
    [HideInInspector]
    public int[] edgesEntering;
    [HideInInspector]
    public List<int> edges = new List<int>();


	public GameObject topLeft, bottomRight;
	public int columnCount;
	public GameObject nodeObject;

	[HideInInspector]
	public float box_width, box_height;

	private float width, height;

	// Use this for initialization
	void Start () {
		width = bottomRight.transform.position.x - topLeft.transform.position.x;
		height = topLeft.transform.position.y - bottomRight.transform.position.y;
		box_width = width / columnCount;
		box_height = height / columnCount;

		float start_x = topLeft.transform.position.x + box_width / 2;
		float start_y = topLeft.transform.position.y - box_height / 2;

		int number = 0;
		for(int column = 0; column < columnCount; column++){
			float y = start_y - box_height * column;
			for(int row = 0; row < columnCount; row++){
				float x = start_x + (box_width * row);
				Vector3 nodePos = new Vector3(x, y ,0);
				Node node = nodeObject.GetComponent<Node>();
				node.number = number;
				node.row = row;
				node.column = column;
				Instantiate(nodeObject, nodePos, Quaternion.identity);
				number++;
			}
		}
	}
}
