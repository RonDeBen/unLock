using UnityEngine;
using System.Collections;

public class DoorCloser : MonoBehaviour {

    Animator anim;
    public float waitTime = 2f;
    public float openDoorPause = 0.3f;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        // OpenDoors();
        StartCoroutine(StartOpenDoors());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartCloseDoors()
    {
        StartCoroutine("CloseDoors");
    }

    IEnumerator CloseDoors()
    {
        yield return new WaitForSeconds(waitTime);
        MusicMiddleware.playSound("doorClose2");
        anim.SetTrigger("Close");
    }

    IEnumerator StartOpenDoors(){
        anim.SetTrigger("Open");
        LineSegment.RemoveAllLines();
        yield return new WaitForSeconds(openDoorPause);
        MusicMiddleware.playSound("unlockDoor");
    }
}
