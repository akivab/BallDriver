using UnityEngine;
using System.Collections;

public class sparkleother : FullBallEffect {
	FullBallEffect eff;
	int chance=Random.Range(10,50);

	public override void init()
	{
		eff=makeRandomEffectOrSolid(0f);
		master.loaded+=" "+chance;
	}

	public override void init(int i, string[] argv)
	{
		chance=int.Parse(argv[i]);
		eff=master.makeArgEfffect(argv[i+1],i+2,argv);
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
			if(Random.Range(0,100)<=chance)
				display[i]=Color.black;	
		}
	}
}
