using UnityEngine;
using System.Collections;

public class halves : FullBallEffect {
	Vector3 normal;
	Vector3 rot;
	float width;
	float speed;

	ColorBuffer left=pool.GetBuffer();
	ColorBuffer right=pool.GetBuffer();
	FullBallEffect effleft;
	FullBallEffect effmid;
	FullBallEffect effright;

	float getspeed()
	{
		return (Random.value*8f-4f)/10f;
	}
	public override void init()
	{
		effleft=makeRandomEffectOrSolid(.25f);
		effmid=makeRandomEffectOrSolid(.75f);
		effright=makeRandomEffectOrSolid(.25f);

		normal=Random.onUnitSphere;
		rot=Random.onUnitSphere;
		width=Random.value*.3f+.1f;
		speed=Random.value*5f+4f;
		if(Random.value<.33)
			width=0f;
	}

	public override void kill ()
	{
		base.kill ();
		effleft.kill();
		effmid.kill();
		effright.kill();
		pool.ReturnBuffer(left);
		pool.ReturnBuffer(right);
	}

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		effleft.buildFrame(left.colors,points);
		effmid.buildFrame(display,points);
		effright.buildFrame(right.colors,points);
		normal=Quaternion.Euler(rot*speed)*normal;
		Plane cut=new Plane(normal,0f);
		for(int i=0;i<1500;i++)
		{
			float distance=cut.GetDistanceToPoint(points[i]);
			if(distance>width)
				display[i]=left[i];
			else if(distance>-width)
				display[i]=right[i];
		}
	}
}
