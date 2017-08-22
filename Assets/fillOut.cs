using UnityEngine;
using System.Collections;

public class fillOut : transition {
	float position;
	float speed;
	ColorBuffer c1=pool.GetBuffer();

	public override void init()
	{
		position=0f;
		speed=Random.value*0.02f+0.02f;
	}

	public override void kill ()
	{
		base.kill ();
		pool.ReturnBuffer(c1);
	}

	public override bool buildFrame(Color[] display,FullBallEffect e1,FullBallEffect e2,Vector3[] points)
	{
		position+=speed;
		e1.buildFrame(c1.colors,points);
		e2.buildFrame(display,points);
		for(int panel=0;panel<60;panel++)
		{
			int x=panel*25;
			Vector3 anchor=points[x];
			for(int y=0;y<25;y++)
			{
				int z=x+y;
				Vector3 p=points[z];
				p=p-anchor;
				float d=p.magnitude;
				if(position<d)
					display[z]=c1[z];
			}
		}
		return position>.5f;
	}
}
