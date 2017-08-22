
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.Networking;

public class UDPSend : MonoBehaviour
{
	model ball;
	animate animator;
	UdpClient client;
	public bool sending=false;
	public bool clip=false;
	int count1=0;
	int count2=0;

	public void Start()
	{
//        if(sending)
		    client= new UdpClient("192.168.1.253",7001);
		ball=this.GetComponent<model>();
		animator=this.GetComponent<animate>();
	}
	byte[] packetData=new byte[304];
	void sendFrame(Color[] frameBuffer)
	{
		packetData[0]=304%256;
		packetData[1]=304/256;
		packetData[2]=255;
		packetData[3]=0;

		int z=0;
		for(int j=0;j<15;j++)
		{
			for(int x=0;x<4;x++)
			{
				int n=x*75+4;
				for(int q=0;q<75;q++)
				{
					int r=(int) (frameBuffer[z].r*254f); if(r<0) r=0; if(r>254) r=254;
					int g=(int) (frameBuffer[z].g*254f); if(r<0) g=0; if(r>254) g=254;
					int b=(int) (frameBuffer[z].b*254f); if(r<0) b=0; if(r>254) b=254;
					z++;
					packetData[n+q]=(byte)r;
					q++;
					packetData[n+q]=(byte)g;
					q++;
					packetData[n+q]=(byte)b;
				}
			}
			// clip top and bottom of ball
			if(clip)
			{
				if((j%3)==0)
				{
					for(int n=4;n<31;n++)
						packetData[n]=0;	
				}
				if((j%3)==1)
				{
					for(int n=229;n<256;n++)
						packetData[n]=0;	
				}
			}
			packetData[2]=(byte)j;
			sendBytes(packetData);
		}
	}
	byte[] latchPacket=new byte[4];
	void sendLatch()
	{
		latchPacket[0]=4;
		latchPacket[1]=0;
		latchPacket[2]=255;
		latchPacket[3]=129;
		sendBytes(latchPacket);
	}

	float timeleft=0f;
	float fps=1f/20f;
	Color[] frameBuffer=new Color[1500];
	void Update()
	{
		timeleft+=Time.deltaTime;
		animator.polar=ball.polar;
		if(timeleft>fps)
		{
			sendLatch();
			while(timeleft>fps){
				timeleft-=fps;
				animator.buildFrame(frameBuffer,ball.points);
			}
			sendFrame(frameBuffer);
		}
//		System.GC.Collect();
	}

	public void sendBytes(byte[]data)
	{
		if(sending)
		{
			count1++;
			client.Send(data, data.Length);
			count2++;
		}
		ball.recieve(data);
	}
	void OnGUI()
	{
		sending=GUI.Toggle(new Rect(0,Screen.height-20,100,20),sending,"Send UDP");
		clip=GUI.Toggle(new Rect(0,Screen.height-60,80,20),clip,"Clip");
	}


}