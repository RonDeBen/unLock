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

	public GameObject playerControllr;
	private static PlayerController PC;

	private static GameObject[] locks = new GameObject[3];

    string lockPrompt = "Assign Pattern Lock";
    string hackFail = "Hack Failed";

	void Start () {
		PC = playerControllr.GetComponent<PlayerController>();

		timerText = timerObj.GetComponent<Text>();
		locksLeft = 3;
	}
	
	void FixedUpdate () {
		if(clockIsTicking){
			float totalSeconds = (secondsForTimer - (Time.time - startTime));
			if (totalSeconds <= 0f){
				DecryptorLose();
			}else{
				float minutesLeft = Mathf.Floor(totalSeconds / 60);
				float secondsLeft = (int)(totalSeconds % 60);
				timerText.text = minutesLeft.ToString() + ":" + secondsLeft.ToString("00");
			}
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
		PC.DisableControls();
		PC.EndGame();
		StopTimer();
		Debug.Log("You Lose!");
        GameObject doors =GameObject.FindGameObjectWithTag("doors");
        doors.GetComponent<DoorCloser>().StartCloseDoors();
        LineSegment.ShowSolution();
	}
}
