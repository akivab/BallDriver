using UnityEngine;
using System.Collections;

public class dissolve : transition {
	int fade;
	ColorBuffer c1=pool.GetBuffer();

	public override void init()
	{
		fade=0;
	}

	public override void kill ()
	{
		base.kill ();
		pool.ReturnBuffer(c1);
	}

	public override bool buildFrame(Color[] display,FullBallEffect e1,FullBallEffect e2,Vector3[] points)
	{
		fade++;
		e1.buildFrame(c1.colors,points);
		e2.buildFrame(display,points);
		for(int i=0;i<1500;i++)
		{
			display[i]=((i*87)%25)>fade?c1[i]:display[i];
		}
		return fade==25;
	}


}
