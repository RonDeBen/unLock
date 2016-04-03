using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DecryptionManager : MonoBehaviour {

	public float secondsForTimer;
	private static int locksLeft;
	private static float startTime;
	public GameObject timerObj;
	private Text timerText;
	private static bool clockIsTicking = false;

	public GameObject lock1, lock2, lock3;

	private static GameObject[] locks = new GameObject[3];

    string lockPrompt = "Assign Pattern Lock";
    string hackFail = "Hack Failed";

	void Start () {
		timerText = timerObj.GetComponent<Text>();
		locksLeft = 3;

		locks[0] = lock3;
		locks[1] = lock2;
		locks[2] = lock1;
	}
	
	void FixedUpdate () {
		if(clockIsTicking){
			float totalSeconds = (secondsForTimer - (Time.time - startTime));
			if (totalSeconds <= 0f){
				DecryptorLose();
			}
			float minutesLeft = Mathf.Floor(totalSeconds / 60);
			float secondsLeft = (int)(totalSeconds % 60);
			timerText.text = minutesLeft.ToString() + ":" + secondsLeft.ToString("00");
		}
	}

	public static void BreakLock(){
		locksLeft--;

		locks[locksLeft].SetActive(false);//change this to modifying a sprite/playing a sound when a lock breaks

		if(locksLeft == 0){
			DecryptorLose();
		}
	}

	public static void StartTimer(){
		clockIsTicking = true;
		startTime = Time.time;
	}

	public static void StopTimer(){
		clockIsTicking = false;
	}

	public static void DecryptorLose(){
		MusicMiddleware.pauseSound("Locksmith");
		StopTimer();
		Debug.Log("You Lose!");
        GameObject doors =GameObject.FindGameObjectWithTag("doors");
        doors.GetComponent<DoorCloser>().StartCloseDoors();
        LineSegment.ShowSolution();
	}
}
