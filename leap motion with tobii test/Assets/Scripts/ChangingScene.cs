using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangingScene : MonoBehaviour {

	// Use this for initialization

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void switchFromReadToRecord()
	{
		SceneManager.LoadScene ("LeapWithTobiiRecording", LoadSceneMode.Single);
	}
	public void switchFromRecordToRead()
	{
		SceneManager.LoadScene ("LeapWithTobiiReading",	LoadSceneMode.Single);
	}
	void OnApplicationQuit()
	{
		SceneManager.LoadScene ("LeapWithTobiiRecording", LoadSceneMode.Single);
	}
}
