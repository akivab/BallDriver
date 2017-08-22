using UnityEngine;
using System.Collections;
using System.IO;

public class RGBSections : FullBallEffect {
	Texture2D source;	
	float spin;
	float speed;
	float updown;
	float updownspeed;
	Color[] pix ;

	FullBallEffect red;
	FullBallEffect green;
	FullBallEffect blue;
	ColorBuffer grnbuf=pool.GetBuffer();
	ColorBuffer blubuf=pool.GetBuffer();


	public override void init()  // select the color
	{
		Texture2D[] images=master.RGBSectionsList;
		source=images[Random.Range(0,images.Length)];
		speed=(Random.Range(0,2)*8)-4;
		updownspeed = (Random.Range (0, 3) * 2) - 2;
		updown = 0;
		spin=0;
		red=makeRandomEffectOrSolid(.5f);
		green=makeRandomEffectOrSolid(.5f);
		blue=makeRandomEffectOrSolid(.5f);
		pix = source.GetPixels(0, 0, source.width, source.height);
	}

	public override void kill ()
	{
		base.kill ();
		red.kill();
		green.kill();
		blue.kill();
		pool.ReturnBuffer(grnbuf);
		pool.ReturnBuffer(blubuf);
	}
	// display is a collection of vect3 which are the colors of the 1500 pixels
	// points are the x,y,z locations of the points in a sphere with a radius slightly less than one.
	// display and points are each 1500 long and have the same index.

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		red.buildFrame(display,points);
		green.buildFrame(grnbuf.colors,points);
		blue.buildFrame(blubuf.colors,points);
		spin-=speed;
		spin+=360;
		spin%=360;
		updown += updownspeed;
		updown += 180;
		updown %= 180;

		for(int i=0;i<1500;i++)
		{
			float x=master.polar[i].x;
			float y=master.polar[i].y;
			y += updown;
			y += 90;
			y %= 180;
			x+=spin;
			x+=360;
			x%=360;
			x/=360f;
			y/=180f;
//			y+=.5f;
			x=1f-x;
			x*=source.width;
			y*=source.height;
			int n=(int)y*source.width+(int)x;
			Color c=pix[n];
			if(c.r>.5)
				c=display[i];
			else if(c.g>.5)
				c=grnbuf[i];
			else if (c.b>.5)
				c=blubuf[i];
				
			display[i]=c;
		}
	}
}
