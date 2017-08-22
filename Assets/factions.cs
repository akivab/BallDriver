using UnityEngine;
using System.Collections;

public class factions : FullBallEffect {
	byte[] lastframe=new byte[1500];
	byte[] thisframe=new byte[1500];

	Color[] teams=new Color[]
	{
		Color.black,
		Color.black,
		Color.red,
		Color.blue,
		Color.green,
		Color.yellow,
		Color.magenta,
		Color.cyan,
		Color.white,
	};
	int teamcount;
	public override void init()
	{
		teamcount = 9;
//		for (int i = 2; i < teamcount; i++)
//			teams [i] = randomColor (0);
		for (int i = 0; i < 1500; i++)
			lastframe [i] = 0;
	}
	public override void buildFrame(Color[] display,Vector3[] points)
	{
		int x = 0;
		for (int i = 0; i < 3; i++)
			lastframe [Random.Range(0,1500)] =(byte) Random.Range (1, teamcount);
		for (int i = 0; i < 1500; i++) 
		{
			int y = lastframe [i];
			if(Random.Range(0,4)==0)
				y = lastframe [neighbors[x + Random.Range (0, 3)]];
			if (y > 0) {
				thisframe [i] = (byte)y;
				display [i] =teams [y];
			} else
				display [i] = Color.black;
			x += 3;
		}
		for (int i = 0; i < 1500; i++)
			lastframe [i] = thisframe [i];
	}
}
