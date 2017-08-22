using UnityEngine;
using System.Collections;

public class colorSparkle : FullBallEffect {
	int count;
	ColorBuffer persistance=pool.GetBuffer();
	Color color;
	Vector3 fade;
	public override void init()
	{
		float speed=Random.value*.3f+.1f;
		fade=new Vector3(speed,speed,speed);
		color=(Random.value>.5f)?Color.white: randomColor(0f);
		count=Random.Range(20,50);
		for(int i=0;i<1500;i++)
			persistance[i]=Color.black;
	}

	public override void init (int i, string[] argv)
	{
		float speed=.5f*.3f+.1f;
		fade=new Vector3(speed,speed,speed);
		color=Color.white;
		count=30;
		for(i=0;i<1500;i++)
			persistance[i]=Color.black;
	}
	public override void kill ()
	{
		base.kill ();
		pool.ReturnBuffer(persistance);
	}

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		for(int i=0;i<1500;i++)
		{
			persistance.colors[i].r-=fade.x;
			persistance.colors[i].g-=fade.y;
			persistance.colors[i].b-=fade.z;
			if(persistance[i].r<0)
				persistance[i]=Color.black;
		}
		for(int i=0;i<count;i++)
		{
			persistance.colors[Random.Range(0,1500)]=color;
		}
		for(int i=0;i<1500;i++)
		{
			display[i]=persistance[i];
		}
	}
}
