using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public int number, row, column;

    public LineSegment lineSegment;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DrawSegment()
    {
        LineSegment ls = GameObject.Instantiate(lineSegment);
        ls.startpoint = transform.position;
        ls.StartCoroutine("StartDrawing");
    }
}
