using UnityEngine;
using System.Collections;

public class ColorWash : FullBallEffect {
	Vector3 position;
	Vector3 speed;

	float getspeed()
	{
		return (Random.value*8f-4f)/10f;
	}

	public override void init (int i, string[] argv)
	{
		init();
	}
	public override void init()
	{
		speed=new Vector3(getspeed(),getspeed(),getspeed());
	}

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		position+=speed;
		for(int i=0;i<1500;i++)
		{
			Vector3 v=points[i]*5f;
			v+=position;
			Color sin=new Color((Mathf.Sin(v.x)+1f)*.5f,(Mathf.Sin(v.y)+1f)*.5f,(Mathf.Sin(v.z)+1f)*.5f);
			display[i]=sin;
		}
	}

}
