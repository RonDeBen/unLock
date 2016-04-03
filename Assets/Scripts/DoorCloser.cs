using UnityEngine;
using System.Collections;

public class DoorCloser : MonoBehaviour {

    Animator anim;
    public float waitTime = 2f;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        OpenDoors();
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

    public void OpenDoors()
    {
        MusicMiddleware.playSound("unlockDoor");
        anim.SetTrigger("Open");
        LineSegment.RemoveAllLines();
    }
}
