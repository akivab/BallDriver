using UnityEngine;
using System.Collections;

public class fillers : FullBallEffect {

	int[] fillPoints;
	Color[] fillColors;
	byte[] field=new byte[1500];
	int speed;

	public override void init ()
	{
		int l=Random.Range(6,10);
		fillPoints=new int[l];
		fillColors=new Color[l];
		for(int i=0;i<fillPoints.Length;i++)
		{
			fillPoints[i]=Random.Range(0,1500);
			fillColors[i]=randomColor(0);
		}
		for(int i=0;i<1500;i++)
			field[i]=255;
		speed=Random.Range(1,7);
	}

	void advance(byte n)
	{
		int x=fillPoints[n]*3;
		for(int j=0;j<10;j++)
		{
			int y=neighbors[x+Random.Range(0,3)];
			if(field[y]!=n)
			{
				field[y]=n;
				fillPoints[n]=y;
				return;
			}	
		}
		fillPoints[n]=Random.Range(0,1500);
	}

	public override void buildFrame (Color[] display, Vector3[] points)
	{
		for(byte i=0;i<fillPoints.Length;i++)
		{
			for(int x=0;x<speed;x++)
				advance(i);
		}
		for(int i=0;i<1500;i++)
		{
			byte c=field[i];
			if(c==255)
				display[i]=Color.black;
			else
				display[i]=fillColors[c];
		}
	}
}
