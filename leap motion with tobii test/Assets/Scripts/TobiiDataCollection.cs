using System.Collections;
using UnityEngine;
using Tobii.Gaming;
using System.IO;

public class TobiiDataCollection : MonoBehaviour {

	private LimitedArrayList<CBCPoint> points;
	private const float VisualizationDistance = 10f;
	private const int toleranceRadius = 50;
	public Transform objectToShow;

	private LineRenderer LR;
	//private LineRenderer gazedLR;
	private LimitedArrayList<CBCPoint> gazedPoints;

	void Start () {
		points = new LimitedArrayList<CBCPoint> ();
		gazedPoints = new LimitedArrayList<CBCPoint> ();

		string path = "Assets/Resources/tobiidata.csv";
		StreamWriter writer = new StreamWriter(path, false);
		writer.WriteLine("Timestamp,"+"gazePoint.Viewport.x,"+"gazePoint.Viewport.y,"+"gazePointV2.x,"+"gazePointV2.y,"+"Average,Focuses");
		writer.Close ();
		Debug.Log ("Estimated Frame Per Second:"+1/Time.deltaTime);

		Debug.Log ("Drawing");

		LR = this.gameObject.AddComponent<LineRenderer> ();
		//gazedLR = this.gameObject.AddComponent<LineRenderer> ();

		LR.startWidth = 0.2f;
		LR.endWidth = 0.2f;
		LR.startColor = Color.black;
		LR.endColor = Color.blue;
		/*
		gazedLR.startWidth = 0.2f;
		gazedLR.endWidth = 0.2f;
		gazedLR.startColor = Color.blue;
		gazedLR.endColor = Color.black;
		*/
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
		writer.Write (gazePoint.Timestamp+","+gazePoint.Viewport.x+","+gazePoint.Viewport.y+","+gazePointV2.x+","+gazePointV2.y+","+averageDist());

		points.addPoint (new CBCPoint(gazePointV2));
		//If focused on the first point, pop something up
		if (wasFocused ()) {
			Vector3 coordinate = coordinateLocation (points.getPoint (0));

			writer.WriteLine (",True+"+points.getPoint (0).toVector3 ());

			Instantiate (objectToShow, coordinate, Quaternion.identity);
			gazedPoints.addPoint (points.getPoint (0));
			points.Clear ();
		
		} else {
			writer.WriteLine(",False");
		}
		writer.Close ();
		if (points.Count >=1 && averageDist ()==-1) {
			points.Clear ();
		}
		if (!TobiiAPI.GetUserPresence ().IsUserPresent ()) {
			points.Clear ();
		}
		Vector3[] data = new Vector3[points.Count];
		LR.positionCount = data.Length;
		if (points.Count > 0) {
			for (int x = 0; x < data.Length; x++) {
				data [x] = coordinateLocation(((CBCPoint)(points.getPoint (x))).toVector3 ());
			}
		}
		LR.SetPositions (data);
		/*
		Vector3[] gazedData = new Vector3[points.Count];
		gazedLR.positionCount = gazedData.Length;
		if (gazedPoints.Count > 0) {
			for (int x = 0; x < gazedData.Length; x++) {
				gazedData [x] = coordinateLocation(((CBCPoint)(gazedPoints.getPoint (x))).toVector3 ());
			}
		}
		gazedLR.SetPositions(gazedData);
		*/
	}
	public Vector3 coordinateLocation(CBCPoint point)
	{return coordinateLocation (point.toVector3 ());}
	public Vector3 coordinateLocation(Vector3 point)
	{
		Vector3 pt = point;
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
		CBCPoint p = points.getPoint(0);
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
