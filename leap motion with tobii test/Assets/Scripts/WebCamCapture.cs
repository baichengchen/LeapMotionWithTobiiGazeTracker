using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class WebCamCapture : MonoBehaviour
{
	WebCamTexture text;
	void Start()
	{
		text = new WebCamTexture ();
		text.Play ();
	}
	void Update()
	{
		GetComponent<RawImage> ().texture = text;
	}
}