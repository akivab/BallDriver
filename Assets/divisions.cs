using UnityEngine;
using System.Collections;

public class divisions : FullBallEffect {
	ColorBuffer colors=pool.GetBuffer();
	int style;

	static int[] groups=new int[]
	{
		0,10,10,10,7,7,7,1,2,2,7,7,
		0,2,2,2,9,9,9,1,4,4,9,9,
		0,4,4,4,11,11,11,1,6,6,11,11,
		0,6,6,6,3,3,3,1,8,8,3,3,
		0,8,8,8,5,5,5,1,10,10,5,5
	};

	public override void init (int i, string[] argv)
	{
		for(int j=0;j<1500;j++)
			colors.colors[j]=randomColor(0f);
		style=int.Parse(argv[1]);
	}
	public override void init()
	{
		for(int i=0;i<1500;i++)
			colors.colors[i]=randomColor(0f);
		style=Random.Range(0,3);
		master.loaded+=" "+style;
//		Debug.Log("style "+style+"\n");
	}

	public override void kill ()
	{
		base.kill ();
		pool.ReturnBuffer(colors);
	}

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		for(int i=0;i<1500;i++)
		{
			int panels=i/25;
			switch(style)
			{
			case 0:
				display[i]=colors[i];
				break;
			case 1:
				display[i]=colors[panels];
				break;
			case 2:
				display[i]=colors[groups[panels]];
				break;
				
			}
		}
	}
}
