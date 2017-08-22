using UnityEngine;
using System.Collections;

public class Solid : FullBallEffect {
	Color color;

	public override void init()  // select the color
	{
		color=randomColor(0f);  // zero probability that it wil lbe black
	}

	public override void init (int i, string[] argv)
	{
		color=new Color(int.Parse(argv[i])/255f,int.Parse(argv[i+1])/255f,int.Parse(argv[i+2])/255f);
		return;
	}

	// display is a collection of vect3 which are the colors of the 1500 pixels
	// points are the x,y,z locations of the points in a sphere with a radius slightly less than one.
	// display and points are each 1500 long and have the same index.

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		for(int i=0;i<1500;i++)
		{
			display[i]=color;
		}
	}
}
