using UnityEngine;
using System.Collections;

public class tiling : FullBallEffect {
	FullBallEffect eff;

	public override void init()
	{
		eff=makeRandomEffectOrSolid(0f);
	}
	public override void kill ()
	{
		base.kill ();
		eff.kill();
	}

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		eff.buildFrame(display,points);
		for(int i=0;i<1500;i++)
		{
			display[i]=display[(i%25)+75];
		}
	}
}
