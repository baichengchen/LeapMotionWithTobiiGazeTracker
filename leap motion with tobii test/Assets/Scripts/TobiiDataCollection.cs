﻿using System.Collections;
using UnityEngine;
using Tobii.Gaming;
using System.IO;

public class TobiiDataCollection : MonoBehaviour {

	private LimitedArrayList<CBCPoint> points;
	private const float VisualizationDistance = 10f;
	private const int toleranceRadius = 50;
	public Transform objectToShow;

	void Start () {
		points = new LimitedArrayList<CBCPoint> ();

		string path = "Assets/Resources/tobiidata.csv";
		StreamWriter writer = new StreamWriter(path, false);
		writer.WriteLine("Timestamp,"+"gazePoint.Viewport.x,"+"gazePoint.Viewport.y,"+"gazePointV2.x,"+"gazePointV2.y,"+"Average");
		writer.Close ();
		Debug.Log ("Estimated Frame Per Second:"+1/Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
		GazePoint gazePoint = TobiiAPI.GetGazePoint();
		Vector2 gazePointV2 = TobiiAPI.GetGazePoint ().Screen;
		//Location is calculated by ViewPort from the bottom left.
		string path = "Assets/Resources/tobiidata.csv";
		StreamWriter writer = new StreamWriter (path, true);
		//Project to world method//
		//Vector3 p = gazePointV2+(transform.forward * VisualizationDistance);
		//This brings the on screen point 10 units forward.   Original:(1920,1080,0)->After:(1920,1080,10)
		writer.WriteLine (gazePoint.Timestamp+","+gazePoint.Viewport.x+","+gazePoint.Viewport.y+","+gazePointV2.x+","+gazePointV2.y+","+averageDist());
		writer.Close ();
		points.addPoint (new CBCPoint(gazePointV2));
		//If focused on the first point, pop something up
		if (wasFocused ()) {
			Vector3 coordinate = coordinateLocation (points.firstPoint ());
			print ("Focused on:"+points.firstPoint().toVector3()+".........."+coordinate);
			Instantiate(objectToShow, coordinate, Quaternion.identity);

			points.Clear ();
		}
		if (points.Count >=1 && averageDist ()==-1) {
			points.Clear ();
		}
		if (!TobiiAPI.GetUserPresence ().IsUserPresent ()) {
			points.Clear ();
		}
	}
	public Vector3 coordinateLocation(CBCPoint point)
	{
		Vector3 pt = point.toVector3 ();
		pt += (transform.forward * VisualizationDistance);
		return Camera.main.ScreenToWorldPoint(pt);
	}
	public LimitedArrayList<CBCPoint> getPoints()
	{
		return points;
	}
	public bool wasFocused()
	{
		if (!points.isFull ()) {
			return false;
		}
		return averageDist()>=0 && averageDist() < toleranceRadius;
	}
	public int averageDist()
	{
		if (points.Count == 0) {
			return -1;
		}
		int totalDis = 0;
		CBCPoint p = points.firstPoint ();
		foreach (CBCPoint point in points.ToArray()) {
			if (!point.isValid ()) {
				return -1;
			}
			float dist = (p.distance (point));
			totalDis += (int)dist;
		}
		return totalDis/30;
	}
}
