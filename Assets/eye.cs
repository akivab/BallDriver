using UnityEngine;
using System.Collections;

public class eye : FullBallEffect {
	Color color;
	Vector3 center= new Vector3(1f,0f,0f);
	Vector3 dest=new Vector3(1f,0f,0f);

	Color ball=new Color(.75f,.75f,.75f);
	int framecount;

	Vector3 limity(Vector3 place)
	{
		if(place.y>.2f)
			place.y=.2f;
		if(place.y<-.2f)
			place.y=-.2f;
		place=Vector3.Normalize(place);
		return place;
	}

	Vector3 pickpoint()
	{
		framecount=0;
		Vector3 place=Random.onUnitSphere;
		return limity(place);
	}

	public override void init (int i, string[] argv)
	{
		color=Color.blue;		// blue
		center=pickpoint();
		dest=pickpoint();
	}

	public override void init()
	{
		color=Color.blue;		// blue
		if(Random.value<.1f)
			color=Color.green;		// green
		if(Random.value<.02f)
			color=Color.red;		// red
		center=pickpoint();
		dest=pickpoint();
	}

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		framecount++;
		if(framecount<20)
		{
			center=center-(dest-center)*.15f;
			center=limity(center);
		}
		if(framecount>40)
			dest=pickpoint();
		for(int i=0;i<1500;i++)
		{
			Vector3 point=points[i];
			display[i]=ball;
			if(Vector3.Magnitude(point-center)<.5f)
				display[i]=color;
			if(Vector3.Magnitude(point-center)<.15f)
				display[i]=Color.black;

		}
	}
}
