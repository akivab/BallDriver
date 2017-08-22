using UnityEngine;
using System.Collections;

public class edgechase : FullBallEffect {
	FullBallEffect eff;
	int frame;
	Color color1;
	Color color2;

	public override void init()
	{
		eff=makeRandomEfffect();
		color1=randomColor(0f);
		color2=Color.black;
	}

	public override void kill ()
	{
		base.kill ();
		eff.kill();
	}
	public override void buildFrame(Color[] display,Vector3[] points)
	{
		frame++;
		eff.buildFrame(display,points);
		for(int i=0;i<1500;i+=25)
		{
			for(int j=16;j<25;j++)
			{
				display[i+j]=((j+frame)%9)<5?color1:color2;
			}
				
		}
	}
}
