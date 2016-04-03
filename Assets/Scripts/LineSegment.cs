using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineSegment : MonoBehaviour {

    public static LineSegment activeSegment = null;
    public static List<LineSegment> drawnLines = new List<LineSegment>();

    public Material line_mat;
    public float line_width;

    public Vector3 startpoint;
    public LineRenderer lineSegment;
    private bool isDrawing = false;

    public Vector3 topLeft, bottomRight;

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        if(isDrawing && Input.GetMouseButton(0)){
            Vector3 worldPoint = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            if(worldPoint.x > topLeft.x && worldPoint.x < bottomRight.x && worldPoint.y < topLeft.y && worldPoint.y > bottomRight.y){
                lineSegment.SetPosition(1, worldPoint);
            }
        }
	}

    public IEnumerator StartDrawing()
    {
        lineSegment = gameObject.GetComponent<LineRenderer>();
        lineSegment.SetVertexCount(2);
        isDrawing = true;
        lineSegment.SetPosition(0, startpoint);
        activeSegment = this;
        isDrawing = true;
        lineSegment.material = line_mat;
        lineSegment.SetWidth(line_width, line_width);
        lineSegment.SetPosition(1, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0));
        yield return null;
    }

    public void FinishDrawing(Vector3 endpoint)
    {
        isDrawing = false;
        lineSegment.SetPosition(1, endpoint);
    }


    /// <summary>
    /// Stops drawing the active segment and makes it invisible
    /// </summary>
    public static void StopAllDrawing()
    {
        activeSegment.lineSegment.SetVertexCount(0);
        activeSegment = null;
    }

    public static void RemoveAllLines(){
        GameObject[] segments = GameObject.FindGameObjectsWithTag("lineSegment");

        for(int k = 0; k < segments.Length; k ++){
            Destroy(segments[k]);
        }
    }
}
