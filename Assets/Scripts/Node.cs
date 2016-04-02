using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public int number, row, column, edgesIn;

    public LineSegment lineSegment;

    public TextMesh texMexsh;

	// Use this for initialization
	void Start () {
		texMexsh = gameObject.GetComponentInChildren<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DrawSegment(Material mat, float width)
    {
        LineSegment ls = GameObject.Instantiate(lineSegment);
        ls.gameObject.tag = "lineSegment";
        ls.line_mat = mat;
        ls.line_width = width;
        ls.startpoint = transform.position;
        ls.StartCoroutine("StartDrawing");
    }
}
