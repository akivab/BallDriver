using UnityEngine;
using System.Collections;

public class overlay : FullBallEffect {
	FullBallEffect eff;
	int frame;
	Color color1;
	static string[] list=new string[]{"ABCD","JPRQSWXY","EFGHI","GLNMN","ACQRXY"};
	string which;

	public override void init()
	{
		eff=makeRandomEfffect();
		color1=randomColor(0f);
		which=list[Random.Range(0,list.Length)];
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
			for(int j=0;j<which.Length;j++)
			{
				char c=which[j];
				display[(c-'A')+i]=color1;
			}
		}
	}
}
