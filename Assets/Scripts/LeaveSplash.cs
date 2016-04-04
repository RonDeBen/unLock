using UnityEngine;
using System.Collections;

public class LeaveSplash : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey || Input.touches.Length > 0)
        {
            Application.LoadLevel(0);
        }
	}
}
