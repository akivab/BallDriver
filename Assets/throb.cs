using UnityEngine;
using System.Collections;

public class throb : FullBallEffect {
	FullBallEffect eff;
	float angle;
	float speed;

	public override void init()
	{
		eff=makeRandomEffectOrSolid(.75f);
		speed=(Random.value*8f-4f)/10f;
		angle=0f;
	}

	public override void kill ()
	{
		base.kill ();
		eff.kill();
	}
	public override void buildFrame(Color[] display,Vector3[] points)
	{
		angle+=speed;
		float level=Mathf.Sin(angle);
		level+=1f;
		level*=.25f;
		level+=.5f;
		eff.buildFrame(display,points);
		for(int i=0;i<1500;i++)
		{
			display[i].r=(display[i].r*level);
			display[i].g=(display[i].g*level);
			display[i].b=(display[i].b*level);
		}
	}
}
