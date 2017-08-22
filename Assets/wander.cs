using UnityEngine;
using System.Collections;

public class wander : FullBallEffect {

	class entity{
		public int location;
		public int last;
		public Color color;
		public int step;
		public string program;
	}
	entity[] entities;

	static string[] programs=new string[]
	{
		"lr",
		"rlrlrlr",
		"~",
		"lrlr?",
		"lrlrlrlb",
	};

	public override void init()  // select the color
	{
		int count=Random.Range(20,40);
		int what=Random.Range(0,programs.Length);
		entities=new entity[count];
		for(int i=0;i<count;i++)
			entities[i]=new entity();
		int chance=Random.Range(0,4);

		for(int i=0;i<entities.Length;i++)
		{
			entity thing=entities[i];
			thing.color=randomColor(0f);
			thing.location=Random.Range(0,1500);
			thing.last=neighbors[(thing.location*3)+Random.Range(0,3)];
			thing.step=0;
			thing.program=programs[(chance==0)?Random.Range(0,programs.Length):what];

		}
	}


	public override void buildFrame(Color[] display,Vector3[] points)
	{
		for(int i=0;i<1500;i++)
			display[i]=Color.black;
		for(int i1=0;i1<entities.Length;i1++)
		{
			entity thing=entities[i1];
			int j=thing.location*3;
			int x=0;
			// find last position
			for(x=0;x<3;x++)
			{
				if(thing.last==neighbors[j+x])
					break;
			}
			// pick a random other direction
			switch(thing.program[thing.step])
			{
			case 'l':	x+=1; break;
			case 'r':   x+=2; break;
			case 'b':	break;
			case '~':	x+=Random.Range(1,3); break;
			case '?':	x=Random.Range(0,3); break;
			}
			x%=3;
			thing.step++;
			thing.step%=thing.program.Length;
			// move
			thing.last=thing.location;
			thing.location=neighbors[x+j];
			// paint
			x=thing.location;
			j=x*3;
			display[x]=thing.color;
			for(int i=0;i<3;i++)
				display[neighbors[i+j]]=thing.color;
		}
	}


}
