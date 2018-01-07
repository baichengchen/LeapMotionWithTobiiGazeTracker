using System.Collections;
using System.Collections.Generic;
public class CBCLine {

	private CBCPoint start;
	private CBCPoint end;

	public CBCLine()
	{
		start = new CBCPoint ();
		end = new CBCPoint ();
	}
	public CBCLine(CBCPoint s,CBCPoint e)
	{
		start = s;
		end = e;
	}
	public CBCLine(CBCLine l)
	{
		start = l.getStart ();
		end = l.getEnd ();
	}
	public CBCLine(int sx,int sy,int ex,int ey)
	{
		start = new CBCPoint (sx, sy);
		end = new CBCPoint (ex, ey);
	}
	public CBCPoint getStart()
	{return start;}
	public CBCPoint getEnd()
	{return end;}
	public void setStart(CBCPoint p)
	{start = p;}
	public void setEnd(CBCPoint p)
	{end = p;}
	public void setStart(int x,int y)
	{start = new CBCPoint (x, y);}
	public void setEnd(int x,int y)
	{end = new CBCPoint (x, y);}
	public int length()
	{
		if(start == null || end == null)
		{return -1;}
		int d = ((int)start.distance (end));
		return d;
	}
}
