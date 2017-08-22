using UnityEngine;
using System.Collections;

public class spiral : FullBallEffect {
	float spin;
	float speed;
	Color color1;
	int sections;
	float twist;
	FullBallEffect eff;

	// twist -1 1,speed +-4 12,count 1 6,r,g,b,effect...
	public override void init (int i, string[] argv)
	{
		twist=(float)int.Parse(argv[i]);
		speed=(float)int.Parse(argv[i+1]);
		int count=int.Parse(argv[i+2]);
		spin=0f;
		color1=new Color(int.Parse(argv[i+3])/255f,int.Parse(argv[i+4])/255f,int.Parse(argv[i+5])/255f);
		sections=360/count;
		eff=master.makeArgEfffect(argv[i+6],i+7,argv);
	}
	public override void init()
	{
		twist=(float)(Random.Range(0,2)*2-1);
		speed=(Random.value*8f+4f)*(Random.Range(0,2)*2-1);
		spin=0f;
		color1=randomColor(.5f);
		int count=Random.Range(1,6)*2;
		sections=360/count;
		master.loaded+=" "+count;
		eff=makeRandomEffectOrSolid(.5f);
	}

	public override void kill ()
	{
		base.kill ();
		eff.kill();
	}
	public override void buildFrame(Color[] display,Vector3[] points)
	{
		eff.buildFrame(display,points);
		spin+=speed;
		spin+=360;
		spin%=360;
		for(int i=0;i<1500;i++)
		{
			float twistx=master.polar[i].x+360;
			float twisty=twist*master.polar[i].y;
			int angle=(int)(twistx+twisty+spin);
			if(((angle/sections)%2)==0)
				display[i]=color1;
		}
	}
}
