﻿using UnityEngine;
using System.Collections;
using System.IO;

public class image : FullBallEffect {
	Texture2D source;	
	float spin;
	float speed;
	Color[] pix;

	public void promo (int i)
	{
		//		i += findsplit () + 1;
		Texture2D[] images = master.imageList;
		if (i >= images.Length)
			i = 0;
		promo (images [i]);
	}

	public void promo(Texture2D tmpSource) {
		source = tmpSource;
		speed=(Random.Range(0,2)*8)-4;
		spin=0;
		pix = source.GetPixels(0, 0, source.width, source.height);
		Debug.Log ("Saw image of size: " + source.width + "," + source.height);
		return ;
	}
	public void selectRandomImage() {
		promo(Random.Range(0, master.imageList.Length));
	}
				
	public override void init (int i, string[] argv)
	{
		Debug.Log ("Creating from args.");
		promo(int.Parse(argv[i]));
		return ;
	}


	public static Texture2D LoadPNG(string filePath) {

		Debug.Log ("Loading PNG...");
		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists(filePath))     {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
		}
		return tex;
	}
 

	// display is a collection of vect3 which are the colors of the 1500 pixels
	// points are the x,y,z locations of the points in a sphere with a radius slightly less than one.
	// display and points are each 1500 long and have the same index.

	public override void buildFrame(Color[] display,Vector3[] points)
	{
		spin-=speed;
		spin+=360;
		spin%=360;

		for(int i=0;i<1500;i++)
		{
			float x=master.polar[i].x;
			float y=master.polar[i].y;
			x+=spin;
			x+=360;
			x%=360;
			x/=360f;
			y/=180f;
			y+=.5f;
			x=1f-x;
			x*=source.width;
			y*=source.height;
			int n=(int)y*source.width+(int)x;
			display[i]=pix[n];
		}
	}
}
