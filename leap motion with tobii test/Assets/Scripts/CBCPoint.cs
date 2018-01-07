using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBCPoint {

	private int X,Y;

	public CBCPoint()
	{X = -1;Y = -1;}
	public CBCPoint(int x,int y)
	{X = x;Y = y;}
	public CBCPoint(CBCPoint point)
	{X = point.getX ();Y = point.getY ();}
	public CBCPoint(Vector2 locationVector)
	{X = ((int)(locationVector.x));Y = ((int)(locationVector.y));}

	public int getX()
	{return X;}
	public int getY()
	{return Y;}
	public void setX(int x)
	{X = x;}
	public void setY(int y)
	{Y = y;}
	//Return true if p is in radius r with current point;
	public bool inRadius(CBCPoint p,int r)
	{
		if (p == null)
			return false;
		int d = (int)(distance (p));
		return d<=r;
	}
	//return the distance of current point to p;
	public float distance(CBCPoint p)
	{
		if (p == null)
			return -1;
		int x = p.getX ();
		int y = p.getY ();
		float distance = Mathf.Sqrt (Mathf.Pow (X - x, 2) + Mathf.Pow (Y - y, 2));
		return distance;
	}
	public Vector2 toVector2()
	{
		return new Vector2 (X, Y);
	}
	public Vector3 toVector3()
	{
		return new Vector3(X, Y,0);
	}
}
