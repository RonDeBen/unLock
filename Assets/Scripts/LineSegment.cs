using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineSegment : MonoBehaviour {

    public static LineSegment activeSegment = null;
    public static List<LineSegment> drawnLines = new List<LineSegment>();
    public static LineSegment[] solution;

    public Material line_mat;
    public float line_width;

    public Vector3 startpoint;
    public LineRenderer lineSegment;
    private bool isDrawing = false;

    public Vector3 topLeft, bottomRight;

    public Texture2D[] wireTextures;
    public float timeForNextFrame;
    private float lastFrameTime;
    private int currentFrame = 0;

    // Use this for initialization
    void Start () {
        drawnLines.Add(this);
        lastFrameTime = Time.time;
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

    void FixedUpdate(){
        if(Time.time > (lastFrameTime + timeForNextFrame)){
            lastFrameTime = Time.time;
            currentFrame = (currentFrame + 1) % wireTextures.Length;
            for(int k = 0; k < drawnLines.Count; k++){
                drawnLines[k].line_mat.mainTexture = wireTextures[currentFrame];
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
        if(drawnLines.Count > 0){
            foreach(LineSegment segment in drawnLines) { 
                GameObject.Destroy(segment.gameObject);
            }
            drawnLines.Clear();
        }
    }

    public static void RemoveAllSolutionLines(){
        for(int k = 0; k < solution.Length; k++){
            Debug.Log("fat cocks");
            GameObject.Destroy(solution[k]);
        }
    }

    public static void FinalLineSolution(){
        GameObject[] allLines = GameObject.FindGameObjectsWithTag("lineSegment");

        foreach(GameObject segment in allLines){
            GameObject.Destroy(segment);
        }
        drawnLines.Clear();
    }

    public static void HideAllLines()
    {

        GameObject temp = drawnLines[drawnLines.Count - 1].gameObject;
        drawnLines.RemoveAt(drawnLines.Count - 1);
        solution = drawnLines.ToArray();
        foreach(LineSegment segment in solution)
        {
            segment.lineSegment.enabled = false;
        }
        drawnLines.Clear();
        Destroy(temp);
    }

    public static void ShowSolution()
    {
        RemoveAllLines();
        foreach (LineSegment segment in solution)
        {
            segment.lineSegment.enabled = true;
        }
    }

    private void LoopTextures(){
        if(Time.time > (lastFrameTime + timeForNextFrame)){

        }
    }
}
