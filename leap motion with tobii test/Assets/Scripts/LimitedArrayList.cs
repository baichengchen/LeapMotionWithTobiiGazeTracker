using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedArrayList<CBCPoint> : ArrayList{
	//1/Time.deltaTime
	private int limit=50;

	public LimitedArrayList()
	{}
	public LimitedArrayList(int lmt)
	{limit = lmt;}
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
	public CBCPoint getPoint(int index)
	{
		return (CBCPoint)(ToArray()[index]);
	}
}
