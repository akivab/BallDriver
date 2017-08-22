using UnityEngine;
using System.Collections;

public class rgbLayers : FullBallEffect {
	Color color;
	ColorBuffer green=pool.GetBuffer();
	ColorBuffer blue=pool.GetBuffer();
	FullBallEffect er;
	FullBallEffect eg;
	FullBallEffect eb;

	public override void init()
	{
		er=makeRandomEfffect();
		eg=makeRandomEfffect();
		eb=makeRandomEfffect();
	}

	public override void kill ()
	{
		base.kill ();
		pool.ReturnBuffer(green);
		pool.ReturnBuffer(blue);
		er.kill();
		eg.kill();
		eb.kill();
	}
	public override void buildFrame(Color[] display,Vector3[] points)
	{
		er.buildFrame(display,points);
		eg.buildFrame(green.colors,points);
		eb.buildFrame(blue.colors,points);
		for(int i=0;i<1500;i++)
		{
			display[i].g=green[i].r;
			display[i].b=blue[i].r;
		}
	}
}
