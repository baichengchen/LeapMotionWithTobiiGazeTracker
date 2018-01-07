using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedArrayList<CBCPoint> : ArrayList{
	//1/Time.deltaTime
	private const int limit=50;

	public LimitedArrayList()
	{}
	public bool isFull()
	{
		return Count == limit;
	}
	public int addPoint(CBCPoint value)
	{
		while (Count >= limit) {
			RemoveAt (0);
		}
		return base.Add (value);
	}
	public CBCPoint firstPoint()
	{return ((CBCPoint)(ToArray ()[0]));}
}
