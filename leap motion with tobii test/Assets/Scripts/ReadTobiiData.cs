using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ReadTobiiData : MonoBehaviour {

	// Use this for initialization
	public Transform objectToShow;

	private string[] lines;
	private float[] timeStamps;
	private float[] ViewPortXs;
	private float[] ViewPortYs;
	private float[] ScreenXs;
	private float[] ScreenYs;
	private float[] Averages;
	private bool[] Focuses;

	private LineRenderer LR;

	private int frameIndex = 0;
	private const float VisualizationDistance = 10f;

	void Start () {
		string path = "Assets/Resources/tobiidata.csv";
		StreamReader reader = new StreamReader (path);
		string text = reader.ReadToEnd ();
		reader.Close ();
		lines = text.Split ('\n');
		int dataLength = lines.Length - 2;
		timeStamps = new float[dataLength];
		ViewPortXs = new float[dataLength];
		ViewPortYs = new float[dataLength];
		ScreenXs = new float[dataLength];
		ScreenYs = new float[dataLength];
		Averages = new float[dataLength];
		Focuses = new bool[dataLength];

		try
		{	
			int index = 0;
			foreach (string line in lines)
			{
				if(line.StartsWith("Time"))
				{
					continue;
				}
				if(index == lines.Length-2)
				{
					break;
				}
				string[] data = line.Split (',');
				//timeStamp
				//print(data[0]+".Time");
				timeStamps[index] = float.Parse(data[0]);
				//gazepoint.viewport.x
				//print(data[1]+".x1");
				ViewPortXs[index] = float.Parse(data[1]);
				//gazepoint.viewport.y
				//print(data[2]+".y1");
				ViewPortYs[index] = float.Parse(data[2]);
				//gazepoint.screen.x
				//print(data[3]+".x2");
				ScreenXs[index] = float.Parse(data[3]);
				//gazepoint.screen.y
				//print(data[4]+".y2");
				ScreenYs[index] = float.Parse(data[4]);
				//average
				//print(data[5]+".avg");
				Averages[index] = float.Parse(data[5]);
				//Move to next
				index = index+1;
				//Focusing or not
				/*Use a better data structure than arraylist
				string[] focusdata = data[6].Split('+');
				Focuses[index] = bool.Parse(focusdata[0]);
				if(focusdata.Length>1)
				{
					gazedPoints.Add(focusdata[1]+","+data[7]+")");
				}
				*/
			}
		}
		catch(IOException e) {
			Debug.Log ("..................................................................................Error reading data\n"+e.Message);
		}
		/////////////////////////////////////////////////////////////////////////////////
		////////Draw the gaze path////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////
		/// 

		LR = this.gameObject.AddComponent<LineRenderer> ();
		LR.startWidth = 0.2f;
		LR.endWidth = 0.2f;
		LR.startColor = Color.black;
		LR.endColor = Color.blue;

		Vector3[] allPoints = getVector3ArrayForPoints();
		Vector3[] trimmedData = trim (allPoints);
		for (int x = 0; x < trimmedData.Length; x++) {
			trimmedData [x] = coordinateLocation (trimmedData [x]);
			print (trimmedData [x]+"..");
			Instantiate (objectToShow, trimmedData[x], Quaternion.identity);
		}
		LR.SetPositions (trimmedData);

	}
	public Vector3 coordinateLocation(CBCPoint point)
	{return coordinateLocation (point.toVector3 ());}
	public Vector3 coordinateLocation(Vector3 point)
	{
		Vector3 pt = point;
		pt += (transform.forward * VisualizationDistance);
		return Camera.main.ScreenToWorldPoint(pt);
	}
	Vector3[] getVector3ArrayForPoints()
	{
		Vector3[] pointData = new Vector3[ScreenXs.Length];
		for (int x = 0; x < ScreenXs.Length; x++) {
			pointData [x] = new Vector3 (ScreenXs[x],ScreenYs[x],0);
		}
		return pointData;
	}
	Vector3[] trim(Vector3[] points)
	{
		int total = points.Length;
		int index = 0;
		foreach( Vector3 point in points)
		{
			if (float.IsNaN (point.x) || Averages [index] == -1) {
				total-=1;
			}
			index+=1;
		}
		index = 0;
		int indexForAVG = 0;
		Vector3[] results = new Vector3[total];
		foreach( Vector3 point in points)
		{
			if ((!float.IsNaN (point.x)) && Averages [indexForAVG] != -1) 
			{
				results [index] = point;
				index+=1;
			}
			indexForAVG +=1;
		}
		return results;
	}
	//Update is called once per frame
	void Update () {
		
	}
}

