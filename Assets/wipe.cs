using UnityEngine;
using System.Collections;

public class wipe : transition {
	Vector3 normal;
	Color border;
	float position;
	float speed;
	ColorBuffer c1;

	public override void init()
	{
		c1=pool.GetBuffer();
		normal=Random.onUnitSphere;
		border=randomColor(.1f);
		position=1.2f;
		speed=Random.value*0.3f+0.05f;
	}

	public override void kill ()
	{
		base.kill ();
		pool.ReturnBuffer(c1);
	}

	public override bool buildFrame(Color[] display,FullBallEffect e1,FullBallEffect e2,Vector3[] points)
	{
		position-=speed;
		Plane cut=new Plane(normal,position);
		e1.buildFrame(c1.colors,points);
		e2.buildFrame(display,points);
		for(int i=0;i<1500;i++)
		{
			float distance=cut.GetDistanceToPoint(points[i]);
			if(distance>.1)
				display[i]=c1[i];
			else if(distance>-.1)
				display[i]=border;
		}
		return position<-1.2;
	}
}
