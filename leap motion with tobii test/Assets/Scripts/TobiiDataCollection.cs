using System.Collections;
using UnityEngine;
using Tobii.Gaming;
using System.IO;

public class TobiiDataCollection : MonoBehaviour {

	private LimitedArrayList<CBCPoint> points;
	private const float VisualizationDistance = 10f;
	private const int toleranceRadius = 20;

	void Start () {
		points = new LimitedArrayList<CBCPoint> ();

		string path = "Assets/Resources/tobiidata.csv";
		StreamWriter writer = new StreamWriter(path, false);
		writer.WriteLine("Timestamp,"+"gazePoint.Viewport.x,"+"gazePoint.Viewport.y,"+"gazePointV2.x,"+"gazePointV2.y,");
		writer.Close ();
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
		writer.WriteLine (gazePoint.Timestamp+","+gazePoint.Viewport.x+","+gazePoint.Viewport.y+","+gazePointV2.x+","+gazePointV2.y);
		writer.Close ();
		points.addPoint (new CBCPoint(gazePointV2));
		if (wasFocused ()) {
			Vector3 focusedPoint = points.firstPoint().toVector3()+(transform.forward * VisualizationDistance);
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.position = focusedPoint;
			points.Clear ();
		}
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
		int totalDis = 0;
		CBCPoint p = points.firstPoint ();
		foreach (CBCPoint point in points.ToArray()) {
			totalDis += ((int)(p.distance(point)));
		}
		return totalDis / 30 < toleranceRadius;
	}
}
