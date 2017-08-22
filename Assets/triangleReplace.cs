using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class triangleReplace : transition {
	float position;
	int speed;
	ColorBuffer c1=pool.GetBuffer();
	List<int> used;
	int[] marked=new int[60];
	Color replacecolor;

	public override void init()
	{
		used=new List<int>();
		for(int i=0;i<60;i++)
		{
			used.Add(i);
			marked[i]=0;
		}
		speed=Random.Range(3,5);
		replacecolor=randomColor(0f);
	}

	public override void kill ()
	{
		base.kill ();
		pool.ReturnBuffer(c1);
	} 

	public override bool buildFrame(Color[] display,FullBallEffect e1,FullBallEffect e2,Vector3[] points)
	{
		int n=-1;
		for(int a=0;a<speed;a++)
		{
			if(used.Count>0)
			{
				int x=Random.Range(0,used.Count);
				n=used[x];
				used.Remove(n);
				marked[n]=1;
			}
		}
		position-=0.1f;
		e1.buildFrame(c1.colors,points);
		e2.buildFrame(display,points);
		for(int i=0;i<1500;i++)
		{
			int j=i/25;
			switch(marked[j])
			{
			case 0:
				display[i]=c1[i];
				break;
			case 1:
				display[i]=replacecolor;
				break;
			}
		}
		for(int i=0;i<60;i++)
			if(marked[i]==1)
				marked[i]=2;
		return used.Count==0;
	}
}
