using UnityEngine;
using System.Collections;

public class crossfade : transition {
	float fade;
	ColorBuffer c1=pool.GetBuffer();

	public override void init()
	{
		fade=1f;
	}

	public override void kill ()
	{
		base.kill ();
		pool.ReturnBuffer(c1);
	}
	public override bool buildFrame(Color[] display,FullBallEffect e1,FullBallEffect e2,Vector3[] points)
	{
		fade-=0.05f;
		if(fade<0f)
			fade=0f;
		e1.buildFrame(c1.colors,points);
		e2.buildFrame(display,points);
		float other=1f-fade;
		for(int i=0;i<1500;i++)
		{
			display[i].r=c1[i].r*fade+display[i].r*other;
			display[i].g=c1[i].g*fade+display[i].g*other;
			display[i].b=c1[i].b*fade+display[i].b*other;
		}
		return fade==0;
	}
}
