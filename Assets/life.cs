using UnityEngine;
using System.Collections;

public class life : FullBallEffect {
	byte[] lastframe=new byte[1500];
	byte[] thisframe=new byte[1500];

	Color[] show=new Color[]
	{
		Color.black,
		Color.red,
		Color.black,
		Color.black,
		Color.blue
	};

	byte randtype()
	{
		byte n=(byte)Random.Range(0,5);
		if(n==2||n==3)
			n=0;
		return n;
	}

	public override void init ()
	{
		show[1]=randomColor(0f);
		show[4]=randomColor(0f);
		for(int i=0;i<1500;i++)
			lastframe[i]=randtype();
	}

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		// randomize it a little
		for(int j=0;j<20;j++)
			lastframe[Random.Range(0,1500)]=randtype();
		for(int i=0;i<1500;i++)
		{
			thisframe[i]=lastframe[i];
			int x=i*3;
			int n=0;
			for(int j=0;j<3;j++)
				n+=lastframe[neighbors[x+j]];
			switch(n)
			{
			case 12:
				if(lastframe[i]==0) thisframe[i]=4; //	[j]	444	0	->	4	(junction repair)
				if(lastframe[i]==4) thisframe[i]=1; //	[d]	441	4	->	1	(junction)
				break;
			case 9:
				if(lastframe[i]==0) thisframe[i]=4; //	[k]	441	0	->	4	(junction)
				break;
			case 8:
				if(lastframe[i]==1) thisframe[i]=0; //  [e]	440	1	->	0	(junction)
				if(lastframe[i]==0) thisframe[i]=4; //  [g]	440	0	->	4	(gap repair, collision, and junction)
				break;
			case 6:
				if(lastframe[i]==0) thisframe[i]=4; //  [f]	411	0	->	4	(junction)
				break;
			case 5:
				if(lastframe[i]==0) thisframe[i]=4; //  [c]	410	0	->	4	(wire)
				if(lastframe[i]==4) thisframe[i]=1; //  [a]	410	4	->	1	(wire)
				if(lastframe[i]==1) thisframe[i]=0; //  [i]	410	1	->	0	(junction)
				break;
			case 4:
				if(lastframe[i]==1) thisframe[i]=0; //  [b]	400	1	->	0	(wire)
				break;
			case 1:
				if(lastframe[i]==1) thisframe[i]=4; //  [h]	100	1	->	4	(collision, junction)
				if(lastframe[i]==4) thisframe[i]=1; //  [m]	100	4	->	1	(repair wire end; period 3 oscillator)
				break;
			case 0:
				if(lastframe[i]==1) thisframe[i]=4; //  [n]	000	1	->	4	(repair wire end)
				break;
			}
			display[i]=show[thisframe[i]];
		}
		byte[] swap=thisframe;
		thisframe=lastframe;
		lastframe=swap;
	}
}

/*
[j]	444	0	->	4	(junction repair)
[d]	441	4	->	1	(junction)
[k]	441	0	->	4	(junction)
[e]	440	1	->	0	(junction)
[g]	440	0	->	4	(gap repair, collision, and junction)
[f]	411	0	->	4	(junction)
[c]	410	0	->	4	(wire)
[a]	410	4	->	1	(wire)
[i]	410	1	->	0	(junction)
[b]	400	1	->	0	(wire)
[h]	100	1	->	4	(collision, junction)
[m]	100	4	->	1	(repair wire end; period 3 oscillator)
[n]	000	1	->	4	(repair wire end)
rest	???	x	->	x	(no change)  
 */ 
