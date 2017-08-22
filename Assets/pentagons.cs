using UnityEngine;
using System.Collections;

public class pentagons : FullBallEffect {
	FullBallEffect eff;

	static string list="CBCDEDCBBAABBCDEDCBCEDEAAABCBAEDCBCDEDEAAEDEDCBCDEABCBAAAEDE";
	static int[] source=new int[]{39,33,32,37,38};

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
			int destpanel=list[i/25]-'A';
			int sourcepanel=source[destpanel];
			display[i]=display[(i%25)+(sourcepanel*25)];
		}
	}
}
