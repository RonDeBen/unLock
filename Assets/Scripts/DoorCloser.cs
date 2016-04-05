using UnityEngine;
using System.Collections;

public class DoorCloser : MonoBehaviour {

    Animator anim;
    private bool closed;
    public float waitTime = 5f;
    public float openDoorPause = 0.3f;
    public GameObject playerControllerObj;
    private PlayerController PC;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        StartCoroutine(StartOpenDoors());
        PC = playerControllerObj.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartCloseDoors()
    {
        closed = true;
        StartCoroutine("CloseDoors");
    }

    IEnumerator CloseDoors()
    {
        yield return new WaitForSeconds(waitTime);
        closed = true;
        MusicMiddleware.playSound("doorClose2");
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(3f);
        PC.EnableNewEncryption();
    }

    IEnumerator StartOpenDoors(){
        anim.SetTrigger("Open");
        LineSegment.RemoveAllLines();
        yield return new WaitForSeconds(openDoorPause);
        MusicMiddleware.playSound("unlockDoor");
        yield return new WaitForSeconds(2f);
        closed = false;
    }

    public void OpenDoorsIfClosed(){
        if(closed){
            StartCoroutine(StartOpenDoors());
        }
    }
}
