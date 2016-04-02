using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineSegment : MonoBehaviour {

    public static LineSegment activeSegment = null;
    public static List<LineSegment> drawnLines = new List<LineSegment>();

    public Vector3 startpoint;
    public LineRenderer lineSegment;
    private bool isDrawing = false;

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator StartDrawing()
    {
        lineSegment = gameObject.GetComponent<LineRenderer>();
        lineSegment.SetVertexCount(2);
        isDrawing = true;
        lineSegment.SetPosition(0, startpoint);
        activeSegment = this;
        while (isDrawing)
        {
            lineSegment.SetPosition(1, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0));
            yield return null;
        }
    }

    public void FinishDrawing(Vector3 endpoint)
    {
        isDrawing = false;
        lineSegment.SetPosition(1, endpoint);
    }

    public static void StopAllDrawing()
    {
        activeSegment = null;
    }
}
